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

        public TradeController(IFinnhubService finnhubService, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }

        [HttpGet("index/{stockSymbol}")]

        public async Task<IActionResult> GetCompanyProfile(string stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
            {
                ModelState.AddModelError("stockSymbol", "Stock symbol cannot be empty.");
                return BadRequest(ModelState);
            }

            var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);

            if (companyProfile == null)
            {
                return NotFound($"Stonk with symbol '{stockSymbol}' not found.");
            }

            return Ok(companyProfile);
        }

        //GET    /api/trade/orderspdf → генерирует и отдаёт PDF-файл со всеми ордерами(любой PDF-генератор, например QuestPDF или DinkToPdf)
        
        [HttpPost("buyorder")]
        public async Task<IActionResult> CreateBuyOrder([FromBody] BuyOrderRequest buyOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBuyOrder = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return Created();
        }

        [HttpPost("sellorder")]
        public async Task<IActionResult> CreateSellOrder([FromBody] SellOrderRequest sellOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSellOrder = await _stocksService.CreateSellOrder(sellOrderRequest);

            return Created();
        }

        [HttpGet("buyorderslist")]
        public async Task<IActionResult> GetBuyOrders()
        {
            var buyOrders = await _stocksService.GetBuyOrders();

            return Ok(buyOrders);
        }

        [HttpGet("sellorderslist")]
        public async Task<IActionResult> GetSellOrders()
        {
            var sellOrders = await _stocksService.GetSellOrders();

            return Ok(sellOrders);
        }

    }
}
