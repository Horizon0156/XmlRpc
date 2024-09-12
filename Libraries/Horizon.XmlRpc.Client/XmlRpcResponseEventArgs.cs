using System;
using System.IO;

namespace Horizon.XmlRpc.Client
{
    public class XmlRpcResponseEventArgs : EventArgs
    {
        private Guid _guid;
        private long _request;
        private Stream _responseStream;

        public XmlRpcResponseEventArgs(Guid guid, long request,
          Stream responseStream)
        {
            _guid = guid;
            _request = request;
            _responseStream = responseStream;
        }

        public Guid ProxyID
        {
            get { return _guid; }
        }

        public long RequestNum
        {
            get { return _request; }
        }

        public Stream ResponseStream
        {
            get { return _responseStream; }
        }
    }
}

