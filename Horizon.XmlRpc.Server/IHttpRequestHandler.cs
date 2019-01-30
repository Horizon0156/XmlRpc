namespace Horizon.XmlRpc.Server
{
    public interface IHttpRequestHandler
    {
        void HandleHttpRequest(IHttpRequest httpReq, IHttpResponse httpResp);
    }

}
