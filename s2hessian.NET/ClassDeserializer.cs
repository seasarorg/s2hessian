using System;

namespace org.seasar.s2hessian.client
{
	public class ClassDeserializer:Deserializer
	{
		override public System.Type Type
		{
			get
			{
				return typeof(System.Type);
			}
			
		}
		public ClassDeserializer()
		{
		}

        public override System.Object readMap(ObjectRead _in)
        {
			System.String name = null;
			
//			while (!_in.isEnd())
//			{
//				System.String key = _in.readString();
//				System.String value_Renamed = _in.readString();
//				
//				if (key.Equals("name"))
//					name = value_Renamed;
//			}
//			
//			_in.readMapEnd();
//			
//			if (name == null)
//				throw new System.IO.IOException("Serialized Class expects name.");
//			
//			
//			try
//			{
//
//				return null;
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
