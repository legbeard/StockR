using Microsoft.AspNetCore.Mvc;
using Stockr.API.Interface;
using Stockr.API.Model;

namespace Stockr.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetCurrentStocks()
    {
        return Ok(await _stockService.GetCurrentStocks());
    }
    
    [HttpGet("{symbol}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stock))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStock(string symbol)
    {
        var stock = await _stockService.GetStock(symbol);
        return stock != null ? Ok(stock) : NotFound();
    }

    [HttpGet("{symbol}/history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Stock>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistoricStockValues(string symbol)
    {
        var historicValues = await _stockService.GetHistoricStockValues(symbol);
        return historicValues != null && historicValues.Any() ? Ok(historicValues) : NotFound();
    }

}