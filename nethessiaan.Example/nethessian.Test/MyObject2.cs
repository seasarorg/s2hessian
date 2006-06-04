using System;
using System.Collections.Generic;
using System.Text;

namespace nethessian.Test
{
    //[JavaClass("org.seasar.s2hessian.example.MyObject2")]
    public class MyObject2
    {
        private MyObject1 mo1;
        public MyObject2()
        {

        }
        public MyObject1 getMo1()
        {
            return mo1;
        }
        public void setMo1(MyObject1 mo)
        {
            mo1 = mo;
        }
    }
}
