using Core.Interfaces.Grains;
using Microsoft.AspNetCore.SignalR;
using Stockr.API.Hubs;
using Stockr.API.Interface;
using Stockr.API.Model;

namespace Stockr.API.Services;

public class OrleansStockService : IStockService
{
    private readonly IClusterClient _clusterClient;
    private readonly IHubContext<StockHub, IStockClient> _hubContext;

    public OrleansStockService(IClusterClient clusterClient, IHubContext<StockHub, IStockClient> hubContext)
    {
        _clusterClient = clusterClient;
        _hubContext = hubContext;
    }

    public async Task<IEnumerable<Stock>> GetCurrentStocks()
    {
        return await _clusterClient.GetGrain<ICurrentStocksGrain>(ICurrentStocksGrain.Name).GetCurrentStocks();
    }

    public async Task<Stock> GetStock(string symbol)
    {
        return await _clusterClient.GetGrain<IStockGrain>(symbol).GetStock();
    }

    public async Task<IEnumerable<StockUpdated>> GetHistoricStockValues(string symbol)
    {
        return await _clusterClient.GetGrain<IStockGrain>(symbol).GetHistoricUpdates();
    }
}