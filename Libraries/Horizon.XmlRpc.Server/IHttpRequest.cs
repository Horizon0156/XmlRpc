using System.IO;

namespace Horizon.XmlRpc.Server
{
    public interface IHttpRequest
    {
        Stream InputStream { get; }
        string HttpMethod { get; }
    }
}