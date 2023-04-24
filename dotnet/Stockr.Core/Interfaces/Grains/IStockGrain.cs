using Orleans;
using Stockr.API.Model;

namespace Core.Interfaces.Grains;

public interface IStockGrain : IGrainWithStringKey
{
    Task UpdateStock(StockUpdated update);
    Task<Stock> GetStock();
    Task<IEnumerable<StockUpdated>> GetHistoricUpdates();
}