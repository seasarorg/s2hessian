using System;

namespace org.seasar.s2hessian.client
{
	/// <summary>
	/// HashTest の概要の説明です。
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
