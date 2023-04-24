using Stockr.API.Model;

namespace Stockr.API.Interface;

public interface IStockService
{
    Task<IEnumerable<Stock>> GetStocks();
    Task<Stock> GetStock(string symbol);
}