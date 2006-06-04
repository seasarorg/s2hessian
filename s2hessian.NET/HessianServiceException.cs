using System;
namespace org.seasar.s2hessian.client
{

	[Serializable]
	public class HessianServiceException:System.Exception
	{

		virtual public System.String Code
		{
			get
			{
				return code;
			}
			
		}

		virtual public System.Object Detail
		{
			get
			{
				return detail;
			}
			
		}
		private System.String code;
		private System.Object detail;
		
		public HessianServiceException()
		{
		}
        public HessianServiceException(System.Exception e):base(e.Message)
        {

        }
		
		
		public HessianServiceException(System.String message, System.String code, System.Object detail):base(message)
		{
			this.code = code;
			this.detail = detail;
		}
	}
}