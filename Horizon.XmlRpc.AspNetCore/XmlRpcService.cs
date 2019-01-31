using System.Threading.Tasks;
using Horizon.XmlRpc.AspNetCore.Adapter;
using Horizon.XmlRpc.Server;
using Microsoft.AspNetCore.Http;

namespace Horizon.XmlRpc.AspNetCore
{
    public abstract class XmlRpcService : XmlRpcHttpServerProtocol
    {
        /// <summary>
        ///     Handles a HTTP reques to this service.
        /// </summary>
        /// <param name="context"> The context of the request. </param>
        public void HandleHttpRequest(HttpContext context)
        {
            HandleHttpRequest(
                new HttpRequestAdapter(context.Request),
                new HttpResponseAdapter(context.Response));
        }

        /// <summary>
        ///     Handles a HTTP request to this service asynchronous.
        /// </summary>
        /// <remarks>
        ///     Not really async. yet. Just a Task wrapper for convenient usage
        ///     with the ASP.NET Core pipeline.
        /// </remarks>
        /// <param name="context"> The context of the request. </param>
        public Task HandleHttpRequestAsync(HttpContext context)
        {
            // Async. not yet supported by XmlRpcHttpServerProtocol
            HandleHttpRequest(context);

            return Task.CompletedTask;
        }
    }
}