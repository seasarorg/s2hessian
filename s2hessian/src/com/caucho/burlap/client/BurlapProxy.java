/*
 * The Apache Software License, Version 1.1
 *
 * Copyright (c) 2001-2004 Caucho Technology, Inc.  All rights reserved.
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
 * 4. The names "Burlap", "Resin", and "Caucho" must not be used to
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

package com.caucho.burlap.client;

import java.io.*;
import java.util.*;
import java.net.*;
import java.lang.reflect.*;
import java.lang.reflect.Proxy;

import com.caucho.burlap.io.*;

/**
 * Proxy implementation for Burlap clients.  Applications will generally
 * use BurlapProxyFactory to create proxy clients.
 */
public class BurlapProxy implements InvocationHandler {
  private BurlapProxyFactory _factory;
  private URL _url;
  
  BurlapProxy(BurlapProxyFactory factory, URL url)
  {
    _factory = factory;
    _url = url;
  }

  /**
   * Returns the proxy's URL.
   */
  public URL getURL()
  {
    return _url;
  }

  /**
   * Handles the object invocation.
   *
   * @param proxy the proxy object to invoke
   * @param method the method to call
   * @param args the arguments to the proxy object
   */
  public Object invoke(Object proxy, Method method, Object []args)
    throws Throwable
  {
    String methodName = method.getName();
    Class []params = method.getParameterTypes();

    // equals and hashCode are special cased
    if (methodName.equals("equals") &&
        params.length == 1 && params[0].equals(Object.class)) {
      Object value = args[0];
      if (value == null || ! Proxy.isProxyClass(value.getClass()))
        return new Boolean(false);

      BurlapProxy handler = (BurlapProxy) Proxy.getInvocationHandler(value);

      return new Boolean(_url.equals(handler.getURL()));
    }
    else if (methodName.equals("hashCode") && params.length == 0)
      return new Integer(_url.hashCode());
    else if (methodName.equals("getBurlapType"))
      return proxy.getClass().getInterfaces()[0].getName();
    else if (methodName.equals("getBurlapURL"))
      return _url.toString();
    else if (methodName.equals("toString") && params.length == 0)
      return "[BurlapProxy " + _url + "]";

    InputStream is = null;
    
    try {
      URLConnection conn = _factory.openConnection(_url);
      conn.setRequestProperty("Content-Type", "text/xml");
    
      OutputStream os;

      try {
	os = conn.getOutputStream();
      } catch (Exception e) {
	throw new BurlapRuntimeException(e);
      }

      BurlapOutput out = _factory.getBurlapOutput(os);

      if (! _factory.isOverloadEnabled()) {
      }
      else if (args != null)
        methodName = methodName + "__" + args.length;
      else
        methodName = methodName + "__0";

      out.call(methodName, args);

      try {
	os.flush();
      } catch (Exception e) {
	throw new BurlapRuntimeException(e);
      }

      if (conn instanceof HttpURLConnection) {
        HttpURLConnection httpConn = (HttpURLConnection) conn;
        int code = 500;

        try {
          code = httpConn.getResponseCode();
        } catch (Exception e) {
        }

        if (code != 200) {
          StringBuffer sb = new StringBuffer();
          int ch;

          try {
            is = httpConn.getInputStream();

            if (is != null) {
              while ((ch = is.read()) >= 0)
                sb.append((char) ch);

              is.close();
            }

            is = httpConn.getErrorStream();
            if (is != null) {
              while ((ch = is.read()) >= 0)
                sb.append((char) ch);
            }
          } catch (FileNotFoundException e) {
            throw new BurlapRuntimeException(String.valueOf(e));
          } catch (IOException e) {
          }

          if (is != null)
            is.close();

          throw new BurlapProtocolException(sb.toString());
        }
      }

      is = conn.getInputStream();

      AbstractBurlapInput in = _factory.getBurlapInput(is);

      return in.readReply(method.getReturnType());
    } catch (BurlapProtocolException e) {
      throw new BurlapRuntimeException(e);
    } finally {
      if (is != null)
        is.close();
    }
  }
}
