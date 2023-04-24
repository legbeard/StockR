using Core.Interfaces.Observers;
using Microsoft.AspNetCore.SignalR;
using Stockr.API.Model;

namespace Stockr.API.Hubs;

public interface IStockClient
{
    Task SendStockUpdate(Stock stockUpdate);
}

public class StockHub : Hub<IStockClient>
{
    // DI Observer and keep a reference to initialize and avoid GC.
    private readonly ICurrentStockObserver _observer;
    public StockHub(ICurrentStockObserver observer)
    {
        _observer = observer;
    }
    public async Task UpdateStock(Stock stock)
    {
        await Clients.All.SendStockUpdate(stock);
    }
}