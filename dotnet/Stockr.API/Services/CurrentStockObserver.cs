using Core.Interfaces.Grains;
using Core.Interfaces.Observers;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using Stockr.API.Hubs;
using Stockr.API.Model;

namespace Stockr.API.Services;

public class CurrentStockObserver : ICurrentStockObserver
{
    private readonly IHubContext<StockHub, IStockClient> _hubContext;

    // TODO: Fix the fact that this observer eventually dies to expiration.
    // We'd need to create a new observer only once per API instance and keep that alive.
    // BackgroundService and some mechanism to check whether the observer is still alive seem like a viable option.
    // See: https://learn.microsoft.com/en-us/dotnet/orleans/grains/observers, specifically the note about Observers being inherently unreliable.
    // This is a non-trivial fix.
    public CurrentStockObserver(IClusterClient clusterClient, IHubContext<StockHub, IStockClient> hubContext)
    {
        _hubContext = hubContext;
        var grain = clusterClient.GetGrain<ICurrentStocksGrain>(ICurrentStocksGrain.Name);
        var objectReference = clusterClient.CreateObjectReference<ICurrentStockObserver>(this);

        grain.Subscribe(objectReference);
    }

    public async Task ReceiveStockUpdate(Stock update)
    {
        Log.Information("Received stock update for '{symbol}'", update);
        await _hubContext.Clients.All.SendStockUpdate(update);
    }
}