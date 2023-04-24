namespace Stockr.StockSimulator;

public class SimulatorConfiguration
{
    public const string Position = "Simulator";
    public TimeSpan StockUpdateInterval { get; set; } = TimeSpan.FromMilliseconds(100);
    public int RandomizerSeed { get; set; } = 0;
    public double MaximumStockChangePercent { get; set; } = 0.05;
    public int NumberOfStocks { get; set; } = 100;
    public bool UpdateStocksSequentially { get; set; } = true;
}