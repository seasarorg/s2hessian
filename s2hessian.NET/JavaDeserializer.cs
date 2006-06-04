using System;

namespace org.seasar.s2hessian.client
{
	public class JavaDeserializer:AbstractMapDeserializer
	{
		override public System.Type Type
		{
			get
			{
				return _type;
			}
			
		}
		private System.Type _type;
		private System.Collections.Hashtable _fieldMap;
		private System.Reflection.MethodInfo _readResolve;
		private System.Reflection.ConstructorInfo _constructor;
		private System.Object[] _constructorArgs;
		
		public JavaDeserializer(System.Type cl)
		{
			_type = cl;
			_fieldMap = getFieldMap(cl);
			try
			{
				_readResolve = cl.GetMethod("readResolve", (new System.Type[0] == null)?new System.Type[0]:(System.Type[]) new System.Type[0]);
			}
			catch (System.Exception e)
			{
                throw new HessianServiceException();
            }
			
			System.Reflection.ConstructorInfo[] constructors = cl.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
			int bestLength = System.Int32.MaxValue;
			
			for (int i = 0; i < constructors.Length; i++)
			{
				if (constructors[i].GetParameters().Length < bestLength)
				{
					_constructor = constructors[i];
					bestLength = _constructor.GetParameters().Length;
				}
			}
			
			if (_constructor != null)
			{
				System.Reflection.ParameterInfo[] _params = _constructor.GetParameters();
				_constructorArgs = new System.Object[_params.Length];
				for (int i = 0; i < _params.Length; i++)
				{
					_constructorArgs[i] = getParamArg(_params[i].ParameterType);
				}
			}
		}

        public override System.Object readMap(ObjectRead _in)
        {
			try
			{
				System.Object obj = instantiate();
				
				return ReadMap(_in, obj);
            }
			catch (System.IO.IOException e)
			{
				throw e;
			}
			catch (System.SystemException e)
			{
				throw e;
			}
			catch (System.Exception e)
			{
				throw new System.IO.IOException(System.Convert.ToString(e));
			}
		}

        public virtual System.Object ReadMap(ObjectRead _in, System.Object obj)
        {
			try
			{
				int _ref = _in.AddRef(obj);
				
				while (!_in.GetReplyProcess().IsEnd())
				{
					System.Object key = _in.ReadObject();
					
					System.Reflection.FieldInfo field = (System.Reflection.FieldInfo) _fieldMap[key];
					
					if (field != null)
					{
						System.Object _value= _in.ReadObject(field.FieldType);
						field.SetValue(obj, _value);
					}
					else
					{
						System.Object _value= _in.ReadObject();
					}
				}

                _in.GetReplyProcess().ReadEnd();

                System.Object resolvex = Resolve(obj);
				
				if (obj != resolvex)
					_in.SetRef(_ref, resolvex);
				
				return resolvex;
			}
			catch (System.IO.IOException e)
			{
				throw e;
			}
			catch (System.Exception e)
			{

				throw new System.IO.IOException(System.Convert.ToString(e));
			}
		}
		
		private System.Object Resolve(System.Object obj)
		{
//            try
//            {
//                if (_readResolve != null)
//                    return _readResolve.Invoke(obj, (System.Object[])new System.Object[0]);
//            }
//            catch (System.Exception e)
//            {
//                throw new HessianServiceException();
//            }
//
            return obj;
		}
		
		protected internal virtual System.Object instantiate()
		{
			if (_constructor != null )
				return _constructor.Invoke(_constructorArgs);
			else
			{
				return System.Activator.CreateInstance(_type);
			}
		}
		
		protected internal virtual System.Collections.Hashtable getFieldMap(System.Type cl)
		{
			System.Collections.Hashtable fieldMap = new System.Collections.Hashtable();
			
			for (; cl != null; cl = cl.BaseType)
			{
				System.Reflection.FieldInfo[] fields = cl.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
				for (int i = 0; i < fields.Length; i++)
				{
					System.Reflection.FieldInfo field = fields[i];
					
					if (field.IsStatic)
						continue;
					else
					{
						if (fieldMap[field.Name] != null)
							continue;
					}
					
					try
					{
					}
					catch (System.Exception e)
					{

						throw new System.Exception();
					}
					
					fieldMap[field.Name] = field;
				}
			}
			
			return fieldMap;
		}
		
		protected internal static System.Object getParamArg(System.Type cl)
		{
			if (!cl.IsPrimitive)
				return null;
			else if (typeof(bool).Equals(cl))
				return false;
			else if (typeof(sbyte).Equals(cl))
				return (sbyte) 0;
			else if (typeof(short).Equals(cl))
				return (short) 0;
			else if (typeof(char).Equals(cl))
				return (char) 0;
			else if (typeof(int).Equals(cl))
				return 0;
			else if (typeof(long).Equals(cl))
				return 0;
			else if (typeof(float).Equals(cl))
				return (double) 0;
			else if (typeof(double).Equals(cl))
				return (double) 0;
			else
				throw new System.NotSupportedException();
		}
	}

}
