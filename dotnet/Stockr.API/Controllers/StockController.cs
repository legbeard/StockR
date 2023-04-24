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
    public async Task<IActionResult> GetStocks()
    {
        return Ok(await _stockService.GetStocks());
    }
    
    [HttpGet("{symbol}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stock))]
    public async Task<IActionResult> GetStock(string symbol)
    {
        var stock = await _stockService.GetStock(symbol);
        return stock != null ? Ok(stock) : NotFound();
    }

}