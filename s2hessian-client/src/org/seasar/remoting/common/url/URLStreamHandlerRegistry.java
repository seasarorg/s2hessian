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
package org.seasar.remoting.common.url;

import java.net.URL;
import java.net.URLStreamHandler;
import java.net.URLStreamHandlerFactory;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

import org.seasar.framework.log.Logger;

/**
 * <code>URLStreamHandler</code> のレジストリです。 <br>
 * このレジストリは <code>URLStreamHandlerFactory</code> であり、 <code>URL</code>
 * クラスに設定されます。 {@link #createURLStreamHandler(String)} が呼び出されると、登録されている
 * <code>URLStreamHandler</code> を返します。
 * 
 * @author koichik
 */
public class URLStreamHandlerRegistry implements URLStreamHandlerFactory {

    private static final Logger logger = Logger.getLogger(URLStreamHandlerRegistry.class);
    protected static final Map registry = Collections.synchronizedMap(new HashMap());
    protected static boolean initialized;

    /**
     * インスタンスを構築します。
     */
    private URLStreamHandlerRegistry() {
    }

    /**
     * 指定されたプロトコルのための、 <code>URLStreamHandler</code> の新しいインスタンスを作成します。
     * 
     * @param protocol
     *            プロトコル (<code>rmi</code> など)
     */
    public URLStreamHandler createURLStreamHandler(final String protocol) {
        return (URLStreamHandler) registry.get(protocol);
    }

    /**
     * プロトコルのための新しい <code>URLStreamHandler</code> を登録します。
     * 
     * @param protocol
     *            プロトコル (<code>rmi</code> など
     * @param handler
     *            プロトコルのための <code>URLStreamHandler</code>
     */
    public static void registerHandler(final String protocol, final URLStreamHandler handler) {
        initialize();
        registry.put(protocol, handler);
    }

    /**
     * 
     */
    public static synchronized void initialize() {
        if (initialized) {
            return;
        }

        try {
            URL.setURLStreamHandlerFactory(new URLStreamHandlerRegistry());
        }
        catch (final Throwable e) {
            logger.log("WRMT0001", null, e);
        }
        initialized = true;
    }
}
