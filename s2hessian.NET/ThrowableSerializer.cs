using System;

namespace org.seasar.s2hessian.client
{
	public class ThrowableSerializer:JavaSerializer
	{
		public ThrowableSerializer():base(typeof(System.Exception))
		{
		}
		
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			System.Exception e = (System.Exception) obj;

			base.writeObject(obj, _out);
		}
	}
}
