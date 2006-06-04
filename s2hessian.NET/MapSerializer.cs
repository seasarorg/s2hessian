using System;

namespace org.seasar.s2hessian.client
{
	public class MapSerializer:Serializer
	{
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			if (_out.addRef(obj))
				return ;
			
			System.Collections.IDictionary map = (System.Collections.IDictionary) obj;
			
			System.Type cl = obj.GetType();
			if (cl.Equals(typeof(System.Collections.Hashtable)))
				_out.writeMapBegin(null);
			else
			{
				string className = obj.GetType().FullName;
                if (className.IndexOf("[") > 0)
                {
                    _out.writeMapBegin(null);
                }
                else
                {
                    _out.writeMapBegin(obj.GetType().FullName);
                }
			}
			System.Collections.IEnumerator iter = map.GetEnumerator();
			while (iter.MoveNext())
			{
				System.Object key = iter.Current;
				System.Type st= key.GetType();
				
				if (st==typeof(System.Collections.DictionaryEntry))
				{
					_out.writeObject(((System.Collections.DictionaryEntry)key).Key);
					_out.writeObject(((System.Collections.DictionaryEntry)key).Value);
				}
				else
				{
					_out.writeObject(key);
					System.Object _value = map[key];
					_out.writeObject(_value);
				}
				

				

			}
			_out.writeMapEnd();
		}
	}
}
