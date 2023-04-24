using Core.Configuration;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace Core.Utilities;

public static class OrleansExtensions
{
    public static ISiloBuilder AddConsulClustering(this ISiloBuilder builder, ClusterConfiguration configuration)
    {
        return builder.UseConsulSiloClustering(options => ConfigureConsulClustering(options, configuration));
    }

    public static IClientBuilder AddConsulClientClustering(this IClientBuilder builder, ClusterConfiguration configuration)
    {
        return builder.UseConsulClientClustering(options => ConfigureConsulClustering(options, configuration));
    }

    private static void ConfigureConsulClustering(ConsulClusteringOptions options, ClusterConfiguration configuration)
    {
        options.ConfigureConsulClient(configuration.ConsulUri);
        options.KvRootFolder = "rootFolder";
    }
}