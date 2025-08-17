using System.Text;
using Client;
using Horizon.XmlRpc.Client;

var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
proxy.Url = "http://127.0.0.1:5678";

var result = proxy.AddNumbers(3, 4);
Console.WriteLine("Received AddNumbers result: " + result);

var sb = new StringBuilder();
for (var i = 0; i < 26; ++i)
    sb.Append((char)('A' + i), 20);
var echoData = sb.ToString();
for (var i = 0; i <= 1; ++i)
{
    var echoProxy = XmlRpcProxyGen.Create<IEchoServiceProxy>();
    echoProxy.Url = proxy.Url;
    if (i == 1)
    {
        echoProxy.EnableCompression = true;
        echoProxy.EnableClientCompression = true;
        echoData = new string(echoData.Reverse().ToArray());
    }

    var echoResult = echoProxy.Echo(echoData);
    Console.WriteLine(string.Format("Received Echo result OK: {0} ({1} == {2}, compression={3})", echoResult == echoData, echoResult.Length, echoData.Length,
        echoProxy.EnableCompression && echoProxy.EnableClientCompression));
}
