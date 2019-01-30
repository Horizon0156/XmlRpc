using Horizon.XmlRpc.Client;
using Demo.Contracts;

namespace Horizon.XmlRpc.Core.ClientDemo
{
    public interface IAddServiceProxy : IXmlRpcProxy, IAddService
    {
    }
}
