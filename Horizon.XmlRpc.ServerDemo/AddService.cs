using CookComputing.XmlRpc;

namespace Horizon.XmlRpc.ServerDemo
{
    internal class AddService : XmlRpcListenerService
    {
        [XmlRpcMethod("Demo.addNumbers", Description = "Return product of number a and number b")]
        public int AddNumbers(int numberA, int numberB)
        {
            System.Console.WriteLine($"Received request to Demo.addNumbers. Parameters: [{numberA}, {numberB}]");
            return numberA + numberB;
        }
    }
}
