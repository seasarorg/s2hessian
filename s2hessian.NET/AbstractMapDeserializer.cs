using System;

namespace org.seasar.s2hessian.client
{
	public class AbstractMapDeserializer:Deserializer
	{
        public override System.Object readObject(ObjectRead _in)
        {
//			int code = _in.readMapStart();
//			
//			switch (code)
//			{
//				
//				case 'N': 
//					return null;
//				
//				case 'R': 
//					return _in.readRef();
//			}
//			
//			System.String type = _in.readType();
//			
//			return readMap(_in);

            //must delete
            return null;
        }
	}
}
