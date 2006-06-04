#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

namespace org.seasar.s2hessian.client
{
    public static class ParseUtil
    {
        public static int ParseShort(PeekStreamReader _streamReader)
        {
            return (_streamReader.Read() << 8) + _streamReader.Read();
        }
    }
}
