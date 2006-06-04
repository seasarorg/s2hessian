using System;

namespace org.seasar.s2hessian.client

{
	public class JavaSerializer:Serializer
	{
        private System.Reflection.FieldInfo[] _fields;

        public System.Reflection.FieldInfo[] Fields
        {
            get { return _fields; }
        }
		private System.Reflection.MethodInfo _writeReplace;
		
		public JavaSerializer(System.Type cl)
		{
			try
			{
				_writeReplace = cl.GetMethod("writeReplace", (new System.Type[0] == null)?new System.Type[0]:(System.Type[]) new System.Type[0]);
			}
			catch (System.Exception e)
			{
                throw new HessianServiceException();
            }
			
			System.Collections.ArrayList primitiveFields = new System.Collections.ArrayList();
			System.Collections.ArrayList compoundFields = new System.Collections.ArrayList();
			
			for (; cl != null; cl = cl.BaseType)
			{
				System.Reflection.FieldInfo[] fields = cl.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
				for (int i = 0; i < fields.Length; i++)
				{
					System.Reflection.FieldInfo field = fields[i];
					
					if (field.IsStatic)
						continue;
					
					if (field.FieldType.IsPrimitive || field.FieldType.FullName.StartsWith("java.lang.") && !field.FieldType.Equals(typeof(System.Object)))
						primitiveFields.Add(field);
					else
						compoundFields.Add(field);
				}
			}
			
			System.Collections.ArrayList fields2 = new System.Collections.ArrayList();
			fields2.AddRange(primitiveFields);
			fields2.AddRange(compoundFields);
			
			_fields = new System.Reflection.FieldInfo[fields2.Count];
			_fields=(System.Reflection.FieldInfo[])fields2.ToArray(typeof(System.Reflection.FieldInfo));
		}
		
		public override void  writeObject(System.Object obj, OutputStream _out)
		{
			if (_out.addRef(obj))
				return ;
			
			System.Type cl = obj.GetType();
			
			try
			{
				if (_writeReplace != null)
				{
					System.Object repl = _writeReplace.Invoke(obj, (System.Object[]) new System.Object[0]);
					
					_out.removeRef(obj);
					
					_out.writeObject(repl);
					
					_out.replaceRef(repl, obj);
					
					return ;
				}
			}
			catch (System.Exception e)
			{
                throw new HessianServiceException();
            }
			
			try
			{
				_out.writeMapBegin(_out.getDnp().getConvertClass(cl));
				
				for (int i = 0; i < _fields.Length; i++)
				{
					System.Reflection.FieldInfo field = _fields[i];
					
					_out.writeString(field.Name);
					_out.writeObject(field.GetValue(obj));
				}
				_out.writeMapEnd();
			}
			catch (System.UnauthorizedAccessException e)
			{
				throw new System.IO.IOException(System.Convert.ToString(e));
			}
		}
	}
}
