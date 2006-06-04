using System;

namespace org.seasar.s2hessian.client
{
	public class ArraySerializer:Serializer
	{
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			if (_out.addRef(obj))
				return ;
			
			System.Object[] array = (System.Object[]) obj;
			
			_out.writeListBegin(array.Length, getArrayType(obj.GetType()));
			
			for (int i = 0; i < array.Length; i++)
				_out.writeObject(array[i]);
			
			_out.writeListEnd();
		}
		

		private System.String getArrayType(System.Type cl)
		{
			if (cl.IsArray)
				return '[' + getArrayType(cl.GetElementType());
			
			System.String name = cl.FullName;
			
			if (name.Equals("java.lang.String"))
				return "string";
			else if (name.Equals("java.lang.Object"))
				return "object";
			else if (name.Equals("java.util.Date"))
				return "date";
			else
				return name;
		}
	}
}
