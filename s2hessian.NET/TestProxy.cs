using System;
using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nethessian;



namespace org.seasar.s2hessian.client
{

	public class TestProxy
	{

		public TestProxy()
		{

		}

		static void Main()
		{

			DotNetProxy dnp= new DotNetProxy("http://localhost:8080/hessiantest/s2h/");
			dnp.setClassConv(typeof(nethessian.ErrorItem),"org.seasar.axis.examples.shimura.ErrorItem");
			ErrorItem ht=new ErrorItem();
			ht.cat="W";
			ht.errData="Edata";
			ht.errElement="Ele";
			ht.errMsg="Emsg";
			ErrorInfo ei = new ErrorInfo();
			ErrorItem [] eii={ht};
			ei.eitem=eii;
			ei.errorNo=10;
			ei.warningNo=5;
			Double d=3.25;
			Double d2=45.01;
				object[] arg={ d,d2};
			Double dx=(Double)dnp.invoke("seasartest","doublePlus",typeof(Double),arg);
			Double dd=dx;
		
		}
	}
}
