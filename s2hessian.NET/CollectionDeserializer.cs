using System;
using System.Collections;

namespace org.seasar.s2hessian.client
{
	public class CollectionDeserializer:AbstractListDeserializer
	{
		override public System.Type Type
		{
			get
			{
				return _type;
			}
			
		}
		private System.Type _type;
		
		public CollectionDeserializer(System.Type type)
		{
			_type = type;
		}

        public override System.Object readList(ObjectRead _in, int length)
        {
			System.Collections.IList list = null;

            if (_type == null)
                list = new System.Collections.ArrayList();
            else if (!_type.IsInterface)
            {
                try
                {
                    list = (System.Collections.IList)System.Activator.CreateInstance(_type);
                }
                catch (System.Exception e)
                {
                }
            }

            if (list != null)
            {
            }

            else if (typeof(System.Collections.IList).IsAssignableFrom(_type))
                list = new System.Collections.ArrayList();
            else if (typeof(System.Collections.ICollection).IsAssignableFrom(_type))
                list = new System.Collections.ArrayList();
            else
            {
                try
                {
                    list = (System.Collections.IList)System.Activator.CreateInstance(_type);
                }
                catch (System.Exception e)
                {
                    throw new System.IO.IOException(System.Convert.ToString(e));
                }
            }

            _in.AddRef(list);

            while (!_in.GetReplyProcess().IsEnd())
            {
                list.Add(_in.ReadObject());
            }

            _in.GetReplyProcess().ReadEnd();

            return list;
        }
	}
}
