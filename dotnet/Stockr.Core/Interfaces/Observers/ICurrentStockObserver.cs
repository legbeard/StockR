using Orleans;
using Stockr.API.Model;

namespace Core.Interfaces.Observers;

public interface ICurrentStockObserver : IGrainObserver
{
    Task ReceiveStockUpdate(StockUpdated update);
}