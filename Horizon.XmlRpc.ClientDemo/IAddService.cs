using Horizon.XmlRpc.Client;

namespace Horizon.XmlRpc.Core.ClientDemo
{
    [XmlRpcUrl("http://127.0.0.1:5678")]
    public interface IAddService : IXmlRpcProxy
    {
        [XmlRpcMethod("Demo.addNumbers")]
        int AddNumbers(int numberA, int numberB);
    }
}
