using System;

namespace  org.seasar.s2hessian.client
{
	public class StringValueDeserializer:Deserializer
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
		
		public StringValueDeserializer(System.Type cl)
		{
			_cl = cl;
			_constructor = cl.GetConstructor(new System.Type[]{typeof(System.String)});
		}

        public override System.Object readMap(ObjectRead _in)
        {
			System.String initValue = null;
			
//			while (!_in.isEnd())
//			{
//				System.String key = _in.readString();
//				System.String _value = _in.readString();
//				
//				if (key.Equals("value"))
//					initValue = _value;
//			}
//			
//			_in.readMapEnd();
//			
//			if (initValue == null)
//			{
//				throw new System.IO.IOException(_cl.FullName + " expects name.");
//			}
//			
//			try
//			{
//				return _constructor.Invoke(new System.Object[]{initValue});
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
