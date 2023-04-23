using Serilog;
using Stockr.API;
using Stockr.API.Hubs;
using Stockr.API.Interface;
using Stockr.API.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IStockService, BogusStockService>();
builder.Services.AddHostedService<StockUpdatingBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<StockHub>("/Stock/Hub");

app.Run();