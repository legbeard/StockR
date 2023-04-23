using Microsoft.AspNetCore.SignalR;
using Serilog;
using Stockr.API.Hubs;
using Stockr.API.Model;

namespace Stockr.API;

public class StockUpdatingBackgroundService : BackgroundService
{
    private readonly IHubContext<StockHub, IStockClient> _hubContext;

    public StockUpdatingBackgroundService(IHubContext<StockHub, IStockClient> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            await _hubContext.Clients.All.SendStockUpdate(new StockUpdated(new Stock("Test1", counter, counter + 1.1), DateTimeOffset.Now));
            counter++;
            await Task.Delay(5000, stoppingToken);
        }
    }
}