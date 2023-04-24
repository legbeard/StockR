using Core.Configuration;
using Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Stockr.StockSimulator;

var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var clusterConfiguration = new ClusterConfiguration();
configuration.GetSection(ClusterConfiguration.Position).Bind(clusterConfiguration);

await Host.CreateDefaultBuilder()
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.AddConsulClientClustering(clusterConfiguration);
    })
    .UseSerilog((context, logging) =>
    {
        logging.WriteTo.Console()
            .MinimumLevel.Information();
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<SimulatorConfiguration>(context.Configuration.GetSection(SimulatorConfiguration.Position));
        services.AddHostedService<StockSimulator>();
    }).RunConsoleAsync();