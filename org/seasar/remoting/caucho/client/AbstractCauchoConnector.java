/*
*
* The Seasar Software License, Version 1.1
*
* Copyright (c) 2003-2004 The Seasar Project. All rights reserved.
*
* Redistribution and use in source and binary forms, with or
* without modification, are permitted provided that the following
* conditions are met:
*
* 1. Redistributions of source code must retain the above
*    copyright notice, this list of conditions and the following
*    disclaimer.
*
* 2. Redistributions in binary form must reproduce the above
*    copyright notice, this list of conditions and the following
*    disclaimer in the documentation and/or other materials provided
*    with the distribution.
*
* 3. The end-user documentation included with the redistribution,
*    if any, must include the following acknowledgement:
*    "This product includes software developed by the
*    Seasar Project (http://www.seasar.org/)."
*    Alternately, this acknowledgement may appear in the software
*    itself, if and wherever such third-party acknowledgements
*    normally appear.
*
* 4. Neither the name "The Seasar Project" nor the names of its
*    contributors may be used to endorse or promote products derived
*    from this software without specific prior written permission of
*    the Seasar Project.
*
* THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR
* IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
* PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE SEASAR PROJECT
* OR ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
* INCIDENTAL,SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
* WHETHER IN CONTRACT, STRICT LIABILITY,OR TORT (INCLUDING
* NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
* THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
package org.seasar.remoting.caucho.client;

import java.lang.reflect.Method;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;

import org.seasar.remoting.client.connector.impl.URLBasedConnector;

import com.caucho.services.client.ServiceProxyFactory;

/**
 * @author mshimura
 */
public abstract class AbstractCauchoConnector extends URLBasedConnector {

    /* (non-Javadoc)
     * @see org.seasar.remoting.client.connector.impl.URLBasedConnector#invoke(java.net.URL, java.lang.reflect.Method, java.lang.Object[])
     */
    protected final ServiceProxyFactory factory;
    protected final Map proxyCache = new HashMap();

    protected AbstractCauchoConnector(final ServiceProxyFactory factory) {
        this.factory = factory;
    }
    protected Object invoke(URL targetURL, Method method, Object[] args)
            throws Throwable {
        final Class targetClass = method.getDeclaringClass();
        Object proxy;
        synchronized (this) {
            proxy = proxyCache.get(targetClass);
            if (proxy == null) {
                proxy = factory.create(targetClass, targetURL.toString());
                proxyCache.put(targetClass, proxy);
            }
        }
        return method.invoke(proxy, args);
    }

}
