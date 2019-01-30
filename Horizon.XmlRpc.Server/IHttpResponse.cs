using System;
using System.IO;

namespace Horizon.XmlRpc.Server
{
    public interface IHttpResponse
    {
        Int64 ContentLength { set; }
        string ContentType { get; set; }
        TextWriter Output { get; }
        Stream OutputStream { get; }
        bool SendChunked { get; set; }
        int StatusCode { get; set; }
        string StatusDescription { get; set; }
    }
}
