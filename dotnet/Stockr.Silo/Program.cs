// See https://aka.ms/new-console-template for more information

using Core.Configuration;
using Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Stockr.Silo;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var clusterConfiguration = new ClusterConfiguration();
configuration.GetSection(ClusterConfiguration.Position).Bind(clusterConfiguration);

await Host.CreateDefaultBuilder()
    .UseSerilog((context, logging) =>
    {
        logging
            .WriteTo.Console()
            .MinimumLevel.Information();
    })
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.AddConsulClustering(clusterConfiguration);
        siloBuilder.AddMemoryGrainStorage("stocks");
        siloBuilder.ConfigureServices(services =>
        {
            services.AddHostedService<StockUpdatingBackgroundService>();
        });
    })
    .RunConsoleAsync();