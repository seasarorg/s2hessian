#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

#endregion

namespace org.seasar.s2hessian.client
{
    public class ObjectRead
    {
        public ObjectRead(ReplyProcess rp)
        {
            _replyProcess = rp;
            _serializerFactory = new SerializerFactory();
            _serializerFactory.setDotNetProxy(_replyProcess.GetDotNetProxy());
            _refs = new ArrayList();
        }
        private ReplyProcess _replyProcess;
        private SerializerFactory _serializerFactory;
        private ArrayList _refs;
        public ReplyProcess GetReplyProcess()
        {
            return _replyProcess;
        }
        public System.Object ReadObject()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {

                case 'N':
                    return null;


                case 'T':
                    return true;


                case 'F':
                    return false;


                case 'I':
                    return ParseInt();


                case 'L':
                    return (long)ParseLong();


                case 'D':
                    return (double)ParseDouble();


                case 'd':
                    long r = ParseLong();
                    return new System.DateTime((r + 62135629200000) * 10000);


//                case 'x':
//                case 'X':
//                    {
//                        _isLastChunk = tag == 'X';
//                        _chunkLength = (read() << 8) + read();
//
//                        return parseXML();
//                    }
//
//
                case 's':
                case 'S':
                    {

                        return _replyProcess.ReadString(tag == 'S');
                    }


                case 'b':
                case 'B':
                    {
                        return _replyProcess.ReadBytes(tag == 'B');
                    }


                case 'V':
                    {
                        System.String type = _replyProcess.ReadType();
                        int length = ReadListLength();

                        return _serializerFactory.readList(this, length, type);

                    }


                case 'M':
                    {
                        System.String type = _replyProcess.ReadType();
                        return _serializerFactory.readMap(this, type);

                    }


                case 'R':
                    {
                        return ReadRef();
                    }

//
//                case 'r':
//                    {
//                        System.String type = readType();
//                        System.String url = readString();
//
//                        return resolveRemote(type, url);
//                    }


                default:
                    throw new HessianProtocolException("unknown code:" + (char)tag);

            }
        }
        public System.Object ReadObject(System.Type expectedType)
        {
            if (expectedType == null)
            {
                return ReadObject();
            }
            int objectType =_replyProcess.GetStreamReader().Peek();

            switch (objectType)
            {
                case 'N':
                    _replyProcess.GetStreamReader().Read();
                    return null;


                case 'M':
                    {

                        _replyProcess.GetStreamReader().Read();
                        return ReadMap(expectedType);
                        
                    }


                case 'V':
                    {
                        _replyProcess.GetStreamReader().Read();
                        return ReadList(expectedType);

                    }


                case 'R':
                    {
                        _replyProcess.GetStreamReader().Read();
                        return ReadRef();
                    }

//
//                case 'r':
//                    {
//                        _replyProcess.GetStreamReader().Read();
//                        System.String type = readType();
//                        System.String url = readString();
//
//                        return resolveRemote(type, url);
//                    }



            }
            Object _value = _serializerFactory.getDeserializer(expectedType)
                .readObject(this);
            return _value;

        }
		public  int AddRef(System.Object _ref)
		{
			_refs.Add(_ref);
			return _refs.Count - 1;
		}
		public  void  SetRef(int i, System.Object _ref)
		{
			_refs[i] = _ref;
		}
		public  System.Object ReadRef()
		{
			return _refs[ParseInt()];
		}
        private Object ReadMap(System.Type expectedType)
        {
            System.String replyType = _replyProcess.ReadType();

            Deserializer reader;
            reader = _serializerFactory.getObjectDeserializer(replyType);

            if (expectedType != reader.Type && expectedType.IsAssignableFrom(reader.Type))
                return reader.readMap(this);

            reader = _serializerFactory.getDeserializer(expectedType);

            return reader.readMap(this);
        }
        private Object ReadList(System.Type expectedType)
        {
            System.String type = _replyProcess.ReadType();
            int length = ReadListLength();

            Deserializer reader;
            reader = _serializerFactory.getObjectDeserializer(type);

            if (expectedType != reader.Type && expectedType.IsAssignableFrom(reader.Type))
                return reader.readList(this, length);

            reader = _serializerFactory.getDeserializer(expectedType);

            System.Object v = reader.readList(this, length);

            return v;
        }
        public int ReadInt()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {
                case 'T': return 1;
                case 'F': return 0;
                case 'I': return ParseInt();
				case 'L':  return (int) ParseLong();
				case 'D':  return (int) ParseDouble();
                default:
                    throw new HessianProtocolException("expected int at " + (char)tag);

            }
        }

        public Boolean ReadBoolean()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {
                case 'T': return true;
                case 'F': return false;
                case 'I': return ParseInt() == 0;
                case 'L': return ParseLong() == 0;
                case 'D': return ParseDouble() == 0.0;
                case 'N': return false;
                default:
                    throw new HessianProtocolException("expected boolean at " + (char)tag);

            }
        }
        public long ReadLong()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {
                case 'T': return 1;
                case 'F': return 0;
                case 'I': return ParseInt();
                case 'L': return ParseLong();
                case 'D': return (long)ParseDouble();
                default:
                    throw new HessianProtocolException("expected long at " + (char)tag);
            }
        }
        public double ReadDouble()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {
                case 'T': return 1;
                case 'F': return 0;
                case 'I': return ParseInt();
                case 'L': return (double)ParseLong();
                case 'D': return ParseDouble();
                default:
                    throw new HessianProtocolException("expected long at " + (char)tag);

            }
        }

        public string ReadString()
        {
			int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {

                case 'N':
                    return null;


                case 'I':
                    return System.Convert.ToString(ParseInt());

                case 'L':
                    return System.Convert.ToString(ParseLong());

                case 'D':
                    return System.Convert.ToString(ParseDouble());


                case 'S':
                case 's':
                case 'X':
                case 'x':
                    return _replyProcess.ReadString(tag == 'S' || tag == 'X');


                default:
                    throw new HessianProtocolException("expected string at " + (char)tag);
            }
        }
        public byte[] ReadBytes()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            switch (tag)
            {

                case 'N':
                    return null;


                case 'B':
                case 'b':
                    return _replyProcess.ReadBytes(tag == 'B');
                default:
                    throw new HessianProtocolException("expected bytes at " + (char)tag);
            }
        }
        public long ReadUTCDate()
        {
            int tag = _replyProcess.GetStreamReader().Read();

            if (tag != 'd')
                throw new HessianProtocolException("expected date at " + (char)tag);

//            long b64 = read();
//            long b56 = read();
//            long b48 = read();
//            long b40 = read();
//            long b32 = read();
//            long b24 = read();
//            long b16 = read();
//            long b8 = read();
//
//            long r = ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) + (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);

            return ((ParseLong() + 62135629200000) * 10000);
        }
        public Int32 ParseInt()
        {
            int b32 = _replyProcess.GetStreamReader().Read();
            int b24 = _replyProcess.GetStreamReader().Read();
            int b16 = _replyProcess.GetStreamReader().Read();
            int b8 = _replyProcess.GetStreamReader().Read();
            return (b32 << 24) + (b24 << 16) + (b16 << 8) + b8;
        }
        public long ParseLong()
        {
            long b64 = _replyProcess.GetStreamReader().Read();
            long b56 = _replyProcess.GetStreamReader().Read();
            long b48 = _replyProcess.GetStreamReader().Read();
            long b40 = _replyProcess.GetStreamReader().Read();
            long b32 = _replyProcess.GetStreamReader().Read();
            long b24 = _replyProcess.GetStreamReader().Read();
            long b16 = _replyProcess.GetStreamReader().Read();
            long b8 =  _replyProcess.GetStreamReader().Read();
            return ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) +
                (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);
        }
        public double ParseDouble()
        {
//            long b64 = _replyProcess.GetStreamReader().Read();
//            long b56 = _replyProcess.GetStreamReader().Read();
//            long b48 = _replyProcess.GetStreamReader().Read();
//            long b40 = _replyProcess.GetStreamReader().Read();
//            long b32 = _replyProcess.GetStreamReader().Read();
//            long b24 = _replyProcess.GetStreamReader().Read();
//            long b16 = _replyProcess.GetStreamReader().Read();
//            long b8 = _replyProcess.GetStreamReader().Read();
//            long bits = ((b64 << 56) + (b56 << 48) + (b48 << 40) + (b40 << 32) + 
//                (b32 << 24) + (b24 << 16) + (b16 << 8) + b8);
            double dt = BitConverter.Int64BitsToDouble(ParseLong());
            return dt;
        }
        public int ParseUTF8Char()
        {
            int ch = _replyProcess.GetStreamReader().Read();

            if (ch < 0x80)
                return ch;
            else if ((ch & 0xe0) == 0xc0)
            {
                int ch1 = _replyProcess.GetStreamReader().Read();
                int v = ((ch & 0x1f) << 6) + (ch1 & 0x3f);

                return v;
            }
            else if ((ch & 0xf0) == 0xe0)
            {
                int ch1 = _replyProcess.GetStreamReader().Read();
                int ch2 = _replyProcess.GetStreamReader().Read();
                int v = ((ch & 0x0f) << 12) + ((ch1 & 0x3f) << 6) + (ch2 & 0x3f);

                return v;
            }
            else
                throw new HessianProtocolException("bad utf-8 encoding");
        }
        public int ReadListLength()
        {
            int code = _replyProcess.GetStreamReader().Peek();

            if (code != 'l')
            {
                return -1;
            }
            _replyProcess.GetStreamReader().Read();
            return ParseInt();
        }
    }
}
