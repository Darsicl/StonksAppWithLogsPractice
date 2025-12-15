using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StonksAppWithLogs.Core.Domain.Entities;
using StonksAppWithLogs.Core.DTO;
using StonksAppWithLogs.Core.ServiceContracts;

namespace StonksAppWithLogs.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _logger = logger;
        }

        [HttpGet("index/{stockSymbol}")]

        public async Task<IActionResult> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogDebug("Request to get company profile with stock symbol {Symbol}", stockSymbol);

            if (string.IsNullOrWhiteSpace(stockSymbol))
            {
                _logger.LogWarning("Client send empty symbol {Symbol}");

                ModelState.AddModelError("stockSymbol", "Stock symbol cannot be empty.");

                return BadRequest(ModelState);
            }

            var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);

            if (companyProfile == null)
            {
                _logger.LogWarning("Stock {Symbol} not found", stockSymbol);

                return NotFound($"Stonk with symbol '{stockSymbol}' not found.");
            }
            _logger.LogInformation("Succesfuly returned company profile {Profile}", companyProfile);

            return Ok(companyProfile);

        }

        //GET    /api/trade/orderspdf → генерирует и отдаёт PDF-файл со всеми ордерами(любой PDF-генератор, например QuestPDF или DinkToPdf)
        
        [HttpPost("buyorder")]
        public async Task<IActionResult> CreateBuyOrder([FromBody] BuyOrderRequest buyOrderRequest)
        {
            _logger.LogDebug("Request to create buy Order {@BuyOrderRequest}", buyOrderRequest);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for BuyOrder. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            var createdBuyOrder = await _stocksService.CreateBuyOrder(buyOrderRequest);

            _logger.LogInformation("Successfully created buy order {@BuyOrderRequest}", buyOrderRequest);

            return Created();
        }

        [HttpPost("sellorder")]
        public async Task<IActionResult> CreateSellOrder([FromBody] SellOrderRequest sellOrderRequest)
        {
            _logger.LogDebug("Request to create buy Order {@SellOrderRequest}", sellOrderRequest);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for SellOrder. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));

                return BadRequest(ModelState);
            }

            var createdSellOrder = await _stocksService.CreateSellOrder(sellOrderRequest);

            _logger.LogInformation("Successfully created buy order {@SellOrderRequest}", sellOrderRequest);

            return Created();
        }

        [HttpGet("buyorderslist")]
        public async Task<IActionResult> GetBuyOrders()
        {
            _logger.LogDebug("Request to get all buy orders ");

            var buyOrders = await _stocksService.GetBuyOrders();

            _logger.LogInformation("Succesfuly returned all buy order");

            return Ok(buyOrders);
        }

        [HttpGet("sellorderslist")]
        public async Task<IActionResult> GetSellOrders()
        {
            _logger.LogDebug("Request to get all sell orders ");

            var sellOrders = await _stocksService.GetSellOrders();

            _logger.LogInformation("Succesfuly returned all sell order");

            return Ok(sellOrders);
        }

    }
}
