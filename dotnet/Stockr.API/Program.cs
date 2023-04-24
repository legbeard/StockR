using Core.Configuration;
using Core.Interfaces.Observers;
using Core.Utilities;
using Serilog;
using Stockr.API.Hubs;
using Stockr.API.Interface;
using Stockr.API.Services;

var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var clusterConfiguration = new ClusterConfiguration();
configuration.GetSection(ClusterConfiguration.Position).Bind(clusterConfiguration);

var builder = WebApplication.CreateBuilder(args);

var logger =  new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddScoped<IStockService, OrleansStockService>();
builder.Services.AddSingleton<ICurrentStockObserver, CurrentStockObserver>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.AddConsulClientClustering(clusterConfiguration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder =>
{
    policyBuilder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyMethod();
});

app.UseAuthorization();

app.MapControllers();
app.MapHub<StockHub>("/Stock/Hub");

app.Run();