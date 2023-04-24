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
    private readonly List<StockUpdated> _lastStockUpdates = new();
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
            ? n => n % _lastStockUpdates.Count
            : _ => _faker.Random.Int(0, _lastStockUpdates.Count - 1);
        
        var count = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var index = indexFunc.Invoke(count);
            _lastStockUpdates[index] = GenerateNextStockUpdate(_lastStockUpdates[index]);
            await EmitStockUpdate(_lastStockUpdates[index]);
            await Task.Delay(_configuration.StockUpdateInterval, stoppingToken);
            count++;
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

    private async Task SeedStocks(List<string> stockSymbols)
    {
        foreach (var symbol in stockSymbols)
        {
            var initialValue = _faker.Random.Double(10, 500);
            var initialUpdate = new StockUpdated(symbol, initialValue * 0.97, initialValue * 1.03, DateTimeOffset.Now);
            await EmitStockUpdate(initialUpdate);
            _lastStockUpdates.Add(initialUpdate);
        }
    }

    
    private StockUpdated GenerateNextStockUpdate(StockUpdated update)
    {
        var percentageChange = _faker.Random.Double(0, _configuration.MaximumStockChangePercent);
        var multiplier = _faker.Random.Bool() 
            ? 1 - percentageChange 
            : 1 + percentageChange;
        
        var ask = update.Ask * multiplier;
        var bid = update.Bid * multiplier;

        return new StockUpdated(update.Symbol, bid, ask, DateTimeOffset.Now);
    }

    private async Task EmitStockUpdate(StockUpdated update)
    {
        await _client.GetGrain<IStockGrain>(update.Symbol).UpdateStock(update);
    }
}