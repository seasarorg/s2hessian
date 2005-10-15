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
 * �����[�g�I�u�W�F�N�g�̃��\�b�h�Ăяo�����s�����߂̃C���^�[�Z�v�^�ł��B <br>
 * ���̃C���^�[�Z�v�^��Java�C���^�t�F�[�X�܂��͒��ۃN���X�ɓK�p����A�Ăяo���ꂽ���\�b�h���^�[�Q�b�g�ɂ���Ď�������Ă��Ȃ��ꍇ(���ۃ��\�b�h)��
 * {@link Connector}�ɈϏ����邱�Ƃɂ�胊���[�g�I�u�W�F�N�g�̃��\�b�h�Ăяo�����s���܂��B <br>
 * �C���^�[�Z�v�^�̓^�[�Q�b�g�̃R���|�[�l���g��`���疼�O( <code>&lt;component&gt;</code> �v�f��
 * <code>name</code> �����̒l)���擾���A���̖��O�������[�g�I�u�W�F�N�g�̖��O�Ƃ��� {@link Connector#invoke}
 * ���Ăяo���܂��B�R���|�[�l���g��`�ɖ��O����`����Ă��Ȃ��ꍇ�́A�R���|�[�l���g�̌^��( <code>&lt;component&gt;</code>
 * �v�f�� <code>class</code> �����̒l)����p�b�P�[�W�������������O�������[�g�I�u�W�F�N�g�̖��O�Ƃ��܂��B
 * �R���|�[�l���g�̌^������`����Ă��Ȃ��ꍇ�́A�^�[�Q�b�g�I�u�W�F�N�g�̃N���X������p�b�P�[�W�������������O�������[�g�I�u�W�F�N�g�̖��O�Ƃ��܂��B
 * �����v���p�e�B <code>remoteName</code>
 * (�I�v�V����)���ݒ肳��Ă���΁A���̒l����Ƀ����[�g�I�u�W�F�N�g�̖��O�Ƃ��Ďg�p����܂��B
 * 
 * @see Connector
 * @author koichik
 */
public class RemotingInterceptor extends AbstractInterceptor {
    protected Connector connector;
    protected String remoteName;

    /**
     * �����[�g�Ăяo�������s���� {@link Connector}��ݒ肵�܂��B���̃v���p�e�B�͕K�{�ł��B
     * 
     * @param connector
     *            �����[�g�Ăяo�������s���� {@link Connector}
     */
    public void setConnector(final Connector connector) {
        this.connector = connector;
    }

    /**
     * �����[�g�I�u�W�F�N�g�̖��O��ݒ肵�܂��B���̃v���p�e�B�̓I�v�V�����ł��B
     * �R���|�[�l���g��`����擾�ł��閼�O���g�����Ƃ��o���Ȃ��ꍇ�ɂ̂ݐݒ肵�Ă��������B
     * 
     * @param remoteName
     *            �����[�g�I�u�W�F�N�g�̖��O
     */
    public void setRemoteName(final String remoteName) {
        this.remoteName = remoteName;
    }

    /**
     * �^�[�Q�b�g�̃��\�b�h���N�����ꂽ���ɌĂяo����܂��B�N�����ꂽ���\�b�h�����ۃ��\�b�h�Ȃ� {@link Connector}�ɈϏ����܂��B
     * ��ۃ��\�b�h�Ȃ�^�[�Q�b�g�̃��\�b�h���Ăяo���܂��B
     * 
     * @param invocation
     *            ���\�b�h�̋N�����
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
     * �����[�g�I�u�W�F�N�g�̖��O��Ԃ��܂��B�����[�g�I�u�W�F�N�g�̖��O�͎��̏��ŉ������܂��B
     * <ul>
     * <li>�v���p�e�B <code>remoteName</code> ���ݒ肳��Ă���΂��̒l�B</li>
     * <li>�R���|�[�l���g��`�ɖ��O���ݒ肳��Ă���΂��̒l�B</li>
     * <li>�R���|�[�l���g��`�Ɍ^���ݒ肳��Ă���΂��̖��O����p�b�P�[�W�����������l�B</li>
     * <li>�^�[�Q�b�g�I�u�W�F�N�g�̌^������p�b�P�[�W�����������l�B</li>
     * </ul>
     * 
     * @param invocation
     *            ���\�b�h�̋N�����
     * @return �����[�g�I�u�W�F�N�g�̖��O
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