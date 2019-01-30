using System.IO;
using System.Net;

namespace Horizon.XmlRpc.Server
{
    public class XmlRpcListenerRequest : IHttpRequest
    {
        public XmlRpcListenerRequest(HttpListenerRequest request)
        {
            _request = request;
        }

        public Stream InputStream
        {
            get { return _request.InputStream; }
        }

        public string HttpMethod
        {
            get { return _request.HttpMethod; }
        }

        private HttpListenerRequest _request;
    }
}