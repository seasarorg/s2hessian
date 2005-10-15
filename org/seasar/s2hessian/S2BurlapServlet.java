package org.seasar.s2hessian;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.ServletConfig;
import javax.servlet.ServletException;

import org.seasar.framework.container.factory.SingletonS2ContainerFactory;
import org.seasar.framework.container.servlet.S2ContainerServlet;
import org.seasar.framework.container.S2Container;

import com.caucho.burlap.io.BurlapInput;
import com.caucho.burlap.io.BurlapOutput;
import com.caucho.burlap.server.BurlapSkeleton;

public class S2BurlapServlet extends HttpServlet {

    private S2Container container;

    public void doPost(HttpServletRequest req, HttpServletResponse res) throws ServletException {
        container = S2ContainerServlet.getContainer();
        String componentName = req.getPathInfo().substring(1);
        Class componentApi = container.getComponentDef(componentName).getComponentClass();
        Object component = container.getComponent(componentName);
        BurlapSkeleton skeleton = new BurlapSkeleton(component, componentApi);

        try {
            BurlapInput in = new BurlapInput(req.getInputStream());
            BurlapOutput out = new BurlapOutput(res.getOutputStream());
            skeleton.invoke(in, out);
        } catch (Throwable e) {
            throw new ServletException(e);
        }
    }
}
