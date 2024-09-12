using Horizon.XmlRpc.Server;
using Contracts;

namespace Server;

public class AddService : XmlRpcListenerService, IAddService
{
    public int AddNumbers(int numberA, int numberB)
    {
        return numberA + numberB;
    }
}