namespace Stockr.API.Model;

public record StockUpdated(string Symbol, double Bid, double Float, DateTimeOffset Timestamp)
{
}