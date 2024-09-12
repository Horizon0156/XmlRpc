
using System.Net;

namespace Server;

public class XmlRpcService(
    ILogger<XmlRpcService> logger,
    HttpListener httpListener,
    AddService addService) : BackgroundService
{
    private readonly ILogger<XmlRpcService> _logger = logger;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        httpListener.Prefixes.Add("http://127.0.0.1:5678/");
        httpListener.Start();

        _logger.LogInformation("XmlRpcService started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            var context = httpListener.GetContext();
            addService.ProcessRequest(context);
        }

        return Task.CompletedTask;
    }
}
