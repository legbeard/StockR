namespace Stockr.API.Model;

public record StockUpdated(Stock Stock, DateTimeOffset Timestamp)
{
}