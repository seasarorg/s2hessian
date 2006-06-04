using System;
using System.Collections.Generic;
using System.Text;
using org.seasar.s2hessian.client;

namespace nethessian.Test
{
    [JavaClass("org.seasar.s2hessian.example.MyObject")]
    public class MyObject1
    {
        private int int1;
        private string string1;
        public MyObject1()
        {

        }
        public int getInt1()
        {
            return int1;
        }

        public void setInt1(int int1)
        {
            this.int1 = int1;
        }

        public String getString1()
        {
            return string1;
        }
        public void setString1(String string1)
        {
            this.string1 = string1;
        }
    }
}
