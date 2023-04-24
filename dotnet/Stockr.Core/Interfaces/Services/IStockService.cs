using Stockr.API.Model;

namespace Stockr.API.Interface;

public interface IStockService
{
    Task<IEnumerable<Stock>> GetCurrentStocks();
    Task<Stock> GetStock(string symbol);
    Task<IEnumerable<StockUpdated>> GetHistoricStockValues(string symbol);
}