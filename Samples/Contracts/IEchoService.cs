using Horizon.XmlRpc.Core;

namespace Contracts
{
    public interface IEchoService
    {
        [XmlRpcMethod("echo")]
        string Echo(string message);
    }
}
