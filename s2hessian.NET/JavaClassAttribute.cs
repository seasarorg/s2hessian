using System;

namespace  org.seasar.s2hessian.client
{
	[AttributeUsage(
		 AttributeTargets.Class,
		 AllowMultiple = false,
		 Inherited = false)]
	public class JavaClassAttribute :Attribute
	{
		private string javaClass;
		public JavaClassAttribute(string jc)
		{
			javaClass=jc;
		}
		public string getJavaClass()
		{
			return javaClass;
		}
	}
}
