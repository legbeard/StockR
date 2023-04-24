using Core.Interfaces.Grains;
using Stockr.API.Model;

namespace Stockr.Silo.Grains;

public class StockGrain : Grain, IStockGrain
{
    private readonly List<StockUpdated> _updateHistory = new();
    private Stock _current;
    private DateTimeOffset _lastUpdated;

    public Task UpdateStock(StockUpdated update)
    {
        _current = _current != null ? new Stock(update, _current) : new Stock(update);
        _lastUpdated = update.Timestamp;
        _updateHistory.Add(update);

        var currentStocksGrain = GrainFactory.GetGrain<ICurrentStocksGrain>(ICurrentStocksGrain.Name);
        currentStocksGrain.UpdateStock(update);
        return Task.CompletedTask;
    }

    public Task<Stock> GetStock()
    {
        return Task.FromResult(_current);
    }

    public Task<IEnumerable<StockUpdated>> GetHistoricUpdates()
    {
        return Task.FromResult<IEnumerable<StockUpdated>>(_updateHistory);
    }
}