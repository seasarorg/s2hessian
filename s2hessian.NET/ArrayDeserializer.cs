using System;

namespace org.seasar.s2hessian.client
{
	public class ArrayDeserializer:AbstractListDeserializer
	{
		override public System.Type Type
		{
			get
			{
				return typeof(System.Object[]);
			}
			
		}
		private System.Type _componentType;
		
		public ArrayDeserializer(Deserializer componentDeserializer)
		{
			if (componentDeserializer != null)
				_componentType = componentDeserializer.Type;
		}

        public override System.Object readList(ObjectRead _in, int length)
        {
//			if (length >= 0)
//			{
//				System.Object[] data = createArray(length);
//				
//				_in.addRef(data);
//				
//				if (_componentType != null)
//				{
//					for (int i = 0; i < data.Length; i++)
//						data[i] = _in.readObject(_componentType);
//				}
//				else
//				{
//					for (int i = 0; i < data.Length; i++)
//						data[i] = _in.readObject();
//				}
//				
//				_in.readListEnd();
//				
//				return data;
//			}
//			else
//			{
//				System.Collections.ArrayList list = new System.Collections.ArrayList();
//				
//				_in.addRef(list);
//				
//				if (_componentType != null)
//				{
//					while (!_in.isEnd())
//						list.Add(_in.readObject(_componentType));
//				}
//				else
//				{
//					while (!_in.isEnd())
//						list.Add(_in.readObject());
//				}
//				
//				_in.readListEnd();
//				
//				System.Object[] data = createArray(list.Count);
//				for (int i = 0; i < data.Length; i++)
//					data[i] = list[i];
//				
//				return data;
//			}
            //must delete
            return null;
        }
		
		protected internal virtual System.Object[] createArray(int length)
		{
			if (_componentType != null)
				return (System.Object[]) System.Array.CreateInstance(_componentType, length);
			else
				return new System.Object[length];
		}
		
		public override System.String ToString()
		{
			return "ArrayDeserializer[" + _componentType + "]";
		}
	}
}
