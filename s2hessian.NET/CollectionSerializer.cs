using System;

namespace org.seasar.s2hessian.client
{
	public class CollectionSerializer:Serializer
	{
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			if (_out.addRef(obj))
				return ;
			
			System.Collections.ICollection list = (System.Collections.ICollection) obj;
			
			System.Type cl = obj.GetType();
			if (cl.Equals(typeof(System.Collections.ArrayList)))
				_out.writeListBegin(list.Count, null);
			else
			{
                string className = obj.GetType().FullName;
                if (className.IndexOf("[") > 0)
                {
                    _out.writeListBegin(list.Count, null);
                }
                else
                {
                    _out.writeListBegin(list.Count, obj.GetType().FullName);
                }
			}
			
			System.Collections.IEnumerator iter = list.GetEnumerator();
			while (iter.MoveNext())
			{
				System.Object value_Renamed = iter.Current;
				
				_out.writeObject(value_Renamed);
			}
			_out.writeListEnd();
		}
	}
}
