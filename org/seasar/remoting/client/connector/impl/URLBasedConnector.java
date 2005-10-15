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
 * URL�Ɋ�Â��ă����[�g�I�u�W�F�N�g�̃A�h���X����������R�l�N�^�̒��ۊ��N���X�ł��B
 * <p>
 * ���̃R�l�N�^�̓x�[�XURL���v���p�e�B�Ɏ����A�����[�g�I�u�W�F�N�g�̖��O���x�[�XURL����̑���URL�Ƃ��ă����[�g�I�u�W�F�N�g��URL���������܂��B
 * ���̂��߁C�x�[�XURL���X���b�V��( <code>/</code> )�ŏI�����Ă��Ȃ��ꍇ�A�\�����Ȃ����ʂɂȂ邩������Ȃ����Ƃɒ��ӂ��Ă��������B
 * <p>
 * ��������܂��B <br>
 * �x�[�XURL�����̂悤�ɐݒ肳��Ă���Ƃ��܂��B
 * 
 * <pre>
 *  http://host/context/services/
 * </pre>
 * 
 * �����[�g�I�u�W�F�N�g�����̖��O�ł���Ƃ��܂��B
 * 
 * <pre>
 *  Foo
 * </pre>
 * 
 * �����[�g�I�u�W�F�N�g��URL�͎��̂悤�ɂȂ�܂��B
 * 
 * <pre>
 *  http://host/context/services/Foo
 * </pre>
 * 
 * �x�[�XURL�����̂悤�ɃX���b�V��( <code>/</code> )�ŏI�����Ă��Ȃ��ꍇ�͌��ʂ��قȂ�܂��B
 * 
 * <pre>
 *  http://host/context/services
 * </pre>
 * 
 * �����[�g�I�u�W�F�N�g�����̖��O�ł���Ƃ��܂��B
 * 
 * <pre>
 *  Foo
 * </pre>
 * 
 * �����[�g�I�u�W�F�N�g��URL�͎��̂悤�ɂȂ�܂��B
 * 
 * <pre>
 *  http://host/context/Foo
 * </pre>
 * 
 * @author koichik
 */
public abstract class URLBasedConnector implements Connector {
    /**
     * �����[�g�I�u�W�F�N�g��URL���L���b�V���������̃f�t�H���g�l
     */
    protected static final int DEFAULT_MAX_CACHED_URLS = 10;

    protected URL baseURL;
    protected LRUMap cachedURLs = new LRUMap(DEFAULT_MAX_CACHED_URLS);

    /**
     * �x�[�XURL��ݒ肵�܂��B
     * 
     * @param baseURL
     *            �x�[�XURL
     */
    public void setBaseURL(final String baseURL) throws MalformedURLException {
        this.baseURL = new URL(baseURL);
    }

    /**
     * �����[�g�I�u�W�F�N�g��URL���L���b�V����������ݒ肵�܂��B
     * 
     * @param maxCachedURLs
     *            �����[�g�I�u�W�F�N�g��URL���L���b�V��������
     */
    public synchronized void setMaxCachedURLs(final int maxCachedURLs) {
        cachedURLs.setMaxSize(maxCachedURLs);
    }

    /**
     * �����[�g�I�u�W�F�N�g��URL���������A�T�u�N���X�ŗL�̕��@�Ń��\�b�h�Ăяo�������s���܂��B
     * 
     * @param remoteName
     *            �����[�g�I�u�W�F�N�g�̖��O
     * @param method
     *            �Ăяo�����\�b�h
     * @param args
     *            �����[�g�I�u�W�F�N�g�̃��\�b�h�Ăяo���ɓn���������l���i�[����I�u�W�F�N�g�z��
     * @return �����[�g�I�u�W�F�N�g�ɑ΂��郁�\�b�h�Ăяo������̖߂�l
     * @throws Throwable
     *             �����[�g�I�u�W�F�N�g�ɑ΂��郁�\�b�h�Ăяo������X���[������O
     */
    public Object invoke(final String remoteName, final Method method, final Object[] args)
            throws Throwable {
        return invoke(getTargetURL(remoteName), method, args);
    }

    /**
     * �����[�g�I�u�W�F�N�g��URL��Ԃ��܂��B
     * �����[�g�I�u�W�F�N�g��URL�́A�����[�g�I�u�W�F�N�g�̖��O���x�[�XURL����̑���URL�Ƃ��ĉ������܂��B
     * 
     * @param remoteName
     *            �����[�g�I�u�W�F�N�g�̖��O
     * @return �����[�g�I�u�W�F�N�g��URL
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
     * �T�u�N���X�ŗL�̕��@�Ń����[�g���\�b�h�̌Ăяo�������s���A���̌��ʂ�Ԃ��܂��B
     * 
     * @param targetURL
     *            �����[�g�I�u�W�F�N�g��URL
     * @param method
     *            �Ăяo�����\�b�h
     * @param args
     *            �����[�g�I�u�W�F�N�g�̃��\�b�h�Ăяo���ɓn���������l���i�[����I�u�W�F�N�g�z��
     * @return �����[�g�I�u�W�F�N�g�ɑ΂��郁�\�b�h�Ăяo������̖߂�l
     * @throws Throwable
     *             �����[�g�I�u�W�F�N�g�ɑ΂��郁�\�b�h�Ăяo������X���[������O
     */
    protected abstract Object invoke(URL targetURL, Method method, Object[] args) throws Throwable;

    /**
     * LRU�}�b�v <br>
     * �G���g�����ɏ��������A����𒴂��ăG���g�����ǉ����ꂽ�ꍇ�ɂ͂����Ƃ��g�p����Ă��Ȃ��G���g������菜�����}�b�v�̎����ł��B
     * �G���g�����̏���͐������₷���Ƃ��o���܂����A���炵�Ă����̐��܂ŃG���g������菜����邱�Ƃ͂���܂���B ���̃}�b�v�͓�������܂���B
     * 
     * @author koichik
     */
    protected static class LRUMap extends LinkedHashMap {
        /** �f�t�H���g�����e�� */
        protected static final int DEFAULT_INITIAL_CAPACITY = 16;
        /** �f�t�H���g���׌W�� */
        protected static final float DEFAULT_LOAD_FACTOR = 0.75f;

        protected int maxSize;

        /**
         * �f�t�H���g�̏����e�ʂƕ��׌W���Ŏw�肳�ꂽ�G���g����������Ƃ���C���X�^���X���\�z���܂��B
         * 
         * @param maxSize
         *            �G���g�����̍ő吔
         */
        public LRUMap(final int maxSize) {
            this(maxSize, DEFAULT_INITIAL_CAPACITY, DEFAULT_LOAD_FACTOR);
        }

        /**
         * �w�肳�ꂽ�����e�ʂƕ��׌W���A�G���g�����̏�������C���X�^���X���\�z���܂��B
         * 
         * @param maxSize
         *            �G���g�����̍ő吔
         * @param initialCapacity
         *            �����e��
         * @param loadFactor
         *            ���׌W��
         */
        public LRUMap(final int maxSize, final int initialCapacity, final float loadFactor) {
            super(initialCapacity, loadFactor, true);
            this.maxSize = maxSize;
        }

        /**
         * �G���g�����̍ő�l��ݒ肵�܂��B
         * 
         * @param maxSize
         *            �G���g�����̍ő吔
         */
        public void setMaxSize(final int maxSize) {
            this.maxSize = maxSize;
        }

        /**
         * �}�b�v�̃G���g�������ő吔�𒴂��Ă���ꍇ <code>true</code> ��Ԃ��܂��B
         * ���̌��ʁA�ł��O�Ƀ}�b�v�ɑ}�����ꂽ�G���g�����}�b�v����폜����܂��B
         * 
         * @param eldest
         *            �����Ƃ��O�Ƀ}�b�v�ɑ}�����ꂽ�G���g��
         * @return �}�b�v�̃G���g�������ő吔�𒴂��Ă���ꍇ <code>true</code>
         */
        protected boolean removeEldestEntry(final Map.Entry eldest) {
            return maxSize > 0 && size() > maxSize;
        }
    }
}