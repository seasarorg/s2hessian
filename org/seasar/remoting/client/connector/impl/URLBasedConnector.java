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
package org.seasar.remoting.client.connector.impl;

import java.lang.reflect.Method;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.LinkedHashMap;
import java.util.Map;

import org.seasar.remoting.client.connector.Connector;

/**
 * URLに基づいてリモートオブジェクトのアドレスを解決するコネクタの抽象基底クラスです。
 * <p>
 * このコネクタはベースURLをプロパティに持ち、リモートオブジェクトの名前をベースURLからの相対URLとしてリモートオブジェクトのURLを解決します。
 * このため，ベースURLがスラッシュ( <code>/</code> )で終了していない場合、予期しない結果になるかもしれないことに注意してください。
 * <p>
 * 例を示します。 <br>
 * ベースURLが次のように設定されているとします。
 * 
 * <pre>
 *  http://host/context/services/
 * </pre>
 * 
 * リモートオブジェクトが次の名前であるとします。
 * 
 * <pre>
 *  Foo
 * </pre>
 * 
 * リモートオブジェクトのURLは次のようになります。
 * 
 * <pre>
 *  http://host/context/services/Foo
 * </pre>
 * 
 * ベースURLが次のようにスラッシュ( <code>/</code> )で終了していない場合は結果が異なります。
 * 
 * <pre>
 *  http://host/context/services
 * </pre>
 * 
 * リモートオブジェクトが次の名前であるとします。
 * 
 * <pre>
 *  Foo
 * </pre>
 * 
 * リモートオブジェクトのURLは次のようになります。
 * 
 * <pre>
 *  http://host/context/Foo
 * </pre>
 * 
 * @author koichik
 */
public abstract class URLBasedConnector implements Connector {
    /**
     * リモートオブジェクトのURLをキャッシュする上限のデフォルト値
     */
    protected static final int DEFAULT_MAX_CACHED_URLS = 10;

    protected URL baseURL;
    protected LRUMap cachedURLs = new LRUMap(DEFAULT_MAX_CACHED_URLS);

    /**
     * ベースURLを設定します。
     * 
     * @param baseURL
     *            ベースURL
     */
    public void setBaseURL(final String baseURL) throws MalformedURLException {
        this.baseURL = new URL(baseURL);
    }

    /**
     * リモートオブジェクトのURLをキャッシュする上限を設定します。
     * 
     * @param maxCachedURLs
     *            リモートオブジェクトのURLをキャッシュする上限
     */
    public synchronized void setMaxCachedURLs(final int maxCachedURLs) {
        cachedURLs.setMaxSize(maxCachedURLs);
    }

    /**
     * リモートオブジェクトのURLを解決し、サブクラス固有の方法でメソッド呼び出しを実行します。
     * 
     * @param remoteName
     *            リモートオブジェクトの名前
     * @param method
     *            呼び出すメソッド
     * @param args
     *            リモートオブジェクトのメソッド呼び出しに渡される引数値を格納するオブジェクト配列
     * @return リモートオブジェクトに対するメソッド呼び出しからの戻り値
     * @throws Throwable
     *             リモートオブジェクトに対するメソッド呼び出しからスローされる例外
     */
    public Object invoke(final String remoteName, final Method method, final Object[] args)
            throws Throwable {
        return invoke(getTargetURL(remoteName), method, args);
    }

    /**
     * リモートオブジェクトのURLを返します。
     * リモートオブジェクトのURLは、リモートオブジェクトの名前をベースURLからの相対URLとして解決します。
     * 
     * @param remoteName
     *            リモートオブジェクトの名前
     * @return リモートオブジェクトのURL
     * @throws MalformedURLException
     */
    protected synchronized URL getTargetURL(final String remoteName) throws MalformedURLException {
        URL targetURL = (URL) cachedURLs.get(remoteName);
        if (targetURL == null) {
            targetURL = new URL(baseURL, remoteName);
            cachedURLs.put(remoteName, targetURL);
        }
        return targetURL;
    }

    /**
     * サブクラス固有の方法でリモートメソッドの呼び出しを実行し、その結果を返します。
     * 
     * @param targetURL
     *            リモートオブジェクトのURL
     * @param method
     *            呼び出すメソッド
     * @param args
     *            リモートオブジェクトのメソッド呼び出しに渡される引数値を格納するオブジェクト配列
     * @return リモートオブジェクトに対するメソッド呼び出しからの戻り値
     * @throws Throwable
     *             リモートオブジェクトに対するメソッド呼び出しからスローされる例外
     */
    protected abstract Object invoke(URL targetURL, Method method, Object[] args) throws Throwable;

    /**
     * LRUマップ <br>
     * エントリ数に上限があり、それを超えてエントリが追加された場合にはもっとも使用されていないエントリが取り除かれるマップの実装です。
     * エントリ数の上限は随時増やすことが出来ますが、減らしてもその数までエントリが取り除かれることはありません。 このマップは同期されません。
     * 
     * @author koichik
     */
    protected static class LRUMap extends LinkedHashMap {
        /** デフォルト初期容量 */
        protected static final int DEFAULT_INITIAL_CAPACITY = 16;
        /** デフォルト負荷係数 */
        protected static final float DEFAULT_LOAD_FACTOR = 0.75f;

        protected int maxSize;

        /**
         * デフォルトの初期容量と負荷係数で指定されたエントリ数を上限とするインスタンスを構築します。
         * 
         * @param maxSize
         *            エントリ数の最大数
         */
        public LRUMap(final int maxSize) {
            this(maxSize, DEFAULT_INITIAL_CAPACITY, DEFAULT_LOAD_FACTOR);
        }

        /**
         * 指定された初期容量と負荷係数、エントリ数の上限を持つインスタンスを構築します。
         * 
         * @param maxSize
         *            エントリ数の最大数
         * @param initialCapacity
         *            初期容量
         * @param loadFactor
         *            負荷係数
         */
        public LRUMap(final int maxSize, final int initialCapacity, final float loadFactor) {
            super(initialCapacity, loadFactor, true);
            this.maxSize = maxSize;
        }

        /**
         * エントリ数の最大値を設定します。
         * 
         * @param maxSize
         *            エントリ数の最大数
         */
        public void setMaxSize(final int maxSize) {
            this.maxSize = maxSize;
        }

        /**
         * マップのエントリ数が最大数を超えている場合 <code>true</code> を返します。
         * その結果、最も前にマップに挿入されたエントリがマップから削除されます。
         * 
         * @param eldest
         *            もっとも前にマップに挿入されたエントリ
         * @return マップのエントリ数が最大数を超えている場合 <code>true</code>
         */
        protected boolean removeEldestEntry(final Map.Entry eldest) {
            return maxSize > 0 && size() > maxSize;
        }
    }
}