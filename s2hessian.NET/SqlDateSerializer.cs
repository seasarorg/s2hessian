using System;

namespace  org.seasar.s2hessian.client
{
	public class SqlDateSerializer:Serializer
	{
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			if (obj == null)
				_out.writeNull();
			else
			{
				System.Type cl = obj.GetType();
				_out.writeMapBegin(cl.FullName);
				_out.writeString("value");
				_out.writeUTCDate(((System.DateTime) obj).Ticks);
				_out.writeMapEnd();
			}
		}
	}
}
