using System;

namespace org.seasar.s2hessian.client
{
	public class InputStreamSerializer:Serializer
	{
		public InputStreamSerializer()
		{
		}
		
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			System.IO.Stream _is = (System.IO.Stream) obj;
			
			if (_is == null)
				_out.writeNull();
			else
			{
				byte[] buf = new byte[1024];
				int len;
				
				while ((len = _is.Read(buf, 0, buf.Length)) > 0)
				{
					_out.writeByteBufferPart(buf, 0, len);
				}
				
				_out.writeByteBufferEnd(buf, 0, 0);
			}
		}
	}
}
