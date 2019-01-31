
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
    public static class XmlRpcServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds required services for using XmlRpc with ASP.NET Core.
        /// </summary>
        /// <param name="serviceCollection"> The service collection that will be extended. </param>
        /// <returns> This collection for fluent usage. </returns>
        public static IServiceCollection AddXmlRpc(this IServiceCollection serviceCollection) 
        {
            serviceCollection.AddRouting();
            serviceCollection.AddSingleton<IXmlRpcServiceFactory, XmlRpcServiceFactory>();

            return serviceCollection;
        }
    }   
}