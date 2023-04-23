using Microsoft.AspNetCore.SignalR;
using Stockr.API.Model;

namespace Stockr.API.Hubs;

public interface IStockClient
{
    Task SendStockUpdate(StockUpdated stockUpdate);
}

public class StockHub : Hub<IStockClient>
{
    public async Task UpdateStock(StockUpdated stock)
    {
        await Clients.All.SendStockUpdate(stock);
    }
}