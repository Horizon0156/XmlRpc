using System;
using Horizon.XmlRpc.Client;

namespace Horizon.XmlRpc.Core.ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
            proxy.Url = "http://127.0.0.1:5678";

            Console.WriteLine("Calling Demo.addNumbers with [3,4]...");
            var result = proxy.AddNumbers(3, 4);
            Console.WriteLine("Received result: " + result);
        }
    }
}
