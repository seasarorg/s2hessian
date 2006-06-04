using System;

namespace org.seasar.s2hessian.client
{
	public class AbstractListDeserializer:Deserializer
	{
        public override System.Object readObject(ObjectRead _in)
        {
//			int code = _in.readListStart();
//			
//			if (code == 'N')
//				return null;
//			else if (code == 'R')
//				return _in.readRef();
//			
//			System.String type = _in.readType();
//			int length = _in.readLength();
//			
//			return readList(_in, length);
            // must delete
            return null;
        }
	}
}
