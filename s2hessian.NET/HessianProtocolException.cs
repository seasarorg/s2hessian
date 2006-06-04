using System;
namespace org.seasar.s2hessian.client
{
	

	[Serializable]
	public class HessianProtocolException:System.IO.IOException
	{
		virtual public System.Exception RootCause
		{
			get
			{
				return rootCause;
			}
			
		}
		private System.Exception rootCause;
		

		public HessianProtocolException()
		{
		}
		

		public HessianProtocolException(System.String message):base(message)
		{
		}
		
		public HessianProtocolException(System.String message, System.Exception rootCause):base(message)
		{
			
			this.rootCause = rootCause;
		}
		
		public HessianProtocolException(System.Exception rootCause):base(System.Convert.ToString(rootCause))
		{
			
			this.rootCause = rootCause;
		}
	}
}