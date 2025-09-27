using Horizon.XmlRpc.Server;
using Contracts;

namespace Server;

public class Services : XmlRpcListenerService, IAddService, IEchoService
{
    public int AddNumbers(int numberA, int numberB)
    {
        return numberA + numberB;
    }

    public string Echo(string message)
    {
        return message;
    }
}