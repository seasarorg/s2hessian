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
package org.seasar.remoting.common.connector;

import java.lang.reflect.Method;

/**
 * リモートオブジェクトに対するメソッド呼び出しを実行するオブジェクトが実装するインタフェースです。
 * 実装クラスは固有の方法(プロトコル)でリモートオブジェクトのメソッドを呼び出します。
 * 
 * @author koichik
 */
public interface Connector {

    /**
     * リモートメソッド呼び出しを実行し、その結果を返します。
     * 
     * @param name
     *            リモートオブジェクトの名前
     * @param method
     *            呼び出すメソッド
     * @param args
     *            リモートオブジェクトのメソッド呼び出しに渡される引数値を格納するオブジェクト配列
     * @return リモートオブジェクトに対するメソッド呼び出しからの戻り値
     * @throws Throwable
     *             リモートオブジェクトに対するメソッド呼び出しからスローされる例外
     */
    Object invoke(String name, Method method, Object[] args) throws Throwable;
}
