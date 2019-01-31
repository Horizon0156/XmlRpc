
using System.IO;
using Horizon.XmlRpc.Server;
using Microsoft.AspNetCore.Http;

namespace Horizon.XmlRpc.AspNetCore.Adapter
{
    internal class HttpRequestAdapter : IHttpRequest
    {
        private readonly HttpRequest _adaptee;

        public HttpRequestAdapter(HttpRequest adaptee) 
        {
            _adaptee = adaptee;
        }
        public Stream InputStream => _adaptee.Body;

        public string HttpMethod => _adaptee.Method;
    }
}