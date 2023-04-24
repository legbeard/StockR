using Core.Interfaces.Grains;
using Core.Interfaces.Observers;
using Microsoft.Extensions.Logging;
using Orleans.Utilities;
using Stockr.API.Model;

namespace Stockr.Silo.Grains;

public class CurrentStocksGrain: Grain, ICurrentStocksGrain
{
    private readonly Dictionary<string, Stock> _stocks = new();
    private readonly ObserverManager<ICurrentStockObserver> _observerManager;
    private readonly ILogger<CurrentStocksGrain> _logger;

    public CurrentStocksGrain(ILogger<CurrentStocksGrain> logger)
    {
        _logger = logger;
        // The timespan in this constructor is used to determine how long we wait until observers are cleared.
        // TODO: Ideally, figure out a better mechanism for maintaining and cleaning observers than "Just let it live for a long time", which is sunshine at best, naive at worst.
        _observerManager = new ObserverManager<ICurrentStockObserver>(TimeSpan.FromDays(9001), logger);
    }
    
    public Task UpdateStock(StockUpdated update)
    {
        var symbol = update.Symbol;
        
        _stocks[symbol] = _stocks.TryGetValue(symbol, out var stock) 
            ? new Stock(update, stock) 
            : new Stock(update);

        _observerManager.Notify(x => x.ReceiveStockUpdate(_stocks[update.Symbol]));
        return Task.CompletedTask;
    }

    public Task Subscribe(ICurrentStockObserver observer)
    {
        _observerManager.Subscribe(observer, observer);
        return Task.CompletedTask;
    }

    public Task Unsubscribe(ICurrentStockObserver observer)
    {
        _observerManager.Unsubscribe(observer);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Stock>> GetCurrentStocks()
    {
        return Task.FromResult<IEnumerable<Stock>>(_stocks.Values.ToList());
    }
}