using System;
using System.Threading.Tasks;
using Horizon.XmlRpc.AspNetCore.Adapter;
using Horizon.XmlRpc.AspNetCore.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Horizon.XmlRpc.AspNetCore.Routing
{
    internal class ServiceRouteBuilder : IServiceRouteBuilder
    {
        private readonly IRouteBuilder _routes;
        private readonly IXmlRpcServiceFactory _serviceFactory;

        public ServiceRouteBuilder(IRouteBuilder routes, IXmlRpcServiceFactory serviceFactory) 
        {
            _routes = routes;
            _serviceFactory = serviceFactory;
        }

        public IServiceRouteBuilder MapService<TService>(string template) where TService : XmlRpcService
        {
            _routes.MapRoute(template, DelegateRpcServiceRequest<TService>);

            return this;
        }

        private Task DelegateRpcServiceRequest<TService>(HttpContext context) where TService : XmlRpcService
        {
            var service = _serviceFactory.CreateService<TService>();
            return service.HandleHttpRequestAsync(context);
        }
    }
}