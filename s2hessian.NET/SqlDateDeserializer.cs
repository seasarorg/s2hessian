using System;

namespace org.seasar.s2hessian.client
{
	public class SqlDateDeserializer:Deserializer
	{
		override public System.Type Type
		{
			get
			{
				return _cl;
			}
			
		}
		private System.Type _cl;
		private System.Reflection.ConstructorInfo _constructor;
		
		public SqlDateDeserializer(System.Type cl)
		{
			_cl = cl;
			_constructor = cl.GetConstructor(new System.Type[]{typeof(long)});
		}

        public override System.Object readMap(ObjectRead _in)
        {
//			long initValue = System.Int64.MinValue;
//			
//			while (!_in.isEnd())
//			{
//				System.String key = _in.readString();
//				
//				if (key.Equals("value"))
//					initValue = _in.readUTCDate();
//				else
//					_in.readString();
//			}
//			
//			_in.readMapEnd();
//			
//			if (initValue == System.Int64.MinValue)
//			{
//				throw new System.IO.IOException(_cl.FullName + " expects name.");
//			}
//			
//			try
//			{
//				return _constructor.Invoke(new System.Object[]{(long) initValue});
//			}
//			catch (System.Exception e)
//			{
//				throw new System.IO.IOException(System.Convert.ToString(e));
//			}

            //must delete
            return null;
        }
	}
}
