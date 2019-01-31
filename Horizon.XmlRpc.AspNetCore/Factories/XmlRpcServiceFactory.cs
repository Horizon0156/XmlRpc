using System;
using Microsoft.Extensions.DependencyInjection;

namespace Horizon.XmlRpc.AspNetCore.Factories
{
    internal class XmlRpcServiceFactory : IXmlRpcServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public XmlRpcServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public XmlRpcService CreateService<TService>() where TService : XmlRpcService 
        {
            return ActivatorUtilities.CreateInstance<TService>(_serviceProvider);
        }
    }
}