using CookComputing.XmlRpc;
using System;

namespace CcuDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Collection devices from CCU...");
            var proxy = XmlRpcProxyGen.Create<IHomeMaticProxy>();
            proxy.Url = "http://192.168.178.49:2010";

            var result = proxy.ListDevices();

            foreach (var device in result)
            {
                Console.WriteLine(device);
            }

            Console.WriteLine("Turning Retro-Licht on...");
            //proxy.SetValue("000218A9928E62:3", "STATE", true); // Working but annoying ;)


            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }
    }
}
