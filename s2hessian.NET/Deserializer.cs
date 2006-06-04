using System;

namespace org.seasar.s2hessian.client
{
	abstract public class Deserializer
	{
		virtual public System.Type Type
		{
			get
			{
				return typeof(System.Object);
			}

			
		}

        public virtual System.Object readObject(ObjectRead _in)
        {
			throw new System.NotSupportedException(System.Convert.ToString(this));
		}

        public virtual System.Object readList(ObjectRead _in, int length)
        {
			throw new System.NotSupportedException(System.Convert.ToString(this));
		}

        public virtual System.Object readMap(ObjectRead _in)
        {
			throw new System.NotSupportedException(System.Convert.ToString(this)); 
		}
	}
}
