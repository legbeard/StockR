using Core.Configuration;
using Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

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
    })
    .RunConsoleAsync();