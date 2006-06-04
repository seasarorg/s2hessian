using System;

namespace org.seasar.s2hessian.client
{

		public class SerializerFactory
		{
			private static System.Collections.Hashtable _serializerMap;
			private static System.Collections.Hashtable _deserializerMap;
			private static System.Collections.Hashtable _typeMap;
		

			protected internal Serializer _defaultSerializer;
			private Deserializer _hashMapDeserializer;
			private System.Collections.Hashtable _cachedSerializerMap;
			private System.Collections.Hashtable _cachedDeserializerMap;
			private System.Collections.Hashtable _cachedTypeDeserializerMap;
			private DotNetProxy dnp;

			public virtual Serializer getSerializer(System.Type cl)
			{
				Serializer serializer;
			
				serializer = (Serializer) _serializerMap[cl];
				if (serializer != null)
					return serializer;
			
				if (_cachedSerializerMap != null)
				{
					serializer = (Serializer) _cachedSerializerMap[cl];
				
					if (serializer != null)
						return serializer;
				}
			
				if (typeof(System.Collections.IDictionary).IsAssignableFrom(cl))
					serializer = new MapSerializer();
				else if (typeof(System.Collections.ICollection).IsAssignableFrom(cl))
					serializer = new CollectionSerializer();
                else if (cl == typeof(System.Collections.Generic.List<>))
                    serializer = new CollectionSerializer();
				else if (cl.IsArray)
					serializer = new ArraySerializer();
				else
				{
					if (typeof(System.Exception).IsAssignableFrom(cl))
						serializer = new ThrowableSerializer();
					else if (typeof(System.IO.Stream).IsAssignableFrom(cl))
						serializer = new InputStreamSerializer();
					else
						serializer = getDefaultSerializer(cl);
				}
			
				if (_cachedSerializerMap == null)
				{
					_cachedSerializerMap = new System.Collections.Hashtable(8);
				}
			
				_cachedSerializerMap[cl] = serializer;
			
				return serializer;
			}

			protected internal virtual Serializer getDefaultSerializer(System.Type cl)
			{
				if (_defaultSerializer != null)
					return _defaultSerializer;
			
				return new JavaSerializer(cl);
			}

			public virtual Deserializer getDeserializer(System.Type cl)
			{
				Deserializer deserializer;
			
				deserializer = (Deserializer) _deserializerMap[cl];
				if (deserializer != null)
					return deserializer;
			
				if (_cachedDeserializerMap != null)
				{
					deserializer = (Deserializer) _cachedDeserializerMap[cl];
				
					if (deserializer != null)
						return deserializer;
				}
			
				if (typeof(System.Collections.IDictionary).IsAssignableFrom(cl))
					deserializer = new MapDeserializer(cl);
				else if (cl.IsArray)
					deserializer = new ArrayDeserializer(getDeserializer(cl.GetElementType()));
					
				else if	(typeof(System.Collections.ICollection).IsAssignableFrom(cl))
					deserializer = new CollectionDeserializer(cl);

				else
					deserializer = getDefaultDeserializer(cl);
			
				if (_cachedDeserializerMap == null)
				{
					_cachedDeserializerMap = new System.Collections.Hashtable(8);
				}
			
				_cachedDeserializerMap[cl] = deserializer;
			
				return deserializer;
			}
		

			protected internal virtual Deserializer getDefaultDeserializer(System.Type cl)
			{
				return new JavaDeserializer(cl);
			}

			public virtual System.Object readList(ObjectRead _in, int length, System.String type)
			{
				Deserializer deserializer = getDeserializer(type);

                if (deserializer != null)
                    return deserializer.readList(_in, length);
//				else
//					return new CollectionDeserializer(typeof(System.Collections.ArrayList)).readList(_in, length);
                //must delete 
                return null;

            }
		
			public virtual System.Object readMap(ObjectRead _in, System.String type)
			{
				Deserializer deserializer = getDeserializer(type);

                if (deserializer != null)
                    return deserializer.readMap(_in);
//				else if (_hashMapDeserializer != null)
//					return _hashMapDeserializer.readMap(_in);
//				else
//				{
//					_hashMapDeserializer = new MapDeserializer(typeof(System.Collections.Hashtable));
//				
//					return _hashMapDeserializer.readMap(_in);
//				}

                //must delete
                return null;
            }
		
			public virtual Deserializer getObjectDeserializer(System.String type)
			{
				Deserializer deserializer = getDeserializer(type);
			
				if (deserializer != null)
					return deserializer;
				else if (_hashMapDeserializer != null)
					return _hashMapDeserializer;
				else
				{
					_hashMapDeserializer = new MapDeserializer(typeof(System.Collections.Hashtable));
				
					return _hashMapDeserializer;
				}
			}
		
			public virtual Deserializer getDeserializer(System.String type)
			{
				if (type == null || type.Equals(""))
					return null;
			
				Deserializer deserializer;
			
				if (_cachedTypeDeserializerMap != null)
				{
					deserializer = (Deserializer) _cachedTypeDeserializerMap[type];
				}
			
				deserializer = (Deserializer) _typeMap[type];
				if (deserializer != null)
				{
					return deserializer;
				}
			
				if (type.StartsWith("["))
				{
					Deserializer subDeserializer = getDeserializer(type.Substring(1));
					deserializer = new ArrayDeserializer(subDeserializer);
				}
				else
				{
					try
					{
//						if (type=="nethessiantest.MyObject1") 
//						{
//							System.Type cl=typeof(nethessiantest.MyObject1);
//						}
						//System.Type cl=System.Type.GetType(type);
						System.Type cl=dnp.getConvertClass2(type);
						deserializer = getDeserializer(cl);
					}
					catch (System.Exception e)
					{
                        throw new HessianServiceException(e);
                    }
				}
			
				if (deserializer != null)
				{
					if (_cachedTypeDeserializerMap == null)
					{
						_cachedTypeDeserializerMap = new System.Collections.Hashtable(8);
					}
				
					_cachedTypeDeserializerMap[type] = deserializer;
				}
			
				return deserializer;
			}
		
			private static void  addBasic(System.Type cl, System.String typeName, int type)
			{
				_serializerMap[cl] = new BasicSerializer(type);
			
				Deserializer deserializer = new BasicDeserializer(type);
				_deserializerMap[cl] = deserializer;
				_typeMap[typeName] = deserializer;
			}
			public virtual void setDotNetProxy(DotNetProxy d)
			{
				dnp=d;
			}
			static SerializerFactory()
			{
				
				_serializerMap = new System.Collections.Hashtable();
				_deserializerMap = new System.Collections.Hashtable();
				_typeMap = new System.Collections.Hashtable();
				
				addBasic(typeof(System.Boolean), "boolean", BasicSerializer.BOOLEAN);
				addBasic(typeof(System.SByte), "byte", BasicSerializer.BYTE);
				addBasic(typeof(System.Int16), "short", BasicSerializer.SHORT);
				addBasic(typeof(System.Int32), "int", BasicSerializer.INTEGER);
				addBasic(typeof(System.Int64), "long", BasicSerializer.LONG);
				addBasic(typeof(System.Single), "float", BasicSerializer.FLOAT);
				addBasic(typeof(System.Double), "double", BasicSerializer.DOUBLE);
				addBasic(typeof(System.Char), "char", BasicSerializer.CHARACTER);
				addBasic(typeof(System.String), "string", BasicSerializer.STRING);
				addBasic(typeof(System.DateTime), "date", BasicSerializer.DATE);
				
				addBasic(typeof(bool), "boolean", BasicSerializer.BOOLEAN);
				addBasic(typeof(sbyte), "byte", BasicSerializer.BYTE);
				addBasic(typeof(short), "short", BasicSerializer.SHORT);
				addBasic(typeof(int), "int", BasicSerializer.INTEGER);
				addBasic(typeof(long), "long", BasicSerializer.LONG);
				addBasic(typeof(float), "float", BasicSerializer.FLOAT);
				addBasic(typeof(double), "double", BasicSerializer.DOUBLE);
				addBasic(typeof(char), "char", BasicSerializer.CHARACTER);
				
				addBasic(typeof(bool[]), "[boolean", BasicSerializer.BOOLEAN_ARRAY);
				addBasic(typeof(sbyte[]), "[byte", BasicSerializer.BYTE_ARRAY);
				addBasic(typeof(short[]), "[short", BasicSerializer.SHORT_ARRAY);
				addBasic(typeof(int[]), "[int", BasicSerializer.INTEGER_ARRAY);
				addBasic(typeof(long[]), "[long", BasicSerializer.LONG_ARRAY);
				addBasic(typeof(float[]), "[float", BasicSerializer.FLOAT_ARRAY);
				addBasic(typeof(double[]), "[double", BasicSerializer.DOUBLE_ARRAY);
				addBasic(typeof(char[]), "[char", BasicSerializer.CHARACTER_ARRAY);
				addBasic(typeof(System.String[]), "[string", BasicSerializer.STRING_ARRAY);
				addBasic(typeof(System.Object[]), "[object", BasicSerializer.OBJECT_ARRAY);
				
				_serializerMap[typeof(System.Type)] = new ClassSerializer();
				_deserializerMap[typeof(System.Type)] = new ClassDeserializer();
				
				_serializerMap[typeof(System.Decimal)] = new StringValueSerializer();
				try
				{
					_deserializerMap[typeof(System.Decimal)] = new StringValueDeserializer(typeof(System.Decimal));
				}
				catch (System.Exception e)
				{
                    throw new HessianServiceException();
                }
				
				_serializerMap[typeof(System.IO.FileInfo)] = new StringValueSerializer();
				try
				{
					_deserializerMap[typeof(System.IO.FileInfo)] = new StringValueDeserializer(typeof(System.IO.FileInfo));
				}
				catch (System.Exception e)
				{
                    throw new HessianServiceException();
                }
				
				try
				{
					System.Type stackTrace = System.Type.GetType("System.Diagnostics.StackTrace");
					
					_deserializerMap[stackTrace] = new StackTraceElementDeserializer();
				}
				catch (System.Exception e)
				{
                    throw new HessianServiceException();
                }
			}
			
		}
	
}
