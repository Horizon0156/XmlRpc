using Client;
using Horizon.XmlRpc.Client;

var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
proxy.Url = "http://127.0.0.1:5678";

var result = proxy.AddNumbers(3, 4);
Console.WriteLine("Received result: " + result);