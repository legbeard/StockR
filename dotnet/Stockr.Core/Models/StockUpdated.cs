using Orleans;

namespace Stockr.API.Model;

[GenerateSerializer]
public record StockUpdated(string Symbol, double Bid, double Ask, DateTimeOffset Timestamp);