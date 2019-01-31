
using System.IO;
using Horizon.XmlRpc.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Horizon.XmlRpc.AspNetCore.Adapter
{
    internal class HttpResponseAdapter : IHttpResponse
    {
        private readonly HttpResponse _adaptee;

        public HttpResponseAdapter(HttpResponse adaptee) 
        {
            _adaptee = adaptee;
        }

        public long ContentLength { set => _adaptee.ContentLength = value; }
        
        public string ContentType { get => _adaptee.ContentType; set => _adaptee.ContentType = value; }

        public TextWriter Output => new StreamWriter(_adaptee.Body);

        public Stream OutputStream => _adaptee.Body;

        public int StatusCode { get => _adaptee.StatusCode; set => _adaptee.StatusCode = value; }
        
        public string StatusDescription { get => ReasonPhrases.GetReasonPhrase(_adaptee.StatusCode); set => _ = value;}
    }
}