using CookComputing.XmlRpc;

namespace Horizon.XmlRpc.ClientDemo
{
    [XmlRpcUrl("http://localhost:5678")]
    public interface IAddService : IXmlRpcProxy
    {
        [XmlRpcMethod("Demo.addNumbers")]
        int AddNumbers(int numberA, int numberB);
    }
}
