using Core.Interfaces.Observers;
using Orleans;
using Stockr.API.Model;

namespace Core.Interfaces.Grains;

public interface ICurrentStocksGrain : IGrainWithStringKey
{
    public const string Name = "CurrentStocksGrain";
    Task UpdateStock(StockUpdated stock);
    Task<IEnumerable<Stock>> GetCurrentStocks();
    Task Subscribe(ICurrentStockObserver observer);
    Task Unsubscribe(ICurrentStockObserver observer);
}