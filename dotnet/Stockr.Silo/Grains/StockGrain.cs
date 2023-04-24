using Core.Interfaces.Grains;
using Stockr.API.Model;

namespace Stockr.Silo.Grains;

public class StockGrain : Grain, IStockGrain
{
    private Stock _stock;
    private DateTimeOffset _lastUpdated;
    
    public Task UpdateStock(StockUpdated update)
    {
        _stock = update.Stock;
        _lastUpdated = update.Timestamp;

        var currentStocksGrain = GrainFactory.GetGrain<ICurrentStocksGrain>(ICurrentStocksGrain.Name);
        currentStocksGrain.UpdateStock(update);
        return Task.CompletedTask;
    }

    public Task<Stock> GetStock()
    {
        return Task.FromResult(_stock);
    }
}