using System;

namespace org.seasar.s2hessian.client
{
	public class StackTraceElementDeserializer:AbstractMapDeserializer
	{
		override public System.Type Type
		{
			get
			{
				return _stackTraceClass;
			}
			
		}
		private static System.Type _stackTraceClass;
		
		public StackTraceElementDeserializer()
		{
		}

        public override System.Object readMap(ObjectRead _in)
        {
			System.Collections.Hashtable map = new System.Collections.Hashtable();
			
//			_in.addRef(map);
//			
//			System.String declaringClass = null;
//			System.String methodName = null;
//			System.String fileName = null;
//			int lineNumber = 0;
//			
//			while (!_in.isEnd())
//			{
//				System.String key = _in.readString();
//				
//				if (key.Equals("declaringClass"))
//					declaringClass = _in.readString();
//				else if (key.Equals("methodName"))
//					methodName = _in.readString();
//				else if (key.Equals("fileName"))
//					fileName = _in.readString();
//				else if (key.Equals("lineNumber"))
//					lineNumber = _in.readInt();
//				else
//				{
//					System.Object value_Renamed = _in.readObject();
//				}
//			}
//			
//			_in.readMapEnd();
//			
//			System.IO.MemoryStream bos = new System.IO.MemoryStream();
//			System.IO.BinaryWriter oos = new System.IO.BinaryWriter(bos);
//			System.Exception e1 = new System.IO.IOException();
//			try
//			{
//				throw e1;
//			}
//			catch (System.Exception e2)
//			{
//				e1 = e2;
//			}
//			
//			
//			oos.Write(lineNumber);
//			
//			if (declaringClass != null)
//			{
//			}
//			else
//			{
//			}
//			
//			if (fileName != null)
//			{
//
//			}
//			else
//			{
//
//			}
//			
//			if (methodName != null)
//			{
//
//			}
//			else
//			{
//
//			}
//			
//			oos.Close();
//			bos.Close();
//			
//			byte[] data = bos.ToArray();
//			
//			try
//			{
//
//				return null;
//			}
//			catch (System.Exception e)
//			{
//				return null;
//			}
            //must delete
            return null;
        }
		static StackTraceElementDeserializer()
		{
		{
			try
			{
				_stackTraceClass = System.Type.GetType("java.lang.StackTraceElement");
			}
			catch (System.Exception e)
			{
                throw new HessianServiceException();
            }
		}
		}
	}
}
