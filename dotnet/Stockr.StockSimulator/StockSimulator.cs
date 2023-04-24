using Bogus;
using Core.Interfaces.Grains;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Stockr.API.Model;

namespace Stockr.StockSimulator;

public class StockSimulator : BackgroundService
{
    private readonly IClusterClient _client;
    private readonly SimulatorConfiguration _configuration;
    private readonly List<Stock> _stocks = new();
    private readonly Faker _faker = new();

    public StockSimulator(IClusterClient client, IOptions<SimulatorConfiguration> configuration)
    {
        _client = client;
        _configuration = configuration.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Randomizer.Seed = new Random(_configuration.RandomizerSeed);

        SeedStocks(GenerateUniqueStockSymbols());

        // Figure out which strategy to use for getting the index of the next stock
        Func<int, int> indexFunc = _configuration.UpdateStocksSequentially
            ? _ => _faker.Random.Int(0, _stocks.Count - 1)
            : n => n % _stocks.Count;
        
        var count = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var index = indexFunc.Invoke(count);
            _stocks[index] = GenerateNextStockValue(_stocks[index]);
            var value = _stocks[index];
            
            await _client.GetGrain<IStockGrain>(value.Symbol).UpdateStock(new StockUpdated(value, DateTimeOffset.Now));

            count++;
            await Task.Delay(_configuration.StockUpdateInterval, stoppingToken);
        }
    }

    private void SeedStocks(List<string> stockSymbols)
    {
        foreach (var symbol in stockSymbols)
        {
            var value = _faker.Random.Double(10, 500);
            var bid = value * 0.97;
            var ask = value * 1.03;
            _stocks.Add(new Stock(symbol, bid, ask));
        }
    }

    private List<string> GenerateUniqueStockSymbols()
    {
        var stockSymbols = new List<string>();
        
        while (stockSymbols.Count < _configuration.NumberOfStocks)
        {
            var symbol = _faker.Random.String2(3).ToUpper();
            if (!stockSymbols.Contains(symbol))
            {
                stockSymbols.Add(symbol);
            }
        }

        return stockSymbols;
    }
    
    private Stock GenerateNextStockValue(Stock stock)
    {
        var percentageChange = _faker.Random.Double(0, _configuration.MaximumStockChangePercent);
        var multiplier = _faker.Random.Bool() 
            ? 1 - percentageChange 
            : 1 + percentageChange;
        
        var ask = stock.Ask * multiplier;
        var bid = stock.Bid * multiplier;

        return new Stock(stock.Symbol, ask, bid);
    }
}