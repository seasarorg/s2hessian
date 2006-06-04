using System;
using System.Collections;
namespace org.seasar.s2hessian.client
{

	public class IdentityComparer:IComparer
	{
		public IdentityComparer()
		{
		}
		public int Compare(Object a, Object b)
		{
			if(a==b)
			{
				return 0;
			}
			if (a.GetHashCode()>b.GetHashCode())
			{
				return 1;
			}
			else
			{
				return -1;
			}

		}


	}
}
