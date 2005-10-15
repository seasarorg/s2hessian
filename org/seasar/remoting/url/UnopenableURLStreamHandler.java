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
package org.seasar.remoting.url;

import java.io.IOException;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLStreamHandler;

/**
 * �f�t�H���g�̃|�[�g�ԍ��������A�I�[�v�����邱�Ƃ̏o���Ȃ� <code>URL</code> �̂��߂� <code>URLStreamHandler</code> �ł��B
 * 
 * @author koichik
 */
public class UnopenableURLStreamHandler extends URLStreamHandler {
    protected final int defaultPort;

    /**
     * �w�肳�ꂽ�|�[�g�ԍ����f�t�H���g�Ƃ��Ď��V�����C���X�^���X���\�z���܂��B
     * 
     * @param defaultPort ���̃v���g�R���̃f�t�H���g�̃|�[�g�ԍ�
     */
    public UnopenableURLStreamHandler(final int defaultPort) {
        this.defaultPort = defaultPort;
    }

    /**
     * ���̑���̓T�|�[�g����܂���B
     * 
     * @throws UnsupportedOperationException ��ɃX���[����܂�
     */
    protected URLConnection openConnection(final URL url) throws IOException {
        throw new UnsupportedOperationException();
    }

    /**
     * <code>URL</code> �����t�B�[���h�l���A�w�肳�ꂽ�l�ɐݒ肵�܂��B<br>
     * �|�[�g�ԍ����w�肳��Ă��Ȃ��ꍇ�́A�R���X�g���N�^�Ŏw�肳�ꂽ�f�t�H���g�̃|�[�g�ԍ���ݒ肵�܂��B
     * 
     * @param url �C������ <code>URL</code>
     * @param protocol �v���g�R����
     * @param host <code>URL</code> �̃����[�g�z�X�g�l
     * @param port �����[�g�}�V����̃|�[�g
     * @param authority  <code>URL</code> �̌�������
     * @param userInfo <code>URL</code> �̃��[�U��񕔕�
     * @param path <code>URL</code> �̃p�X�R���|�[�l���g
     * @param query <code>URL</code> �̃N�G���[����
     * @param ref �Q��
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