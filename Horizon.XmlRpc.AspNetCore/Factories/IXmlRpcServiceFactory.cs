namespace Horizon.XmlRpc.AspNetCore.Factories
{
    internal interface IXmlRpcServiceFactory
    {
        XmlRpcService CreateService<TService>() where TService : XmlRpcService;
    }
}