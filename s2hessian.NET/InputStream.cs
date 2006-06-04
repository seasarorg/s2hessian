using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace org.seasar.s2hessian.client
{

	public class InputStream
	{
		IFormatter bformatter = new BinaryFormatter();
//		MemoryStream mdstream = new MemoryStream();
//		double dt = 10.0;
	
//		int mdstreamLength;
		protected SerializerFactory _serializerFactory;
		protected ArrayList _refs;
		protected Stream _is;
		protected int _peek = -1;
		private string _method;

//		private Stream _chunkInputStream;
		private System.Boolean _isLastChunk;
		private int _chunkLength;
		private System.Exception _replyFault;
		private static int END_OF_DATA = - 2;
		private DotNetProxy dnp;

		public InputStream(Stream ism,DotNetProxy dnp)
		{
			this.dnp =dnp;
			init(ism);
	
//			bformatter.Serialize(mdstream, dt);
//			mdstreamLength=(int)mdstream.Length;
		
		}
		public void setSerializerFactory(SerializerFactory factory)
		{
			_serializerFactory = factory;


		}
		public SerializerFactory getSerializerFactory()
		{
			return _serializerFactory;
		}
		public void init(Stream ism)
		{
			_is=ism;
			_method=null;
			_isLastChunk = true;
			_chunkLength = 0;
			_peek = - 1;
			_refs = null;
			_replyFault = null;
			if (_serializerFactory == null)
			{
				_serializerFactory = new SerializerFactory();
				_serializerFactory.setDotNetProxy(dnp);

			}
			
		}
		public string getMethod()
		{
			return _method;
		}
		virtual public System.Exception ReplyFault
		{
			get
			{
				return _replyFault;
			}
			
		}
		public void  startCall()
		{
			readCall();
			
			while (readHeader() != null)
			{
				readObject();
			}
			
			readMethod();
		}

		public int readCall()
		{
			int tag = read();
			
			if (tag != 'c')
				throw error("expected hessian call ('c') at code=" + tag + " ch=" + (char) tag);
			
			int major = read();
			int minor = read();
			
			return (major << 16) + minor;
		}

		public virtual System.String readMethod()
		{
			int tag = read();
			
			if (tag != 'm')
				throw error("expected hessian method ('m') at code=" + tag + " ch=" + (char) tag);
			int d1 = read();
			int d2 = read();
			
			_isLastChunk = true;
			_chunkLength = d1 * 256 + d2;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int ch;
			while ((ch = parseChar()) >= 0)
				sb.Append((char) ch);
			
			_method = sb.ToString();
			
			return _method;
		}


		public void  completeCall()
		{
			int tag = read();
			
			if (tag != 'z')
				throw expect("end of call", tag);
		}


		public System.Object readReply(System.Type expectedClass)
		{
			int tag = read();
			
			if (tag != 'r')
				error("expected hessian reply");
			
			int major = read();
			int minor = read();
			
			tag = read();
			if (tag == 'f')
			{
				throw prepareFault();
			}
			else
			{
				_peek = tag;
				Object _value = readObject(expectedClass);
				completeValueReply();
				return _value;
			}
		}
		public  void  startReply()
		{
			int tag = read();
			
			if (tag != 'r')
				error("expected hessian reply");
			
			int major = read();
			int minor = read();
			
			tag = read();
			if (tag == 'f')
				throw prepareFault();
			else
				_peek = tag;
		}


		private System.Exception prepareFault()
		{
			Hashtable fault = readFault();
			completeReply();
			Object detail = fault["detail"];
			System.String message = (System.String) fault["message"];
			System.String code = (System.String) fault["code"];
			_replyFault =  new HessianServiceException(message, code, detail);
			return _replyFault;
		}

		public  void  completeReply()
		{
			int tag = read();
			
			if (tag != 'z')
				error("expected end of reply");
		}

		public virtual void  completeValueReply()
		{
			int tag = read();
			
			if (tag != 'z')
				error("expected end of reply");
		}
		
		public virtual System.String readHeader()
		{
			int tag = read();
			
			if (tag == 'H')
			{
				_isLastChunk = true;
				_chunkLength = (read() << 8) + read();
				
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int ch;
				while ((ch = parseChar()) >= 0)
					sb.Append((char) ch);
				
				return sb.ToString();
			}
			
			_peek = tag;
			
			return null;
		}

		public  void  readNull()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'N':  return ;
				
				
				default: 
					throw expect("null", tag);
				
			}
		}

		public  bool readBoolean()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'T':  return true;
				
				case 'F':  return false;
				
				case 'I':  return parseInt() == 0;
				
				case 'L':  return parseLong() == 0;
				
				case 'D':  return parseDouble() == 0.0;
				
				case 'N':  return false;
				
				
				default: 
					throw expect("boolean", tag);
				
			}
		}
		public virtual short readShort()
		{
			return (short) readInt();
		}

		public  int readInt()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'T':  return 1;
				
				case 'F':  return 0;
				
				case 'I':  return parseInt();
				
				case 'L':  return (int) parseLong();
				
				case 'D':  return (int) parseDouble();
				
				
				default: 
					throw expect("int", tag);
				
			}
		}
		public  long readLong()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'T':  return 1;
				
				case 'F':  return 0;
				
				case 'I':  return parseInt();
				
				case 'L':  return parseLong();
				
				case 'D':  return (long) parseDouble();
				
				
				default: 
					throw expect("long", tag);
				
			}
		}

		public virtual float readFloat()
		{
			return (float) readDouble();
		}

		public  double readDouble()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'T':  return 1;
				
				case 'F':  return 0;
				
				case 'I':  return parseInt();
				
				case 'L':  return (double) parseLong();
				
				case 'D':  return parseDouble();
				
				
				default: 
					throw expect("long", tag);
				
			}
		}
		public  long readUTCDate()
		{
			int tag = read();
			
			if (tag != 'd')
				throw error("expected date");
			
			long b64 = read();
			long b56 = read();
			long b48 = read();
			long b40 = read();
			long b32 = read();
			long b24 = read();
			long b16 = read();
			long b8 = read();
			
			long r= ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) + (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);
			
			return ((r+62135629200000)*10000);
		}
		public virtual int readChar()
		{
			if (_chunkLength > 0)
			{
				_chunkLength--;
				if (_chunkLength == 0 && _isLastChunk)
					_chunkLength = END_OF_DATA;
				
				int ch = parseUTF8Char();
				return ch;
			}
			else if (_chunkLength == END_OF_DATA)
			{
				_chunkLength = 0;
				return - 1;
			}
			
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return - 1;
				
				
				case 'S': 
				case 's': 
				case 'X': 
				case 'x': 
					_isLastChunk = tag == 'S' || tag == 'X';
					_chunkLength = (read() << 8) + read();
					
					_chunkLength--;
					int _value = parseUTF8Char();

					if (_chunkLength == 0 && _isLastChunk)
						_chunkLength = END_OF_DATA;
					
					return _value;
				
				
				default: 
					throw new System.IO.IOException("expected 'S' at " + (char) tag);
				
			}
		}

		public virtual int readString(char[] buffer, int offset, int length)
		{
			int readLength = 0;
			
			if (_chunkLength == END_OF_DATA)
			{
				_chunkLength = 0;
				return - 1;
			}
			else if (_chunkLength == 0)
			{
				int tag = read();
				
				switch (tag)
				{
					
					case 'N': 
						return - 1;
					
					
					case 'S': 
					case 's': 
					case 'X': 
					case 'x': 
						_isLastChunk = tag == 'S' || tag == 'X';
						_chunkLength = (read() << 8) + read();
						break;
					
					
					default: 
						throw new System.IO.IOException("expected 'S' at " + (char) tag);
					
				}
			}
			
			while (length > 0)
			{
				if (_chunkLength > 0)
				{
					buffer[offset++] = (char) parseUTF8Char();
					_chunkLength--;
					length--;
					readLength++;
				}
				else if (_isLastChunk)
				{
					if (readLength == 0)
						return - 1;
					else
					{
						_chunkLength = END_OF_DATA;
						return readLength;
					}
				}
				else
				{
					int tag = read();
					
					switch (tag)
					{
						
						case 'S': 
						case 's': 
						case 'X': 
						case 'x': 
							_isLastChunk = tag == 'S' || tag == 'X';
							_chunkLength = (read() << 8) + read();
							break;
						
						
						default: 
							throw new System.IO.IOException("expected 'S' at " + (char) tag);
						
					}
				}
			}
			
			if (readLength == 0)
				return - 1;
			else if (_chunkLength > 0 || !_isLastChunk)
				return readLength;
			else
			{
				_chunkLength = END_OF_DATA;
				return readLength;
			}
		}
		

		public  System.String readString()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return null;
				
				
				case 'I': 
					return System.Convert.ToString(parseInt());
				
				case 'L': 
					return System.Convert.ToString(parseLong());
				
				case 'D': 
					return System.Convert.ToString(parseDouble());
				
				
				case 'S': 
				case 's': 
				case 'X': 
				case 'x': 
					_isLastChunk = tag == 'S' || tag == 'X';
					_chunkLength = (read() << 8) + read();
					
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					int ch;
					
					while ((ch = parseChar()) >= 0)
						sb.Append((char) ch);
					
					return sb.ToString();
				
				
				default: 
					throw expect("string", tag);
				
			}
		}

		public  System.Xml.XmlNode readNode()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return null;
				
				
				case 'S': 
				case 's': 
				case 'X': 
				case 'x': 
					_isLastChunk = tag == 'S' || tag == 'X';
					_chunkLength = (read() << 8) + read();
					
					throw error("can't cope");
				
				
				default: 
					throw expect("string", tag);
				
			}
		}

		public  byte[] readBytes()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return null;
				
				
				case 'B':
				case 'b': 
					_isLastChunk = tag == 'B';
					_chunkLength = (read() << 8) + read();
					
					System.IO.MemoryStream bos = new System.IO.MemoryStream();
					
					int data;
					while ((data = parseByte()) >= 0)
					{
						bos.WriteByte((System.Byte) data);
					}
					
					return bos.ToArray();
				
				
				default: 
					throw expect("bytes", tag);

				
			}
		}

		public virtual int readByte()
		{
			if (_chunkLength > 0)
			{
				_chunkLength--;
				if (_chunkLength == 0 && _isLastChunk)
					_chunkLength = END_OF_DATA;
				
				return read();
			}
			else if (_chunkLength == END_OF_DATA)
			{
				_chunkLength = 0;
				return - 1;
			}
			
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return - 1;
				
				
				case 'B': 
				case 'b': 
					_isLastChunk = tag == 'B';
					_chunkLength = (read() << 8) + read();
					
					int _value = parseByte();
					
					if (_chunkLength == 0 && _isLastChunk)
						_chunkLength = END_OF_DATA;
					
					return _value;
				
				
				default: 
					throw new System.IO.IOException("expected 'B' at " + (char) tag);
				
			}
		}

		public virtual int readBytes(sbyte[] buffer, int offset, int length)
		{
			int readLength = 0;
			
			if (_chunkLength == END_OF_DATA)
			{
				_chunkLength = 0;
				return - 1;
			}
			else if (_chunkLength == 0)
			{
				int tag = read();
				
				switch (tag)
				{
					
					case 'N': 
						return - 1;
					
					
					case 'B': 
					case 'b': 
						_isLastChunk = tag == 'B';
						_chunkLength = (read() << 8) + read();
						break;
					
					
					default: 
						throw new System.IO.IOException("expected 'B' at " + (char) tag);
					
				}
			}
			
			while (length > 0)
			{
				if (_chunkLength > 0)
				{
					buffer[offset++] = (sbyte) read();
					_chunkLength--;
					length--;
					readLength++;
				}
				else if (_isLastChunk)
				{
					if (readLength == 0)
						return - 1;
					else
					{
						_chunkLength = END_OF_DATA;
						return readLength;
					}
				}
				else
				{
					int tag = read();
					
					switch (tag)
					{
						
						case 'B': 
						case 'b': 
							_isLastChunk = tag == 'B';
							_chunkLength = (read() << 8) + read();
							break;
						
						
						default: 
							throw new System.IO.IOException("expected 'B' at " + (char) tag);
						
					}
				}
			}
			
			if (readLength == 0)
				return - 1;
			else if (_chunkLength > 0 || !_isLastChunk)
				return readLength;
			else
			{
				_chunkLength = END_OF_DATA;
				return readLength;
			}
		}

		private System.Collections.Hashtable readFault()
		{
			System.Collections.Hashtable map = new System.Collections.Hashtable();
			
			int code = read();
			for (; code > 0 && code != 'z'; code = read())
			{
				_peek = code;
				
				System.Object key = readObject();
				System.Object _value = readObject();
				
				if (key != null && _value != null)
					map[key] = _value;
			}
			
			if (code != 'z')
				throw expect("fault", code);
			
			return map;
		}

		public  System.Object readObject(System.Type cl)
		{
			
			if (cl == null)
			{
				return readObject();
			}
			
			int tag = read();
			
			switch (tag)
			{
				case 'N': 
					return null;
				
				
//				case 'M': 
//				{
//
//					System.String type = readType();
//
//					Deserializer reader;
//					reader = _serializerFactory.getObjectDeserializer(type);
//					
//					if (cl != reader.Type && cl.IsAssignableFrom(reader.Type))
//						return reader.readMap(this);
//					
//					reader = _serializerFactory.getDeserializer(cl);
//					
//					return reader.readMap(this);
//				}
//				
//				
//				case 'V': 
//				{
//					System.String type = readType();
//					int length = readLength();
//					
//					Deserializer reader;
//					reader = _serializerFactory.getObjectDeserializer(type);
//					
//					if (cl != reader.Type && cl.IsAssignableFrom(reader.Type))
//						return reader.readList(this, length);
//					
//					reader = _serializerFactory.getDeserializer(cl);
//					
//					System.Object v = reader.readList(this, length);
//					
//					return v;
//				}
//				
//				
//				case 'R': 
//				{
//					int _ref = parseInt();
//					
//					return _refs[_ref];
//				}
//				
//				
//				case 'r': 
//				{
//					System.String type = readType();
//					System.String url = readString();
//					
//					return resolveRemote(type, url);
//				}

				

			}
			
			_peek = tag;
            // must shuusei
			//Object _value = _serializerFactory.getDeserializer(cl).readObject(this);
            Object _value = null;
            return _value;
		}

		public  System.Object readObject()
		{
			int tag = read();
			
			switch (tag)
			{
				
				case 'N': 
					return null;
				
				
				case 'T': 
					return true;
				
				
				case 'F': 
					return false;
				
				
				case 'I': 
					return (System.Int32) parseInt();
				
				
				case 'L': 
					return (long) parseLong();
				
				
				case 'D': 
					return (double) parseDouble();
				
				
				case 'd': 
					long r=parseLong();
					return new System.DateTime((r + 62135629200000) * 10000);
				
				
				case 'x': 
				case 'X':  
				{
					_isLastChunk = tag == 'X';
					_chunkLength = (read() << 8) + read();
						
					return parseXML();
				}
				
				
				case 's': 
				case 'S':  
				{
					_isLastChunk = tag == 'S';
					_chunkLength = (read() << 8) + read();
						
					int data;
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
						
					while ((data = parseChar()) >= 0)
						sb.Append((char) data);
						
					return sb.ToString();
				}
				
				
				case 'b': 
				case 'B':  
				{
					_isLastChunk = tag == 'B';
					_chunkLength = (read() << 8) + read();
						
					int data;
					System.IO.MemoryStream bos = new System.IO.MemoryStream();
						
					while ((data = parseByte()) >= 0)
						bos.WriteByte((System.Byte) data);
						
					return bos.ToArray();
				}
				
				
				case 'V':  
				{
//					System.String type = readType();
//					int length = readLength();
//						
//					return _serializerFactory.readList(this, length, type);
                    return null;

                }
				
				
				case 'M':  
				{
//					System.String type = readType();
//					return _serializerFactory.readMap(this, type);
                    return null;

                }
				
				
				case 'R':  
				{
					int _ref = parseInt();
						
					return _refs[_ref];
				}
				
				
				case 'r':  
				{
					System.String type = readType();
					System.String url = readString();
						
					return resolveRemote(type, url);
				}
				
				
				default: 
					throw error("unknown code:" + (char) tag);
				
			}
		}

		public  System.Object readRef()
		{
			return _refs[parseInt()];
		}
		
		public  int readListStart()
		{
			return read();
		}
		
		/// <summary> Reads the start of a list.</summary>
		public  int readMapStart()
		{
			return read();
		}
		public bool isEnd()
		
			{
				int code = read();

		_peek = code;

		return (code < 0 || code == 'z');
		}		

		public  void  readEnd()
		{
			int code = read();
			
			if (code != 'z')
				throw error("unknown code:" + (char) code);
		}
		
		public  void  readMapEnd()
		{
			int code = read();
			
			if (code != 'z')
				throw error("unknown code:" + (char) code);
		}
		
		public  void  readListEnd()
		{
			int code = read();
			
			if (code != 'z')
				throw error("unknown code:" + (char) code);
		}
		
		public  int addRef(System.Object _ref)
		{
			if (_refs == null)
				_refs = new System.Collections.ArrayList();
			
			_refs.Add(_ref);
			
			return _refs.Count - 1;
		}
		
		public  void  setRef(int i, System.Object _ref)
		{
			_refs[i] = _ref;
		}
		
		public virtual System.Object resolveRemote(System.String type, System.String url)
		{
			return null;
		}
		
		public  System.String readType()
		{
			int code = read();
			
			if (code != 't')
			{
				_peek = code;
				return "";
			}
			
			_isLastChunk = true;
			_chunkLength = (read() << 8) + read();
			
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int ch;
			while ((ch = parseChar()) >= 0)
				sb.Append((char) ch);
			
			string type=sb.ToString();
			type=dnp.getConvertClass(type);
			return type;
		}
		
		public  int readLength()
		{
			int code = read();
			
			if (code != 'l')
			{
				_peek = code;
				return - 1;
			}
			
			return parseInt();
		}
		
		private int parseInt()
		{
			int b32 = read();
			int b24 = read();
			int b16 = read();
			int b8 = read();
			
			return (b32 << 24) + (b24 << 16) + (b16 << 8) + b8;
		}

		private long parseLong()
		{
			long b64 = read();
			long b56 = read();
			long b48 = read();
			long b40 = read();
			long b32 = read();
			long b24 = read();
			long b16 = read();
			long b8 = read();
			
			
			return ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) + (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);
		}
		

		private double parseDouble()
		{
//
//
//			mdstream.Seek(mdstreamLength-9,System.IO.SeekOrigin.Begin);
//
//			byte []data=new byte[8];
//			for (int i=0;i<8;i++)
//			{
//				data[i]=(byte)read();
//			}
//			for(int i=0;i<8;i++)
//			{
//				mdstream.WriteByte(data[7-i]);
//			}
//			mdstream.Seek(0,System.IO.SeekOrigin.Begin);
//			dt=(double)bformatter.Deserialize(mdstream);
			long b64 = read();
			long b56 = read();
			long b48 = read();
			long b40 = read();
			long b32 = read();
			long b24 = read();
			long b16 = read();
			long b8 = read();
			
			long bits = ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) + (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);
			double dt=BitConverter.Int64BitsToDouble(bits);
			return dt;
		}
		
		internal virtual System.Xml.XmlNode parseXML()
		{
			throw new System.NotSupportedException();
		}
		

		private int parseChar()
		{
			while (_chunkLength <= 0)
			{
				if (_isLastChunk)
					return - 1;
				
				int code = read();
				
				switch (code)
				{
					
					case 's': 
					case 'x': 
						_isLastChunk = false;
						
						_chunkLength = (read() << 8) + read();
						break;
					
					
					case 'S': 
					case 'X': 
						_isLastChunk = true;
						
						_chunkLength = (read() << 8) + read();
						break;
					
					
					default: 
						throw expect("string", code);
					
				}
			}
			
			_chunkLength--;
			
			return parseUTF8Char();
		}
		

		private int parseUTF8Char()
		{
			int ch = read();
			
			if (ch < 0x80)
				return ch;
			else if ((ch & 0xe0) == 0xc0)
			{
				int ch1 = read();
				int v = ((ch & 0x1f) << 6) + (ch1 & 0x3f);
				
				return v;
			}
			else if ((ch & 0xf0) == 0xe0)
			{
				int ch1 = read();
				int ch2 = read();
				int v = ((ch & 0x0f) << 12) + ((ch1 & 0x3f) << 6) + (ch2 & 0x3f);
				
				return v;
			}
			else
				throw error("bad utf-8 encoding");
		}
		
		private int parseByte()
		{
			while (_chunkLength <= 0)
			{
				if (_isLastChunk)
				{
					return - 1;
				}
				
				int code = read();
				
				switch (code)
				{
					
					case 'b': 
						_isLastChunk = false;
						
						_chunkLength = (read() << 8) + read();
						break;
					
					
					case 'B': 
						_isLastChunk = true;
						
						_chunkLength = (read() << 8) + read();
						break;
					
					
					default: 
						throw expect("byte[]", code);
					
				}
			}
			
			_chunkLength--;
			
			return read();
		}
		

		internal virtual int read(byte[] buffer, int offset, int length)
		{
			int readLength = 0;
			
			while (length > 0)
			{
				while (_chunkLength <= 0)
				{
					if (_isLastChunk)
						return readLength == 0?- 1:readLength;
					
					int code = read();
					
					switch (code)
					{
						
						case 'b': 
							_isLastChunk = false;
							
							_chunkLength = (read() << 8) + read();
							break;
						
						
						case 'B': 
							_isLastChunk = true;
							
							_chunkLength = (read() << 8) + read();
							break;
						
						
						default: 
							throw expect("byte[]", code);
						
					}
				}
				
				int sublen = _chunkLength;
				if (length < sublen)
					sublen = length;
				
				sublen = _is.Read(buffer, offset, sublen);
				offset += sublen;
				readLength += sublen;
				length -= sublen;
				_chunkLength -= sublen;
			}
			
			return readLength;
		}
		
		internal int read()
		{
			if (_peek >= 0)
			{
				int _value = _peek;
				_peek = - 1;
				return _value;
			}
			
			int ch = _is.ReadByte();

			return ch;
		}
		
		protected internal virtual System.IO.IOException expect(System.String expect, int ch)
		{
			if (ch < 0)
				return error("expected " + expect + " at end of file");
			else
				return error("expected " + expect + " at " + (char) ch);
		}
		
		protected internal virtual System.IO.IOException error(System.String message)
		{
			return new HessianProtocolException(message);
		}

	}











}
