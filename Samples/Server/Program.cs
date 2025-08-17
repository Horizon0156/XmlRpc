using System.Net;
using Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<Services>();
builder.Services.AddTransient<HttpListener>();
builder.Services.AddHostedService<XmlRpcService>();

var host = builder.Build();
host.Run();
