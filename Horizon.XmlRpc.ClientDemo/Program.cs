using System;
using CookComputing.XmlRpc;

namespace Horizon.XmlRpc.ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var proxy = XmlRpcProxyGen.Create<IAddService>();
            Console.WriteLine("Calling Demo.addNumbers with [3,4]...");
            var result = proxy.AddNumbers(3, 4);
            Console.WriteLine("Received result: " + result);
            Console.ReadKey();
        }
    }
}
