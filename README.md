# Horizon.XmlRpc
This project targets to port Charles Cook's XML-RPC library to .NET Standard / .NET Core. [XML-RPC.NET](http://xml-rpc.net/) is a library for implementing XML-RPC Services and clients in the .NET environment

[XML-RPC.NET](http://xml-rpc.net/), the port and the extensions are released under the terms of the MIT license. See [License information](LICENSE.md) to get further information.

## Ported components
The library, respectively its main components were ported to .NET Standard while only implementing required changes concerning the access to changed Framwork components. Therefore, the API hasn't changed. In addition, the library was refactored into assemblies.
* `Horizon.XmlRpc.Core`
  Holds all the core members and contracts required by a XML-RPC servive as well as a XML-RPC client
* `Horizon.XmlRpc.Client`  
  Holds the ported `XmlRpcProxyGen`, the `IXmlRpcProxy`interface and their dependencies
* `Horizon.XmlRpc.Server`  
  Holds the ported `XmlRpcListenerService`, the documentation generator and their dependencies used to host a XML-RPC service in any .NET Standard compatible application.

## Added components
* `Horizon.XmlRpc.AspNetCore`  
Provides a middleware to host a XML-RPC service in an ASP.NET Core application

## Examples
This section gives you a brief introduction on how to write a service and how to consume the service with a generated client proxy.

To get a full example of running client and server in a .NET Core application, please have a look at the Demos in the repository.

First define a contrat for your service. This will be used by the client proxy and implemented from the service.

```
    using Horizon.XmlRpc.Core;

    public interface IAddService
    {
        [XmlRpcMethod("Demo.addNumbers")]
        int AddNumbers(int numberA, int numberB);
    }
```

Then define your service and start a new listener. To host the service with ASP.NET Cores please refer to the end of the documentation. 
```
using Horizon.XmlRpc.Server;

public class AddService : XmlRpcListenerService, IAddService
{
    public int AddNumbers(int numberA, int numberB)
    {
        return numberA + numberB;
    }
}

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

To consume the service, simply create a client proxy and use the contract to call its methods.

```
using Horizon.XmlRpc.Client;

public interface IAddServiceProxy : IXmlRpcProxy, IAddService
{
}

var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
proxy.Url = "http://127.0.0.1:5678";

var result = proxy.AddNumbers(3, 4);
Console.WriteLine("Received result: " + result);

// Prints 7 to the output
```

## Hosting XML-RPC services with ASP.NET Core
To be done...

For more information about XML-RPC refer to http://www.xmlrpc.com/
