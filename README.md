# Horizon.XmlRpc
This project targets to port [Charles Cook's XML-RPC](http://xml-rpc.net) library to .NET.

_Do not wonder, the original library / website is no longer available._

## Installation
All libraries are available on [nuget.org](https://www.nuget.org)
* [Horizon.XmlRpc.Core](https://www.nuget.org/packages/Horizon.XmlRpc.Core/)
* [Horizon.XmlRpc.Client](https://www.nuget.org/packages/Horizon.XmlRpc.Client/)
* [Horizon.XmlRpc.Server](https://www.nuget.org/packages/Horizon.XmlRpc.Server/)

## Ported components
The library, respectively its main components were ported to .NET Standard. The APIs haven't changed. In addition, the library was refactored into three dedicated assemblies.

* `Horizon.XmlRpc.Core`
  Holds all the core members and contracts required by a XML-RPC servive as well as a XML-RPC client
* `Horizon.XmlRpc.Client`  
  Holds the ported `XmlRpcProxyGen`, the `IXmlRpcProxy`interface and their dependencies
* `Horizon.XmlRpc.Server`  
  Holds the ported `XmlRpcListenerService`, the documentation generator and their dependencies used to host a XML-RPC service in any .NET Standard compatible application.

## Examples
This section gives you a brief introduction on how to write a service and how to consume the service with a generated client proxy.

To get a full example of a running client and server, please have a look at the [Samples](./Samples), based on .NET 8 `BackgroundService`. 

First define a contrat for your service. This will be used by the client proxy and implemented from the service.

```C#
using Horizon.XmlRpc.Core;

public interface IAddService
{
    [XmlRpcMethod("Demo.addNumbers")]
    int AddNumbers(int numberA, int numberB);
}
```

Then define your service...

```C#
using Horizon.XmlRpc.Server;

public class AddService : XmlRpcListenerService, IAddService
{
    public int AddNumbers(int numberA, int numberB)
    {
        return numberA + numberB;
    }
}
````

and start a new listener.

```C#
using System.Net; 

var service = new AddService();
var listener = new HttpListener();
listener.Prefixes.Add("http://127.0.0.1:5678/");
listener.Start();

while (true)
{
    var context = listener.GetContext();
    service.ProcessRequest(context);
}
```

The service also provides an automatically generated documentation by browing to the opened enpoint http://127.0.0.1:5678/

To consume the service, simply create a client proxy...

```C#
using Horizon.XmlRpc.Client;

public interface IAddServiceProxy : IXmlRpcProxy, IAddService
{
}
````

 and use the proxy to call its methods. 

```C#
using Horizon.XmlRpc.Client;

var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
proxy.Url = "http://127.0.0.1:5678";

var result = proxy.AddNumbers(3, 4);
Console.WriteLine("Received result: " + result);

// Prints 7 to the output
```

For more information about XML-RPC in general refer to https://xmlrpc.com
