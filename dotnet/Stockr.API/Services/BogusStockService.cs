﻿using Stockr.API.Interface;
using Stockr.API.Model;

namespace Stockr.API.Services;

public class BogusStockService : IStockService
{
    public IEnumerable<Stock> GetStocks()
    {
        return new[]
        {
            new Stock("Test", 1.1, 2.2),
            new Stock("Test", 1.1, 2.2),
            new Stock("Test", 1.1, 2.2)
        };
    }

    public Stock GetStock(string symbol)
    {
        return new Stock(symbol, 4.2, 6.9);
    }
}