
using System;
using Horizon.XmlRpc.AspNetCore.Factories;
using Horizon.XmlRpc.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Horizon.XmlRpc.AspNetCore.Extensions
{
    /// <summary>
    ///     Provides extension for working with XmlRpc.
    /// </summary>
    public static class XmlRpcAppBuilderExtensions
    {
        // <summary>
        ///     Uses XmlRpc services within ASP.NET Core pipeline.
        /// </summary>
        /// <param name="builder"> The builder where to register the usage. </param>
        /// <param name="configure"> Configuration for registering one or more XmlRpc services. </param>
        /// <returns> This builder for fluent usage. </returns>
        public static IApplicationBuilder UseXmlRpc(this IApplicationBuilder builder, Action<IServiceRouteBuilder> configure) 
        {
            var routes = new RouteBuilder(builder);
            var rpcRoutes = new ServiceRouteBuilder(
                routes, 
                builder.ApplicationServices.GetService<IXmlRpcServiceFactory>());
            
            configure(rpcRoutes);

            builder.UseRouter(routes.Build());

            return builder;
        }
    }
}