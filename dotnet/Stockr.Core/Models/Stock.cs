using Orleans;

namespace Stockr.API.Model;

[GenerateSerializer]
public record Stock(string Symbol, double Bid, double Ask, double BidChange = 0, double AskChange = 0)
{
    public Stock(StockUpdated update) : this(update.Symbol, update.Bid, update.Ask) { }
    public Stock(StockUpdated update, Stock previous) : this(previous.Symbol, update.Bid, update.Ask, update.Bid - previous.Bid, update.Ask - previous.Ask) { }
};