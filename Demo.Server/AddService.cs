using Demo.Contracts;
using Horizon.XmlRpc.Server;

namespace Horizon.XmlRpc.ServerDemo
{
    internal class AddService : XmlRpcListenerService, IAddService
    {
        public int AddNumbers(int numberA, int numberB)
        {
            System.Console.WriteLine($"Received request to Demo.addNumbers. Parameters: [{numberA}, {numberB}]");
            return numberA + numberB;
        }
    }
}
