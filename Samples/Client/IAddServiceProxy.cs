using Contracts;
using Horizon.XmlRpc.Client;

namespace Client;

public interface IAddServiceProxy : IXmlRpcProxy, IAddService
{
}