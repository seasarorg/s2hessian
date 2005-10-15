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
package org.seasar.remoting.client.interceptors;

import java.lang.reflect.Method;

import org.aopalliance.intercept.MethodInvocation;
import org.seasar.framework.aop.interceptors.AbstractInterceptor;
import org.seasar.framework.container.ComponentDef;
import org.seasar.framework.util.ClassUtil;
import org.seasar.framework.util.MethodUtil;
import org.seasar.remoting.client.connector.Connector;

/**
 * リモートオブジェクトのメソッド呼び出しを行うためのインターセプタです。 <br>
 * このインターセプタはJavaインタフェースまたは抽象クラスに適用され、呼び出されたメソッドがターゲットによって実装されていない場合(抽象メソッド)に
 * {@link Connector}に委譲することによりリモートオブジェクトのメソッド呼び出しを行います。 <br>
 * インターセプタはターゲットのコンポーネント定義から名前( <code>&lt;component&gt;</code> 要素の
 * <code>name</code> 属性の値)を取得し、その名前をリモートオブジェクトの名前として {@link Connector#invoke}
 * を呼び出します。コンポーネント定義に名前が定義されていない場合は、コンポーネントの型名( <code>&lt;component&gt;</code>
 * 要素の <code>class</code> 属性の値)からパッケージ名を除いた名前をリモートオブジェクトの名前とします。
 * コンポーネントの型名が定義されていない場合は、ターゲットオブジェクトのクラス名からパッケージ名を除いた名前をリモートオブジェクトの名前とします。
 * もしプロパティ <code>remoteName</code>
 * (オプション)が設定されていれば、その値が常にリモートオブジェクトの名前として使用されます。
 * 
 * @see Connector
 * @author koichik
 */
public class RemotingInterceptor extends AbstractInterceptor {
    protected Connector connector;
    protected String remoteName;

    /**
     * リモート呼び出しを実行する {@link Connector}を設定します。このプロパティは必須です。
     * 
     * @param connector
     *            リモート呼び出しを実行する {@link Connector}
     */
    public void setConnector(final Connector connector) {
        this.connector = connector;
    }

    /**
     * リモートオブジェクトの名前を設定します。このプロパティはオプションです。
     * コンポーネント定義から取得できる名前を使うことが出来ない場合にのみ設定してください。
     * 
     * @param remoteName
     *            リモートオブジェクトの名前
     */
    public void setRemoteName(final String remoteName) {
        this.remoteName = remoteName;
    }

    /**
     * ターゲットのメソッドが起動された時に呼び出されます。起動されたメソッドが抽象メソッドなら {@link Connector}に委譲します。
     * 具象メソッドならターゲットのメソッドを呼び出します。
     * 
     * @param invocation
     *            メソッドの起動情報
     */
    public Object invoke(final MethodInvocation invocation) throws Throwable {
        final Method method = invocation.getMethod();
        if (MethodUtil.isAbstract(method)) {
            return connector.invoke(getRemoteName(invocation), method, invocation
                    .getArguments());
        }
        return invocation.proceed();
    }

    /**
     * リモートオブジェクトの名前を返します。リモートオブジェクトの名前は次の順で解決します。
     * <ul>
     * <li>プロパティ <code>remoteName</code> が設定されていればその値。</li>
     * <li>コンポーネント定義に名前が設定されていればその値。</li>
     * <li>コンポーネント定義に型が設定されていればその名前からパッケージ名を除いた値。</li>
     * <li>ターゲットオブジェクトの型名からパッケージ名を除いた値。</li>
     * </ul>
     * 
     * @param invocation
     *            メソッドの起動情報
     * @return リモートオブジェクトの名前
     */
    protected String getRemoteName(final MethodInvocation invocation) {
        if (remoteName != null) {
            return remoteName;
        }

        final ComponentDef componentDef = getComponentDef(invocation);
        final String componentName = componentDef.getComponentName();
        if (componentName != null) {
            return componentName;
        }

        final Class componentClass = componentDef.getComponentClass();
        if (componentClass != null) {
            return ClassUtil.getShortClassName(componentClass);
        }

        return ClassUtil.getShortClassName(invocation.getThis().getClass());
    }
}