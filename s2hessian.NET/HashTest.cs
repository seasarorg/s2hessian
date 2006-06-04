using System;

namespace org.seasar.s2hessian.client
{
	/// <summary>
	/// HashTest ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class HashTest
	{
		private string aa;
		public HashTest(string a)
		{
			aa=a;

		}
		public override int GetHashCode()
		{
			return 100;
		}
		public override System.Boolean Equals(object a)
		{
			return true;
		}
		
	}
}
