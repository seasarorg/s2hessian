using System;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Reflection;

namespace org.seasar.s2hessian.client
{

	public class DotNetProxy
	{
		private string baseURI; 
		private OutputStream _os ;
		private Hashtable classConv = new Hashtable();
		private Hashtable classConv2 = new Hashtable();
		private void classConvInit()
		{
            setClassConv(typeof(System.Collections.Hashtable), "java.util.Hashtable");

        }
		public void setClassConv(System.Type type,string s2)
		{
			string s1=type.FullName;
			classConv.Add(s1,s2);
			classConv.Add(s2,s1);
			classConv2.Add(s1,type);
		}
		public System.Type getConvertClass2(String cl)
		{
			System.Type st = (System.Type)classConv2[cl];
            if (st == null)
            {
                throw new Exception("Type Convert error " + cl);
            }
            return st;
		}
        public bool setClassConv(System.Type type)
        {
				JavaClassAttribute jc = (JavaClassAttribute)Attribute.GetCustomAttribute(type, 
					typeof(JavaClassAttribute));
                if (jc != null)
                {
                    string javaclass = jc.getJavaClass();
                    string cl = type.FullName;
                    classConv.Add(cl, javaclass);
                    classConv.Add(javaclass, cl);
                    classConv2.Add(cl, type);
                    return true;
                }
                return false;
        }
		public string getConvertClass(System.Type type)
		{	
			string cl=type.FullName;
            int pos = cl.IndexOf("`");
            if (pos > 0)
            {
                cl = cl.Substring(0, pos+2) ;
            }
			if (classConv[cl]!=null)
			{
				return (string)classConv[cl];
			}
			else
			{
                if (setClassConv(type))
                {
                    return (string)classConv[cl];
                }
                //JavaClassAttribute jc = (JavaClassAttribute)Attribute.GetCustomAttribute(type, 
                //    typeof(JavaClassAttribute));
                //if (jc!=null)
                //{
                //    string javaclass=jc.getJavaClass();
                //    classConv.Add(cl,javaclass);
                //    classConv.Add(javaclass,cl);
                //    classConv2.Add(cl,type);
                //    return javaclass;
                //}
				return cl;
			}

		}

		public string getConvertClass(String cl)
		{
			if (classConv[cl]!=null)
			{
				return (string)classConv[cl];
			}
			else
			{
				return cl;
			}
		}

		public DotNetProxy(string uri)
		{
			baseURI=uri;
			classConvInit();
			_os= new OutputStream(this); 
		}
		public object invoke(string component,string method,System.Type returnType,params object[] arg)
		{

			string url=baseURI+component;
			Stream _is=null;
			try
			{
				if (!returnType.IsSubclassOf(typeof(System.ValueType))&&returnType!=typeof(System.String))
				{
                    string returnCl=returnType.FullName;
                    int pos = returnCl.IndexOf("`");
                    if (pos > 0)
                    {
                        returnCl = returnCl.Substring(0, pos+2) ;
                    }
                    if (classConv[returnCl] == null)
                    {

                        String rs = getConvertClass(returnType);
                        Serializer ser = _os.SerializerFactory.getSerializer(returnType);
                        if (ser.GetType() == typeof(JavaSerializer))
                        {
                            System.Reflection.FieldInfo[] ff = ((JavaSerializer)ser).Fields;
                            foreach (System.Reflection.FieldInfo fi in ff)
                            {
                                getConvertClass(fi.FieldType);
                            }
                        }
                    }
                    //////////test

				}
				System.Net.HttpWebRequest conn = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(url);
				conn.Method="POST";
				conn.ContentType = "text/xml";
				HttpStatusCode code;
				System.Net.HttpWebResponse res;
				byte[] buf=_os.call(method,arg);
				try
				{
					conn.ContentLength=buf.Length;
					Stream requestStream=conn.GetRequestStream();
					requestStream.Write(buf,0,buf.Length);
					requestStream.Close();

					res = (HttpWebResponse)conn.GetResponse();
					code = HttpStatusCode.InternalServerError;
						

						code = res.StatusCode;
										}
					catch (System.Exception e)
					{
						throw new Exception(System.Convert.ToString(e));
					}
						
					if (code != HttpStatusCode.OK)
					{
						System.Text.StringBuilder sb = new System.Text.StringBuilder();
						int ch;
							
						try
						{
							_is = res.GetResponseStream();
								
							if (_is != null)
							{
								while ((ch = _is.ReadByte()) >= 0)
								{
									sb.Append((char) ch);
								}
									
								_is.Close();
							}
								
						}
						catch (System.IO.FileNotFoundException e)
						{
							throw new Exception(System.Convert.ToString(e));
						}
						catch (System.IO.IOException e)
						{
							throw new Exception(System.Convert.ToString(e));
						}
						catch (Exception e)
						{

							throw new Exception(System.Convert.ToString(e));
						}
							
						if (_is != null)
							_is.Close();
							
						throw new Exception("HttpStatusCode ERROR "+sb.ToString());
					}
				
					
				_is = res.GetResponseStream();

                ReplyProcess _rp = new ReplyProcess(_is, this);

                return _rp.Execute(returnType);
            }
			catch (Exception e)
			{

				throw new Exception(System.Convert.ToString(e));
			}
			finally
			{
				if (_is != null)
				_is.Close();
			}

		}
	

	}
}
