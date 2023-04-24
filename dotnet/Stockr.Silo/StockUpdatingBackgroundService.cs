using Bogus;
using Core.Interfaces.Grains;
using Microsoft.Extensions.Hosting;
using Serilog;
using Stockr.API.Model;

namespace Stockr.Silo;

public class StockUpdatingBackgroundService : BackgroundService
{
    private readonly IGrainFactory _grainFactory;

    public StockUpdatingBackgroundService(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stockNames = new[] { "ASD", "TSL", "MET", "VST", "BST" };
        var faker = new Faker<Stock>()
            .RuleFor(o => o.Symbol, f => f.PickRandom(stockNames))
            .RuleFor(o => o.Ask, f => f.Random.Double() * 100)
            .RuleFor(o => o.Bid, f => f.Random.Double() * 100);
        
        while (true)
        {
            var stock = faker.Generate();
            var grain = _grainFactory.GetGrain<IStockGrain>(stock.Symbol);
            await grain.UpdateStock(new StockUpdated(stock, DateTimeOffset.Now));
            await Task.Delay(5000, stoppingToken);
        }
    }
}