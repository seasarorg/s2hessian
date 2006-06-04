using System;
using System.Collections.Generic;
using System.Text;

namespace org.seasar.s2hessian.client
{
    public class SqlDate
    {
        private DateTime value;
        public DateTime getValue()
        {
            return value;
        }

        public void setValue(DateTime value)
        {
            this.value = value;
        }
    }
}
