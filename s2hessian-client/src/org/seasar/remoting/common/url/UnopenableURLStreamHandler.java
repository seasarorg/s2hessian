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
import java.net.URLConnection;
import java.net.URLStreamHandler;

/**
 * デフォルトのポート番号を持ち、オープンすることの出来ない <code>URL</code> のための
 * <code>URLStreamHandler</code> です。
 * 
 * @author koichik
 */
public class UnopenableURLStreamHandler extends URLStreamHandler {

    // instance fields
    protected final int defaultPort;

    /**
     * 指定されたポート番号をデフォルトとして持つ新しいインスタンスを構築します。
     * 
     * @param defaultPort
     *            このプロトコルのデフォルトのポート番号
     */
    public UnopenableURLStreamHandler(final int defaultPort) {
        this.defaultPort = defaultPort;
    }

    /**
     * この操作はサポートされません。
     * 
     * @throws UnsupportedOperationException
     *             常にスローされます
     */
    protected URLConnection openConnection(final URL url) {
        throw new UnsupportedOperationException();
    }

    /**
     * <code>URL</code> 引数フィールド値を、指定された値に設定します。 <br>
     * ポート番号が指定されていない場合は、コンストラクタで指定されたデフォルトのポート番号を設定します。
     * 
     * @param url
     *            修正する <code>URL</code>
     * @param protocol
     *            プロトコル名
     * @param host
     *            <code>URL</code> のリモートホスト値
     * @param port
     *            リモートマシン上のポート
     * @param authority
     *            <code>URL</code> の権限部分
     * @param userInfo
     *            <code>URL</code> のユーザ情報部分
     * @param path
     *            <code>URL</code> のパスコンポーネント
     * @param query
     *            <code>URL</code> のクエリー部分
     * @param ref
     *            参照
     */
    protected void setURL(final URL url, final String protocol, final String host, int port,
            final String authority, final String userInfo, final String path, final String query,
            final String ref) {
        if (port == -1) {
            port = defaultPort;
        }
        super.setURL(url, protocol, host, port, authority, userInfo, path, query, ref);
    }
}
