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
package org.seasar.remoting.common.connector.impl;

import java.lang.reflect.Method;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * ターゲットとなるオブジェクトごとに個別のURLを使用してリモートメソッド呼び出しを行うコネクタの抽象基底クラスです。
 * <p>
 * このコネクタはリモートオブジェクトの名前をスーパークラスのプロパティに設定されたベースURLからの相対URLとしてターゲットとなるリモートオブジェクトのURLを解決します。
 * このため，ベースURLがスラッシュ( <code>/</code> )で終了していない場合、予期しない結果になるかもしれないことに注意してください。
 * <p>
 * 例を示します。 <br>
 * ベースURLが次のように設定されているとします。
 * 
 * <pre>
 *     http://host/context/services/
 * </pre>
 * 
 * リモートオブジェクトが次の名前であるとします。
 * 
 * <pre>
 * Foo
 * </pre>
 * 
 * リモートオブジェクトのURLは次のようになります。
 * 
 * <pre>
 *     http://host/context/services/Foo
 * </pre>
 * 
 * ベースURLが次のようにスラッシュ( <code>/</code> )で終了していない場合は結果が異なります。
 * 
 * <pre>
 *     http://host/context/services
 * </pre>
 * 
 * リモートオブジェクトが次の名前であるとします。
 * 
 * <pre>
 * Foo
 * </pre>
 * 
 * リモートオブジェクトのURLは次のようになります。
 * 
 * <pre>
 *     http://host/context/Foo
 * </pre>
 * 
 * @author koichik
 */
public abstract class TargetSpecificURLBasedConnector extends URLBasedConnector {

    // constants
    /**
     * リモートオブジェクトのURLをキャッシュする上限のデフォルト値
     */
    protected static final int DEFAULT_MAX_CACHED_URLS = 10;

    // instance fields
    protected LRUMap cachedURLs = new LRUMap(DEFAULT_MAX_CACHED_URLS);

    /**
     * リモートオブジェクトのURLをキャッシュする上限を設定します。
     * 
     * @param maxCachedURLs
     *            リモートオブジェクトのURLをキャッシュする上限です
     */
    public synchronized void setMaxCachedURLs(final int maxCachedURLs) {
        cachedURLs.setMaxSize(maxCachedURLs);
    }

    /**
     * ターゲットとなるリモートオブジェクトのURLを解決し、サブクラス固有の方法でメソッド呼び出しを実行します。
     * 
     * @param remoteName
     *            リモートオブジェクトの名前
     * @param method
     *            呼び出すメソッド
     * @param args
     *            リモートオブジェクトのメソッド呼び出しに渡される引数値を格納するオブジェクト配列
     * @return リモートオブジェクトに対するメソッド呼び出しからの戻り値
     * @throws Throwable
     *             リモートオブジェクトに対するメソッド呼び出しからスローされた例外です
     */
    public Object invoke(final String remoteName, final Method method, final Object[] args)
            throws Throwable {
        return invoke(getTargetURL(remoteName), method, args);
    }

    /**
     * ターゲットとなるリモートオブジェクトのURLを返します。
     * リモートオブジェクトのURLは、リモートオブジェクトの名前をベースURLからの相対URLとして解決します。
     * 
     * @param remoteName
     *            ターゲットとなるリモートオブジェクトの名前
     * @return ターゲットとなるリモートオブジェクトのURL
     * @throws MalformedURLException
     *             ベースURLとリモートオブジェクトの名前からURLを作成できなかった場合にスローされます
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
     *            ターゲットとなるリモートオブジェクトのURL
     * @param method
     *            呼び出すメソッド
     * @param args
     *            リモートオブジェクトのメソッド呼び出しに渡される引数値を格納するオブジェクト配列
     * @return リモートオブジェクトに対するメソッド呼び出しからの戻り値
     * @throws Throwable
     *             リモートオブジェクトに対するメソッド呼び出しからスローされた例外です
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

        private static final long serialVersionUID = 1L;

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
