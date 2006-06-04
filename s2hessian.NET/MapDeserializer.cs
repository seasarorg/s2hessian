using System;

namespace org.seasar.s2hessian.client
{
	public class MapDeserializer:AbstractMapDeserializer
	{	private System.Type type;
		override public System.Type Type
		{
			get
			{ return type;}
		}
		
		public MapDeserializer(System.Type type)
		{
			this.type = type;
		}

        public override System.Object readMap(ObjectRead _in)
        {
			System.Collections.IDictionary map;

            if (type == null)
            {
                map = new System.Collections.Hashtable();
            }
            else if (type.Equals(typeof(System.Collections.IDictionary)))
            {
                map = new System.Collections.Hashtable();
            }
            else
            {
                if (type.Equals(typeof(System.Collections.SortedList)))
                {
                    map = new System.Collections.SortedList();
                }
                else
                {
                    try
                    {
                        map = (System.Collections.IDictionary)System.Activator.CreateInstance(type);
                    }
                    catch (System.Exception e)
                    {
                        throw new System.IO.IOException(System.Convert.ToString(e));
                    }
                }
            }

            _in.AddRef(map);

            while (!_in.GetReplyProcess().IsEnd())
            {
                map[_in.ReadObject()] = _in.ReadObject();
            }

            _in.GetReplyProcess().ReadEnd();

            return map;

        }
	}
}
