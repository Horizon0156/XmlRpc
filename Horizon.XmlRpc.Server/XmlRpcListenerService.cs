using System;
using System.Net;

namespace Horizon.XmlRpc.Server
{
    public abstract class XmlRpcListenerService : XmlRpcHttpServerProtocol
    {
        public virtual void ProcessRequest(HttpListenerContext RequestContext)
        {
            try
            {
                IHttpRequest req = new XmlRpcListenerRequest(RequestContext.Request);
                IHttpResponse resp = new XmlRpcListenerResponse(RequestContext.Response);
                HandleHttpRequest(req, resp);
            }
            catch (Exception ex)
            {
                // "Internal server error"
                RequestContext.Response.StatusCode = 500;
                RequestContext.Response.StatusDescription = ex.Message;
            }
            finally
            {
                RequestContext.Response.OutputStream.Close();
            }
        }
    }
}