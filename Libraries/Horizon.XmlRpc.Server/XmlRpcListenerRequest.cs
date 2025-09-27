using System.IO;
using System.Net;
using Horizon.XmlRpc.Core;

namespace Horizon.XmlRpc.Server
{
    public class XmlRpcListenerRequest : IHttpRequest
    {
        public XmlRpcListenerRequest(HttpListenerRequest request)
        {
            _request = request;
            var contentEncoding = _request.Headers["Content-Encoding"];
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                if (contentEncoding == "gzip")
                    _compression = DecompressionMethods.GZip;
                else if (contentEncoding == "deflate")
                    _compression = DecompressionMethods.Deflate;
            }
        }

        public Stream InputStream
        {
            get
            {
                if (_compression != DecompressionMethods.None)
                    return GetDecompressedStream();
                return _request.InputStream;
            }
        }

        public string HttpMethod
        {
            get { return _request.HttpMethod; }
        }

        private Stream GetDecompressedStream()
        {
            if (_decompressedStream == null)
            {
                var stream = _request.InputStream;
                if (_compression == DecompressionMethods.GZip)
                    _decompressedStream = Util.UnGZipStream(stream);
                else
                    _decompressedStream = Util.UnDeflateStream(stream);
            }
            return _decompressedStream;
        }

        private HttpListenerRequest _request;
        private DecompressionMethods _compression;
        private Stream _decompressedStream;
    }
}