using Stockr.API.Model;

namespace Stockr.API.Interface;

public interface IStockService
{
    IEnumerable<Stock> GetStocks();
    Stock GetStock(string symbol);
}