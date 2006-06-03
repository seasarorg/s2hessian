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
package org.seasar.s2hessian;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.ServletConfig;
import javax.servlet.ServletException;

import org.seasar.framework.container.factory.SingletonS2ContainerFactory;
import org.seasar.framework.container.servlet.S2ContainerServlet;
import org.seasar.framework.container.S2Container;
import org.seasar.framework.container.ComponentDef;
import org.seasar.framework.container.ComponentNotFoundRuntimeException;
import org.seasar.framework.container.MetaDef;

import com.caucho.hessian.io.HessianInput;
import com.caucho.hessian.io.HessianOutput;
import com.caucho.hessian.server.HessianSkeleton;

public class S2HessianServlet extends HttpServlet {

    private S2Container container;

    public void doPost(HttpServletRequest req, HttpServletResponse res) throws ServletException {
        container = S2ContainerServlet.getContainer();
        String componentName = req.getPathInfo().substring(1);
        ComponentDef def = container.getComponentDef(componentName);
        MetaDef meta = def.getMetaDef(MetaConstant.META);
        if (meta == null) {
            throw new ServletException("Component Name[" + componentName
                    + "] is not public.");
        }
        Class componentApi = def.getComponentClass();
        Object component = container.getComponent(componentName);
        HessianSkeleton skeleton = new HessianSkeleton(component, componentApi);

        try {
            HessianInput in = new HessianInput(req.getInputStream());
            HessianOutput out = new HessianOutput(res.getOutputStream());
            skeleton.invoke(in, out);
        } catch (Throwable e) {
            throw new ServletException(e);
        }
    }

}
