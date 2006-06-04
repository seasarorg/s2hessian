using System;

namespace org.seasar.s2hessian.client
{
	abstract public class Serializer
	{
		abstract public void  writeObject(System.Object obj, OutputStream out_Renamed);
	}
}
