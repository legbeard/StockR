using Orleans;

namespace Stockr.API.Model;

[GenerateSerializer]
public record StockUpdated(Stock Stock, DateTimeOffset Timestamp)
{
}