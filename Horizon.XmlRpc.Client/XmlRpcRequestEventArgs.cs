using System;
using System.IO;

namespace Horizon.XmlRpc.Client
{
    public class XmlRpcRequestEventArgs : EventArgs
    {
        private Guid _guid;
        private long _request;
        private Stream _requestStream;

        public XmlRpcRequestEventArgs(Guid guid, long request, Stream requestStream)
        {
            _guid = guid;
            _request = request;
            _requestStream = requestStream;
        }

        public Guid ProxyID
        {
            get { return _guid; }
        }

        public long RequestNum
        {
            get { return _request; }
        }

        public Stream RequestStream
        {
            get { return _requestStream; }
        }
    }
}
