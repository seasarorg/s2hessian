using System;

namespace org.seasar.s2hessian.client
{
	/// <summary>
	/// HashTest �̊T�v�̐����ł��B
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
