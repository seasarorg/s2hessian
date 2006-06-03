/*
 * Copyright 2004-2006 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
package org.seasar.remoting.caucho.client;

import java.lang.reflect.Method;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;

import org.seasar.remoting.common.connector.impl.URLBasedConnector;

import com.caucho.services.client.ServiceProxyFactory;

/**
 * @author mshimura
 */
public abstract class AbstractCauchoConnector extends URLBasedConnector {

    protected final ServiceProxyFactory factory;
    protected final Map proxyCache = new HashMap();

    protected AbstractCauchoConnector(final ServiceProxyFactory factory) {
        this.factory = factory;
    }
	public Object invoke(String name, Method method, Object[] args) throws Throwable {
        final Class targetClass = method.getDeclaringClass();
        Object proxy;
        synchronized (this) {
            proxy = proxyCache.get(targetClass);
            if (proxy == null) {
                proxy = factory.create(targetClass, getBaseURL().toString()+name);
                proxyCache.put(targetClass, proxy);
            }
        }
        return method.invoke(proxy, args);
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
