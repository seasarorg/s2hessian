#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

namespace org.seasar.s2hessian.client
{
    public class ReplyProcess
    {
        public ReplyProcess(Stream sr, DotNetProxy dnp)
        {
            _streamReader = new PeekStreamReader(sr);
            _dnp = dnp;
            _objectRead = new ObjectRead(this);
            _chunkReader = new ChunkReader(_objectRead);

        }
        private PeekStreamReader _streamReader;
        private DotNetProxy _dnp;
        private ObjectRead _objectRead;
        private ChunkReader _chunkReader;
        public PeekStreamReader GetStreamReader()
        {
            return _streamReader;
        }
        public ChunkReader GetChunkReader()
        {
            return _chunkReader;
        }
        public System.Object Execute(System.Type expectedClass)
        {
            StartReply();
            int[] version = ReadVersion();
            CheckFaultReply();
            Object returnObject = _objectRead.ReadObject(expectedClass);
            // CompleteReply();

            return returnObject;
        }
        private void StartReply()
        {
            if (_streamReader.Read() != 'r')
            {
                throw new HessianProtocolException("expected hessian reply");
            }
        }
        public DotNetProxy GetDotNetProxy()
        {
            return _dnp;
        }
        private int[] ReadVersion()
        {
            int[] version = new int[2];
            version[0] = _streamReader.Read();
            version[1] = _streamReader.Read();
            return version;
        }
        private void CheckFaultReply()
        {
            if (_streamReader.Peek() == 'f')
            {
                throw new HessianProtocolException("TODO fault reply");
            }
        }
        private bool CheckStartType()
        {
            if (_streamReader.Peek() == 't')
            {
                _streamReader.Read();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEnd()
        {
            int code = _streamReader.Peek();

            return (code < 0 || code == 'z');
        }
        public void ReadEnd()
        {
            int code = _streamReader.Read();

            if (code != 'z')
                throw new HessianProtocolException("unknown code:" + (char)code);
        }
        public System.String ReadType()
        {

            if (!CheckStartType())
            {
                return "";
            }

            string type = ReadString(true);
            type = _dnp.getConvertClass(type);
            return type;
        }
        public int ReadLength()
        {
            int read = (_streamReader.Read() << 8) + _streamReader.Read();
            return read;
        }
        public String ReadString(bool isLastChunk)
        {
            _chunkReader.InitLastChunk(isLastChunk);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int ch;
            while ((ch = _chunkReader.parseChar()) >= 0)
                sb.Append((char)ch);

            return sb.ToString();
        }
        public byte[] ReadBytes(bool isLastChunk)
        {
            _chunkReader.InitLastChunk(isLastChunk);
            System.IO.MemoryStream bos = new System.IO.MemoryStream();

            int data;
            while ((data = _chunkReader.ParseByte()) >= 0)
            {
                bos.WriteByte((System.Byte)data);
            }

            return bos.ToArray();
        }
    }
}
