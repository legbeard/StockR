namespace Core.Configuration;

public class ClusterConfiguration
{
    public const string Position = "Cluster";
    public Uri ConsulUri { get; set; }
}