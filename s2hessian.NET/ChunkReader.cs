#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

namespace org.seasar.s2hessian.client
{
    public class ChunkReader
    {
        public ChunkReader(ObjectRead or)
        {
            _isLastChunk = false;
            _objectRead = or;
        }
        public ChunkReader(ObjectRead or, bool lastChunk)
        {
            _isLastChunk = lastChunk;
            _objectRead = or;

        }
        private bool _isLastChunk;
        private ObjectRead _objectRead;
        private int _chunkLength;
        public void SetIsLastChunk(bool lastChunk)
        {
            _isLastChunk = lastChunk;
        }
        public int ReadChunkLength()
        {
            return ParseUtil.ParseShort(_objectRead.GetReplyProcess().GetStreamReader());
        }
        public void InitLastChunk(bool isLastChunk)
        {
            _isLastChunk = isLastChunk;
            _chunkLength = ReadChunkLength();
        }
        public int parseChar()
        {
            if (!CheckAndGetNextChunk())
            {
                return -1;
            }
            _chunkLength--;

            return _objectRead.ParseUTF8Char();
        }
        private bool CheckAndGetNextChunk()
        {
            while (_chunkLength <= 0)
            {
                if (_isLastChunk)
                    return false;

                int code = _objectRead.GetReplyProcess().GetStreamReader().Read();
                switch (code)
                {

                    case 's':
                    case 'x':
                    case 'b':
                        _isLastChunk = false;

                        _chunkLength = _objectRead.GetReplyProcess().ReadLength();
                        break;

                    case 'S':
                    case 'X':
                    case 'B':
                        _isLastChunk = true;

                        _chunkLength = _objectRead.GetReplyProcess().ReadLength();
                        break;

                    default:
                        throw new HessianProtocolException("expected string or byte at " + (char)code);
                }

            }
            return true;
        }
        public int ParseByte()
        {
            if (!CheckAndGetNextChunk())
            {
                return -1;
            }
            _chunkLength--;
            int read = _objectRead.GetReplyProcess().GetStreamReader().Read();
            return read;
        }
    }
}
