/*
 * Copyright (c) 2001-2004 Caucho Technology, Inc.  All rights reserved.
 *
 * The Apache Software License, Version 1.1
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *
 * 3. The end-user documentation included with the redistribution, if
 *    any, must include the following acknowlegement:
 *       "This product includes software developed by the
 *        Caucho Technology (http://www.caucho.com/)."
 *    Alternately, this acknowlegement may appear in the software itself,
 *    if and wherever such third-party acknowlegements normally appear.
 *
 * 4. The names "Hessian", "Resin", and "Caucho" must not be used to
 *    endorse or promote products derived from this software without prior
 *    written permission. For written permission, please contact
 *    info@caucho.com.
 *
 * 5. Products derived from this software may not be called "Resin"
 *    nor may "Resin" appear in their names without prior written
 *    permission of Caucho Technology.
 *
 * THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED.  IN NO EVENT SHALL CAUCHO TECHNOLOGY OR ITS CONTRIBUTORS
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
 * OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
 * OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR
 * BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * @author Scott Ferguson
 */

package com.caucho.hessian.server;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.io.IOException;

import javax.servlet.Servlet;
import javax.servlet.GenericServlet;
import javax.servlet.ServletConfig;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.ServletException;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.caucho.hessian.io.HessianInput;
import com.caucho.hessian.io.HessianOutput;

import com.caucho.services.server.Service;
import com.caucho.services.server.ServiceContext;

/**
 * Servlet for serving Hessian services.
 */
public class HessianServlet extends GenericServlet {
  private Class _homeAPI;
  private Object _homeImpl;
  
  private Class _objectAPI;
  private Object _objectImpl;
  
  private HessianSkeleton _homeSkeleton;
  private HessianSkeleton _objectSkeleton;
  public String spath;

  public String getServletInfo()
  {
    return "Hessian Servlet";
  }

  /**
   * Sets the home api.
   */
  public void setHomeAPI(Class api)
  {
    _homeAPI = api;
  }

  /**
   * Sets the home implementation
   */
  public void setHome(Object home)
  {
    _homeImpl = home;
  }

  /**
   * Sets the object api.
   */
  public void setObjectAPI(Class api)
  {
    _objectAPI = api;
  }

  /**
   * Sets the object implementation
   */
  public void setObject(Object object)
  {
    _objectImpl = object;
  }

  /**
   * Sets the service class.
   */
  public void setService(Object service)
  {
    setHome(service);
  }

  /**
   * Sets the api-class.
   */
  public void setAPIClass(Class api)
  {
    setHomeAPI(api);
  }

  /**
   * Gets the api-class.
   */
  public Class getAPIClass()
  {
    return _homeAPI;
  }

  /**
   * Initialize the service, including the service object.
   */
  public void init(ServletConfig config)
    throws ServletException
  {
    super.init(config);
    
    try {
      if (_homeImpl != null) {
      }
      else if (getInitParameter("home-class") != null) {
	String className = getInitParameter("home-class");
	
	Class homeClass = loadClass(className);

	_homeImpl = homeClass.newInstance();

	init(_homeImpl);
      }
      else if (getInitParameter("service-class") != null) {
	String className = getInitParameter("service-class");
	
	Class homeClass = loadClass(className);

	_homeImpl = homeClass.newInstance();
	
	init(_homeImpl);
      }
      else {
	if (getClass().equals(HessianServlet.class))
	  throw new ServletException("server must extend HessianServlet");

	_homeImpl = this;
      }

      if (_homeAPI != null) {
      }
      else if (getInitParameter("home-api") != null) {
	String className = getInitParameter("home-api");
	
	_homeAPI = loadClass(className);
      }
      else if (getInitParameter("api-class") != null) {
	String className = getInitParameter("api-class");

	_homeAPI = loadClass(className);
      }
      else if (_homeImpl != null)
	_homeAPI = _homeImpl.getClass();
      
      if (_objectImpl != null) {
      }
      else if (getInitParameter("object-class") != null) {
	String className = getInitParameter("object-class");
	
	Class objectClass = loadClass(className);

	_objectImpl = objectClass.newInstance();

	init(_objectImpl);
      }

      if (_objectAPI != null) {
      }
      else if (getInitParameter("object-api") != null) {
	String className = getInitParameter("object-api");
	
	_objectAPI = loadClass(className);
      }
      else if (_objectImpl != null)
	_objectAPI = _objectImpl.getClass();

      _homeSkeleton = new HessianSkeleton(_homeImpl, _homeAPI);
      if (_objectAPI != null)
	_homeSkeleton.setObjectClass(_objectAPI);

      if (_objectImpl != null) {
	_objectSkeleton = new HessianSkeleton(_objectImpl, _objectAPI);
	_objectSkeleton.setHomeClass(_homeAPI);
      }
      else
	_objectSkeleton = _homeSkeleton;
    } catch (ServletException e) {
      throw e;
    } catch (Exception e) {
      throw new ServletException(e);
    }
  }

  private Class loadClass(String className)
    throws ClassNotFoundException
  {
    ClassLoader loader = Thread.currentThread().getContextClassLoader();

    if (loader != null)
      return Class.forName(className, false, loader);
    else
      return Class.forName(className);
  }

  private void init(Object service)
    throws ServletException
  {
    if (service instanceof Service)
      ((Service) service).init(getServletConfig());
    else if (service instanceof Servlet)
      ((Servlet) service).init(getServletConfig());
  }
  
  /**
   * Execute a request.  The path-info of the request selects the bean.
   * Once the bean's selected, it will be applied.
   */
  public void service(ServletRequest request, ServletResponse response)
    throws IOException, ServletException
  {
    HttpServletRequest req = (HttpServletRequest) request;
    HttpServletResponse res = (HttpServletResponse) response;
    spath=req.getPathInfo();
    System.out.println("Hello: "+spath+"\n");
    if (! req.getMethod().equals("POST")) {
      res.setStatus(500, "Hessian Requires POST");
      PrintWriter out = res.getWriter();

      res.setContentType("text/html");
      out.println("<h1>Hessian Requires POST</h1>");
      
      return;
    }

    String serviceId = req.getPathInfo();
    String objectId = req.getParameter("id");
    if (objectId == null)
      objectId = req.getParameter("ejbid");

    ServiceContext.begin(req, serviceId, objectId);

    try {
      InputStream is = request.getInputStream();
      OutputStream os = response.getOutputStream();

      HessianInput in = new HessianInput(is);
      HessianOutput out = new HessianOutput(os);

      if (objectId != null)
	_objectSkeleton.invoke(in, out);
      else
	_homeSkeleton.invoke(in, out);
    } catch (RuntimeException e) {
      throw e;
    } catch (ServletException e) {
      throw e;
    } catch (Throwable e) {
      throw new ServletException(e);
    } finally {
      ServiceContext.end();
    }
  }
}
