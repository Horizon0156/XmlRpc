namespace Horizon.XmlRpc.AspNetCore.Routing
{
    /// <summary>
    ///     Builder for XmlRpc Service Routing.
    /// </summary>
    public interface IServiceRouteBuilder
    {
        /// <summary>
        ///     Registers the given services to be reachable on the given route.
        /// </summary>
        /// <param name="template"> Template for the service route. </param>
        /// <typeparam name="TService"> Type of the service. </typeparam>
        /// <returns> This builder for fluent usage. </returns>
        IServiceRouteBuilder MapService<TService>(string template) where TService : XmlRpcService;
    }
}