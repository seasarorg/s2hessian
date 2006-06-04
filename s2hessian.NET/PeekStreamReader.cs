#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

#endregion

namespace org.seasar.s2hessian.client
{
    public class PeekStreamReader
    {
        public PeekStreamReader(Stream sr)
        {
            _stream = sr;
            peeked = false;
        }
        private Stream _stream;
        private bool peeked;
        private int peekValue;
        public int Peek()
        {
            if (peeked)
            {
                return peekValue;
            }
            else
            {
                peekValue = _stream.ReadByte();
                peeked = true;
                return peekValue;
            }
        }
        public int Read()
        {
            int read;
            if (peeked)
            {
                peeked = false;
                read = peekValue;
            }
            else
            {
                read =  _stream.ReadByte();
            }

            return read;
        }
    }
}
