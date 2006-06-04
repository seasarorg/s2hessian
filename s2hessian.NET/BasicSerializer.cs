using System;

namespace org.seasar.s2hessian.client
{
	public class BasicSerializer:Serializer
	{
		public const int NULL = 0;
		public const int BOOLEAN = 1;
		public const int BYTE = 2;
		public const int SHORT = 3;
		public const int INTEGER = 4;
		public const int LONG = 5;
		public const int FLOAT = 6;
		public const int DOUBLE = 7;
		public const int CHARACTER = 8;
		public const int STRING = 9;
		public const int DATE = 10;
		public const int BOOLEAN_ARRAY= 11;
		public const int BYTE_ARRAY = 12;
		public const int SHORT_ARRAY = 13;
		public const int INTEGER_ARRAY = 14;
		public const int LONG_ARRAY = 15;
		public const int FLOAT_ARRAY = 16;
		public const int DOUBLE_ARRAY = 17;
		public const int CHARACTER_ARRAY = 18;
		public const int STRING_ARRAY = 19;
		public const int OBJECT_ARRAY = 20;
		
		private int code;
		
		public BasicSerializer(int code)
		{
			this.code = code;
		}
		
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			switch (code)
			{
				
				case BOOLEAN: 
					_out.writeBoolean(((System.Boolean) obj));
					break;
				
				
				case BYTE: 
				case SHORT: 
				case INTEGER: 
					_out.writeInt(System.Convert.ToInt32(((System.ValueType) obj)));
					break;
				
				
				case LONG: 
					_out.writeLong(System.Convert.ToInt64(((System.ValueType) obj)));
					break;
				
				
				case FLOAT: 
				case DOUBLE: 
					_out.writeDouble(System.Convert.ToDouble(((System.ValueType) obj)));
					break;
				
				
				case CHARACTER: 
					_out.writeInt(((System.Char) obj));
					break;
				
				
				case STRING: 
					_out.writeString((System.String) obj);
					break;
				
				
				case DATE: 
					_out.writeUTCDate(((System.DateTime) obj).Ticks);
					break;
				
				
				case BOOLEAN_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					bool[] data = (bool[]) obj;
					_out.writeListBegin(data.Length, "[boolean");
					for (int i = 0; i < data.Length; i++)
						_out.writeBoolean(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case BYTE_ARRAY: 
				{
					byte[] data = (byte[]) obj;
					_out.writeBytes(data, 0, data.Length);
					break;
				}
				
				
				case SHORT_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					short[] data = (short[]) obj;
					_out.writeListBegin(data.Length, "[short");
					for (int i = 0; i < data.Length; i++)
						_out.writeInt(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case INTEGER_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					int[] data = (int[]) obj;
					_out.writeListBegin(data.Length, "[int");
					for (int i = 0; i < data.Length; i++)
						_out.writeInt(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case LONG_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					long[] data = (long[]) obj;
					_out.writeListBegin(data.Length, "[long");
					for (int i = 0; i < data.Length; i++)
						_out.writeLong(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case FLOAT_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					float[] data = (float[]) obj;
					_out.writeListBegin(data.Length, "[float");
					for (int i = 0; i < data.Length; i++)
						_out.writeDouble(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case DOUBLE_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					double[] data = (double[]) obj;
					_out.writeListBegin(data.Length, "[double");
					for (int i = 0; i < data.Length; i++)
						_out.writeDouble(data[i]);
					_out.writeListEnd();
					break;
				}
				
				
				case STRING_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					System.String[] data = (System.String[]) obj;
					_out.writeListBegin(data.Length, "[string");
					for (int i = 0; i < data.Length; i++)
					{
						_out.writeString(data[i]);
					}
					_out.writeListEnd();
					break;
				}
				
				
				case CHARACTER_ARRAY: 
				{
					char[] data = (char[]) obj;
					_out.writeString(data, 0, data.Length);
					break;
				}
				
				
				case OBJECT_ARRAY: 
				{
					if (_out.addRef(obj))
						return ;
					
					System.Object[] data = (System.Object[]) obj;
					_out.writeListBegin(data.Length, "[object");
					for (int i = 0; i < data.Length; i++)
					{
						_out.writeObject(data[i]);
					}
					_out.writeListEnd();
					break;
				}
				
				
				default: 
					throw new System.SystemException(code + " " + System.Convert.ToString(obj.GetType()));
				
			}
		}
	}

}
