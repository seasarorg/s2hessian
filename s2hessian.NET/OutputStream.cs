using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace org.seasar.s2hessian.client
{

	public class OutputStream
	{
		IFormatter bformatter = new BinaryFormatter();
//		MemoryStream mdstream = new MemoryStream();
		public OutputStream(DotNetProxy dnp)
		{
			this.dnp=dnp;
			init();

		}
		public DotNetProxy getDnp()
		{
			return dnp;
		}
		private DotNetProxy dnp;
		private MemoryStream os;
		private static IdentityComparer myComparer = new IdentityComparer();
		private Hashtable _refs = new Hashtable(null,myComparer);
        private SerializerFactory _serializerFactory;

        public SerializerFactory SerializerFactory
        {
            get { return _serializerFactory; }
        }

		public  MemoryStream getBuffer()
		{
			return os;
		}
		public void init()
		{
			if (os==null)
			{
				os= new MemoryStream(1000);
			}
			else
			{
				os.SetLength(0);
			}
			_refs.Clear();
			if (_serializerFactory==null)
			{
				_serializerFactory=new SerializerFactory();
				_serializerFactory.setDotNetProxy(dnp);

			}
		}
		public byte[]  call(string method, System.Object[] args)
		{
			init();
			startCall(method);
			
			if (args != null)
			{
				for (int i = 0; i < args.Length; i++)
				{
					writeObject(args[i]);
				}
			}
			
			completeCall();
			os.Seek(0,SeekOrigin.Begin);
			byte[] buf=new byte[os.Length];
			os.Read(buf,0,(int)os.Length);
			return buf;
		}
		public void  startCall(System.String method)
		{
			os.WriteByte((System.Byte) 'c');
			os.WriteByte((System.Byte) 0);
			os.WriteByte((System.Byte) 1);
				
			os.WriteByte((System.Byte) 'm');
			int len = method.Length;
			os.WriteByte((System.Byte) (len >> 8));
			os.WriteByte((System.Byte) len);
			printString(method, 0, len);
		}
		public void  completeCall()
		{
			os.WriteByte((System.Byte) 'z');
		}
		public void  startReply()
		{
			os.WriteByte((System.Byte) 'r');
			os.WriteByte((System.Byte) 1);
			os.WriteByte((System.Byte) 0);
		}
		public void  completeReply()
		{
			os.WriteByte((System.Byte) 'z');
		}
		public void  writeHeader(System.String name)
		{
			int len = name.Length;
			
			os.WriteByte((System.Byte) 'H');
			os.WriteByte((System.Byte) (len >> 8));
			os.WriteByte((System.Byte) len);
			
			printString(name);
		}
		public virtual void  writeFault(System.String code, System.String message, System.Object detail)
		{
			os.WriteByte((System.Byte) 'f');
			writeString("code");
			writeString(code);
			
			writeString("message");
			writeString(message);
			
			if (detail != null)
			{
				writeString("detail");
				writeObject(detail);
			}
			os.WriteByte((System.Byte) 'z');
		}
		public void  writeObject(System.Object _object)
		{
			if (_object == null)
			{
				writeNull();
				return ;
			}
			
			Serializer serializer;
			
			serializer = _serializerFactory.getSerializer(_object.GetType());
			
			serializer.writeObject(_object, this);
		}
		public void  writeListBegin(int length, System.String type)
		{
			os.WriteByte((System.Byte) 'V');
			os.WriteByte((System.Byte) 't');
			printLenString(type);
			
			os.WriteByte((System.Byte) 'l');
			os.WriteByte((System.Byte) (length >> 24));
			os.WriteByte((System.Byte) (length >> 16));
			os.WriteByte((System.Byte) (length >> 8));
			os.WriteByte((System.Byte) length);
		}
		public void  writeListEnd()
		{
			os.WriteByte((System.Byte) 'z');
		}
		
		public void  writeMapBegin(System.String type)
		{
			os.WriteByte((System.Byte) 'M');
			os.WriteByte((System.Byte) 't');
			printLenString(type);
		}
		public void  writeMapEnd()
		{
			os.WriteByte((System.Byte) 'z');
		}
		public void  writeBoolean(bool _value)
		{
			if (_value)
				os.WriteByte((System.Byte) 'T');
			else
				os.WriteByte((System.Byte) 'F');
		}
		public void  writeInt(int _value)
		{
			os.WriteByte((System.Byte) 'I');
			os.WriteByte((System.Byte) (_value >> 24));
			os.WriteByte((System.Byte) (_value >> 16));
			os.WriteByte((System.Byte) (_value >> 8));
			os.WriteByte((System.Byte) _value);
		}
		public void  writeLong(long _value)
		{
			os.WriteByte((System.Byte) 'L');
			os.WriteByte((byte) (_value >> 56));
			os.WriteByte((byte) (_value >> 48));
			os.WriteByte((byte) (_value >> 40));
			os.WriteByte((byte) (_value >> 32));
			os.WriteByte((byte) (_value >> 24));
			os.WriteByte((byte) (_value >> 16));
			os.WriteByte((byte) (_value >> 8));
			os.WriteByte((byte) (_value));
		}
		public void  writeDouble(double _value)
		{
//			mdstream.Seek(0,System.IO.SeekOrigin.Begin);
//			bformatter.Serialize(mdstream, _value);
//			int sl=(int)mdstream.Length;
//			byte []bb=new byte[sl];
//			mdstream.Seek(0,System.IO.SeekOrigin.Begin);
//			int count=mdstream.Read(bb,0,sl);
			long _long=BitConverter.DoubleToInt64Bits(_value);
//			byte[] xx=new byte[8];
//			xx[0]=((byte) (x>> 56));
//			xx[1]=((byte) (x>> 48));
//			xx[2]=((byte) (x>> 40));
//			xx[3]=((byte) (x>> 32));
//			xx[4]=((byte) (x>> 24));
//			xx[5]=((byte) (x>> 16));
//			xx[6]=((byte) (x>> 8));
//			xx[7]=((byte) (x));
			os.WriteByte((System.Byte) 'D');
//			for(int i=sl-2;i>sl-10;i--)
//			{
//				os.WriteByte(bb[i]);
//			}
//			for(int i=0;i<8;i++)
//			{
//					os.WriteByte(xx[i]);
//			}
			os.WriteByte((byte) (_long >> 56));
			os.WriteByte((byte) (_long >> 48));
			os.WriteByte((byte) (_long >> 40));
			os.WriteByte((byte) (_long >> 32));
			os.WriteByte((byte) (_long >> 24));
			os.WriteByte((byte) (_long >> 16));
			os.WriteByte((byte) (_long >> 8));
			os.WriteByte((byte) (_long));
		}
		public void  writeUTCDate(long time)
		{
			time=time/10000-62135629200000;

			os.WriteByte((System.Byte) 'd');
			byte b1=(byte)(time >> 56);
			byte b2=(byte)(time >> 48);
			byte b3=(byte)(time >> 40);
			byte b4=(byte)(time >> 32);
			byte b5=(byte)(time >> 24);
			byte b6=(byte)(time >> 16);
			byte b7=(byte)(time >> 8);
			byte b8=(byte)(time );
			os.WriteByte((byte) (time >> 56));
			os.WriteByte((byte) (time >> 48));
			os.WriteByte((byte) (time >> 40));
			os.WriteByte((byte) (time >> 32));
			os.WriteByte((byte) (time >> 24));
			os.WriteByte((byte) (time >> 16));
			os.WriteByte((byte) (time >> 8));
			os.WriteByte((byte) (time));
		}
		public void  writeNull()
		{
			os.WriteByte((System.Byte) 'N');
		}
		public void  writeString(System.String _value)
		{
            const int CHUNKLEN=0x8000;
			if (_value == null)
			{
				os.WriteByte((System.Byte) 'N');
			}
			else
			{
				int length = _value.Length;
				int offset = 0;

                while (length > CHUNKLEN)
                {
                    int sublen = CHUNKLEN;

                    os.WriteByte((System.Byte) 's');
					os.WriteByte((System.Byte) (sublen >> 8));
					os.WriteByte((System.Byte) sublen);
					
					printString(_value, offset, sublen);
					
					length -= sublen;
					offset += sublen;
				}
				
				os.WriteByte((System.Byte) 'S');
				os.WriteByte((System.Byte) (length >> 8));
				os.WriteByte((System.Byte) length);
				
				printString(_value, offset, length);
			}
		}
		public void  writeString(char[] buffer, int offset, int length)
		{
			if (buffer == null)
			{
				os.WriteByte((System.Byte) 'N');
			}
			else
			{
				while (length > 0x8000)
				{
					int sublen = 0x8000;
					
					os.WriteByte((System.Byte) 's');
					os.WriteByte((System.Byte) (sublen >> 8));
					os.WriteByte((System.Byte) sublen);
					
					printString(buffer, offset, sublen);
					
					length -= sublen;
					offset += sublen;
				}
				
				os.WriteByte((System.Byte) 'S');
				os.WriteByte((System.Byte) (length >> 8));
				os.WriteByte((System.Byte) length);
				
				printString(buffer, offset, length);
			}
		}
		public void  writeBytes(byte[] buffer)
		{
			if (buffer == null)
				os.WriteByte((System.Byte) 'N');
			else
				writeBytes(buffer, 0, buffer.Length);
		}
		public void  writeBytes(byte[] buffer, int offset, int length)
		{
			if (buffer == null)
			{
				os.WriteByte((System.Byte) 'N');
			}
			else
			{
				while (length > 0x8000)
				{
					int sublen = 0x8000;
					
					os.WriteByte((System.Byte) 'b');
					os.WriteByte((System.Byte) (sublen >> 8));
					os.WriteByte((System.Byte) sublen);
					for(int i=offset;i<offset+sublen;i++)
					{
						os.WriteByte(buffer[i]);
					}
					//os.WriteByte(buffer,offset,sublen);
			
					length -= sublen;
					offset += sublen;
				}
				
				os.WriteByte((System.Byte) 'B');
				os.WriteByte((System.Byte) (length >> 8));
				os.WriteByte((System.Byte) length);
				for(int i=offset;i<offset+length;i++)
				{
					os.WriteByte(buffer[i]);
				}
			}
		}
		public void  writeByteBufferPart(byte[] buffer, int offset, int length)
		{
			while (length > 0)
			{
				int sublen = length;
				
				if (0x8000 < sublen)
					sublen = 0x8000;
				
				os.WriteByte((System.Byte) 'b');
				os.WriteByte((System.Byte) (sublen >> 8));
				os.WriteByte((System.Byte) sublen);
				for(int i=offset;i<offset+sublen;i++)
				{
					os.WriteByte(buffer[i]);
				}
				length -= sublen;
				offset += sublen;
			}
		}
		public void  writeByteBufferEnd(byte[] buffer, int offset, int length)
		{
			writeBytes(buffer, offset, length);
		}
		public void  writeRef(int _value)
		{
			os.WriteByte((System.Byte) 'R');
			os.WriteByte((System.Byte) (_value << 24));
			os.WriteByte((System.Byte) (_value << 16));
			os.WriteByte((System.Byte) (_value << 8));
			os.WriteByte((System.Byte) _value);
		}
		public virtual void  writePlaceholder()
		{
			os.WriteByte((System.Byte) 'P');
		}

		public bool addRef(System.Object _object)
		{
			
			Object _ref =  _refs[_object];
			
			if (_ref != null)
			{
				int _value = (int)_ref;
				
				writeRef(_value);
				return true;
			}
			else
			{
				_refs.Add(_object, (System.Object) _refs.Count);
				
				return false;
			}
		}


		public bool removeRef(System.Object obj)
		{
			if (_refs.Count>0)
			{
				_refs.Remove(obj);
				
				return true;
			}
			else
				return false;
		}

		public bool replaceRef(System.Object oldRef, System.Object newRef)
		{
			Object _value= _refs[oldRef];
 
			if (_value != null)
			{
				_refs.Remove(oldRef);
				_refs.Add(newRef, _value);
				return true;
			}
			else
				return false;	
		}
		public void  printLenString(System.String v)
		{
			if (v == null)
			{
				os.WriteByte((System.Byte) 0);
				os.WriteByte((System.Byte) 0);
			}
			else
			{
				int len = v.Length;
				os.WriteByte((System.Byte) (len >> 8));
				os.WriteByte((System.Byte) len);
				
				printString(v, 0, len);
			}
		}
		public void  printString(System.String v)
		{
			printString(v, 0, v.Length);
		}

		public void  printString(System.String v, int offset, int length)
		{
			for (int i = 0; i < length; i++)
			{
				char ch = v[i + offset];
				
				if (ch < 0x80)
					os.WriteByte((System.Byte) ch);
				else if (ch < 0x800)
				{
					os.WriteByte((System.Byte) (0xc0 + ((ch >> 6) & 0x1f)));
					os.WriteByte((System.Byte) (0x80 + (ch & 0x3f)));
				}
				else
				{
					os.WriteByte((System.Byte) (0xe0 + ((ch >> 12) & 0xf)));
					os.WriteByte((System.Byte) (0x80 + ((ch >> 6) & 0x3f)));
					os.WriteByte((System.Byte) (0x80 + (ch & 0x3f)));
				}
			}
		}
		public void  printString(char[] v, int offset, int length)
		{
			for (int i = 0; i < length; i++)
			{
				char ch = v[i + offset];
				
				if (ch < 0x80)
					os.WriteByte((System.Byte) ch);
				else if (ch < 0x800)
				{
					os.WriteByte((System.Byte) (0xc0 + ((ch >> 6) & 0x1f)));
					os.WriteByte((System.Byte) (0x80 + (ch & 0x3f)));
				}
				else
				{
					os.WriteByte((System.Byte) (0xe0 + ((ch >> 12) & 0xf)));
					os.WriteByte((System.Byte) (0x80 + ((ch >> 6) & 0x3f)));
					os.WriteByte((System.Byte) (0x80 + (ch & 0x3f)));
				}
			}
		}

	}
}
