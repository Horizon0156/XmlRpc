using System;
using System.IO;
using System.Net;

namespace Horizon.XmlRpc.Server
{
    public class XmlRpcListenerResponse : IHttpResponse
    {
        public XmlRpcListenerResponse(HttpListenerResponse response)
        {
            this.response = response;
            response.SendChunked = false;
        }

        Int64 IHttpResponse.ContentLength
        {
            set { response.ContentLength64 = value; }
        }

        string IHttpResponse.ContentType
        {
            get { return response.ContentType; }
            set { response.ContentType = value; }
        }

        TextWriter IHttpResponse.Output
        {
            get { return new StreamWriter(response.OutputStream); }
        }

        Stream IHttpResponse.OutputStream
        {
            get { return response.OutputStream; }
        }

        int IHttpResponse.StatusCode
        {
            get { return response.StatusCode; }
            set { response.StatusCode = value; }
        }

        string IHttpResponse.StatusDescription
        {
            get { return response.StatusDescription; }
            set { response.StatusDescription = value; }
        }

        private HttpListenerResponse response;
    }
}