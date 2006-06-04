using System;

namespace org.seasar.s2hessian.client
{
	public class BasicDeserializer:Deserializer
	{
		override public System.Type Type
		{
			get
			{
				switch (_code)
				{
					
					case NULL: 
						return typeof(void );
					
					case BOOLEAN: 
						return typeof(System.Boolean);
					
					case BYTE: 
						return typeof(System.SByte);
					
					case SHORT: 
						return typeof(System.Int16);
					
					case INTEGER: 
						return typeof(System.Int32);
					
					case LONG: 
						return typeof(System.Int64);
					
					case FLOAT: 
						return typeof(System.Single);
					
					case DOUBLE: 
						return typeof(System.Double);
					
					case CHARACTER: 
						return typeof(System.Char);
					
					case STRING: 
						return typeof(System.String);
					
					case DATE: 
						return typeof(System.DateTime);
					
					
					case BOOLEAN_ARRAY: 
						return typeof(bool[]);
					
					case BYTE_ARRAY: 
						return typeof(sbyte[]);
					
					case SHORT_ARRAY: 
						return typeof(short[]);
					
					case INTEGER_ARRAY: 
						return typeof(int[]);
					
					case LONG_ARRAY: 
						return typeof(long[]);
					
					case FLOAT_ARRAY: 
						return typeof(float[]);
					
					case DOUBLE_ARRAY: 
						return typeof(double[]);
					
					case CHARACTER_ARRAY: 
						return typeof(char[]);
					
					case STRING_ARRAY: 
						return typeof(System.String[]);
					
					case OBJECT_ARRAY: 
						return typeof(System.Object[]);
					
					default: 
						throw new System.NotSupportedException();
					
				}
			}
			
		}
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
		
		private int _code;
		
		public BasicDeserializer(int code)
		{
			_code = code;
		}

        public override System.Object readObject(ObjectRead _in)
        {
            switch (_code)
            {

                case BOOLEAN:
                    return _in.ReadBoolean();


                case BYTE:
                    return (sbyte)_in.ReadInt();


                case SHORT:
                    return (short)_in.ReadInt();


                case INTEGER:
                    return (System.Int32)_in.ReadInt();


                case LONG:
                    return (long)_in.ReadLong();


                case FLOAT:
                    return (float)_in.ReadDouble();


                case DOUBLE:
                    return (double)_in.ReadDouble();


                case STRING:
                    return _in.ReadString();


                case CHARACTER:
                    {
                        System.String s = _in.ReadString();
                        if (s == null || s.Equals(""))
                            return null;
                        else
                            return s[0];
                    }

                case DATE:
                    return new System.DateTime(_in.ReadUTCDate());


                case BYTE_ARRAY:
                    return _in.ReadBytes();


                case CHARACTER_ARRAY:
                    {
                        System.String s = _in.ReadString();

                        if (s == null)
                            return null;
                        else
                        {
                            int len = s.Length;
                            char[] chars = new char[len];
                            chars = s.ToCharArray(0, len);
                            return chars;
                        }
                    }

                case BOOLEAN_ARRAY:
                case SHORT_ARRAY:
                case INTEGER_ARRAY:
                case LONG_ARRAY:
                case FLOAT_ARRAY:
                case DOUBLE_ARRAY:
                case STRING_ARRAY:
                    {
                        if (_in.GetReplyProcess().GetStreamReader().Read() == 'N')
                            return null;

                        System.String type = _in.GetReplyProcess().ReadType();
                        int length = _in.GetReplyProcess().ReadLength();

                        return readList(_in, length);
                    }


                default:
                    throw new System.NotSupportedException();

            }


        }

        public override System.Object readList(ObjectRead _in, int length)
        {
            switch (_code)
            {

                case BOOLEAN_ARRAY:
                    {
                        if (length >= 0)
                        {
                            bool[] data = new bool[length];

                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadBoolean();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add(_in.ReadBoolean());

                            _in.GetReplyProcess().ReadEnd();

                            bool[] data = new bool[list.Count];

                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = ((System.Boolean)list[i]);

                            return data;
                        }
                    }


                case SHORT_ARRAY:
                    {
                        if (length >= 0)
                        {
                            short[] data = new short[length];

                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = (short)_in.ReadInt();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add((short)_in.ReadInt());

                            _in.GetReplyProcess().ReadEnd();

                            short[] data = new short[list.Count];
                            for (int i = 0; i < data.Length; i++)
                                data[i] = (short)((System.Int16)list[i]);

                            _in.AddRef(data);

                            return data;
                        }
                    }


                case INTEGER_ARRAY:
                    {
                        if (length >= 0)
                        {
                            int[] data = new int[length];

                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadInt();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add((System.Int32)_in.ReadInt());

                            _in.GetReplyProcess().ReadEnd();

                            int[] data = new int[list.Count];
                            for (int i = 0; i < data.Length; i++)
                                data[i] = ((System.Int32)list[i]);

                            _in.AddRef(data);

                            return data;
                        }
                    }


                case LONG_ARRAY:
                    {
                        if (length >= 0)
                        {
                            long[] data = new long[length];

                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadLong();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add((long)_in.ReadLong());

                            _in.GetReplyProcess().ReadEnd();

                            long[] data = new long[list.Count];
                            for (int i = 0; i < data.Length; i++)
                                data[i] = (long)list[i];

                            _in.AddRef(data);

                            return data;
                        }
                    }


                case FLOAT_ARRAY:
                    {
                        if (length >= 0)
                        {
                            float[] data = new float[length];
                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                            {
                                data[i] = (float)_in.ReadDouble();
                            }

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add((float)_in.ReadDouble());

                            _in.GetReplyProcess().ReadEnd();

                            float[] data = new float[list.Count];
                            for (int i = 0; i < data.Length; i++)
                            {
                                data[i] = (float)list[i];
                            }

                            _in.AddRef(data);

                            return data;
                        }
                    }

                case DOUBLE_ARRAY:
                    {
                        if (length >= 0)
                        {
                            double[] data = new double[length];
                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadDouble();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add((double)_in.ReadDouble());

                            _in.GetReplyProcess().ReadEnd();

                            double[] data = new double[list.Count];
                            _in.AddRef(data);
                            for (int i = 0; i < data.Length; i++)
                                data[i] = (double)list[i];

                            return data;
                        }
                    }


                case STRING_ARRAY:
                    {
                        if (length >= 0)
                        {
                            System.String[] data = new System.String[length];
                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadString();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add(_in.ReadString());

                            _in.GetReplyProcess().ReadEnd();

                            System.String[] data = new System.String[list.Count];
                            _in.AddRef(data);
                            for (int i = 0; i < data.Length; i++)
                                data[i] = ((System.String)list[i]);

                            return data;
                        }
                    }


                case OBJECT_ARRAY:
                    {
                        if (length >= 0)
                        {
                            System.Object[] data = new System.Object[length];
                            _in.AddRef(data);

                            for (int i = 0; i < data.Length; i++)
                                data[i] = _in.ReadObject();

                            _in.GetReplyProcess().ReadEnd();

                            return data;
                        }
                        else
                        {
                            System.Collections.ArrayList list = new System.Collections.ArrayList();

                            _in.AddRef(list);

                            while (!_in.GetReplyProcess().IsEnd())
                                list.Add(_in.ReadObject());

                            _in.GetReplyProcess().ReadEnd();

                            System.Object[] data = new System.Object[list.Count];
                            for (int i = 0; i < data.Length; i++)
                                data[i] = (System.Object)list[i];

                            return data;
                        }
                    }


                default:
                    throw new System.NotSupportedException(System.Convert.ToString(this));

            }

        }
	}
}
