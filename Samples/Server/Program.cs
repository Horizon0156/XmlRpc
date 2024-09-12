using System.Net;
using Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<AddService>();
builder.Services.AddTransient<HttpListener>();
builder.Services.AddHostedService<XmlRpcService>();

var host = builder.Build();
host.Run();
