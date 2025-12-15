using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StonksAppWithLogs.Core.DTO;
using StonksAppWithLogs.Core.ServiceContracts;
using StonksAppWithLogs.WebAPI.Controllers;
using Xunit;

namespace Tests.ControllerTests
{
    public class TradeControllerTest
    {
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<ILogger<TradeController>> _loggerMock;
        private readonly Fixture _fixture;
        private readonly TradeController _controller;
        private readonly Mock<IStocksService> _stocksServiceMock;

        public TradeControllerTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _stocksServiceMock = new Mock<IStocksService>();
            _loggerMock = new Mock<ILogger<TradeController>>();

            _controller = new TradeController(
        _finnhubServiceMock.Object,
        _stocksServiceMock.Object,
        _loggerMock.Object
        );
        }

        [Fact]
        public async Task GetCompanyProfile_CorrectSymbol()
        {
            //Arrange
            string stockSymbol = "AAPL";
            var expectedProfile = _fixture.Create<Dictionary<string, object>>();

            _finnhubServiceMock
                .Setup(service => service.GetCompanyProfile(stockSymbol))
                .ReturnsAsync(expectedProfile);

            //Act
            var result = await _controller.GetCompanyProfile(stockSymbol);

            //Assert
            var OkResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(OkResult.Value);
            Assert.Equal(StatusCodes.Status200OK, OkResult.StatusCode);
            Assert.Equal(expectedProfile, OkResult.Value);

            _finnhubServiceMock
                .Verify(
                    s => s.GetCompanyProfile(stockSymbol),
                    Times.Once
                );
        }

        [Fact]
        public async Task GetCompanyProfile_SymbolIsNull_ReturnsBadRequest()
        {
            //Arrange
            string stockSymbol = null!;

            //Act
            var result = await _controller.GetCompanyProfile(stockSymbol);

            //Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);


            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
            Assert.NotNull(badResult.Value);

            _finnhubServiceMock.Verify(s => s.GetCompanyProfile(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetCompanyProfile_SymbolNotFound_ReturnsNotFound()
        {
            //Arrange
            string stockSymbol = "XXX";

            _finnhubServiceMock
         .Setup(s => s.GetCompanyProfile(stockSymbol))
         .ReturnsAsync((Dictionary<string, object>?)null);

            //Act
            var result = await _controller.GetCompanyProfile(stockSymbol);

            //Assert

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

            _finnhubServiceMock.Verify(s => s.GetCompanyProfile(stockSymbol), Times.Once);
        }

        [Fact]
        public async Task CreateBuyOrder_ValidModel_SuccessResult()
        {
            //Arrange
            var buyOrder = _fixture.Create<BuyOrderRequest>();
            var expectedBuyOrder = _fixture.Create<BuyOrderResponse>();

            _stocksServiceMock
                .Setup(s => s.CreateBuyOrder(buyOrder))
                .ReturnsAsync(expectedBuyOrder);

            //Act
            var result = await _controller.CreateBuyOrder(buyOrder);

            //Assert 
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(expectedBuyOrder, createdResult.Value);
            _stocksServiceMock
                .Verify(
                    s => s.CreateBuyOrder(buyOrder),
                    Times.Once
                );
        }

        [Fact]
        public async Task CreateBuyOrder_UnValidModel_BadRequestResponse()
        {
            //Arrange
            var buyOrder = _fixture.Create<BuyOrderRequest>();

            buyOrder.Quantity = 0;

            _controller.ModelState.AddModelError("Quantity", "The Quantity must be greater than zero.");

            //Act
            var result = await _controller.CreateBuyOrder(buyOrder);

            //Assert 
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            _stocksServiceMock
                .Verify(
                    s => s.CreateBuyOrder(buyOrder),
                    Times.Never
                );
        }

        [Fact]
        public async Task CreateSellOrder_ValidModel_SuccessResult()
        {
            //Arrange
            var sellOrder = _fixture.Create<SellOrderRequest>();
            var expectedSellOrder = _fixture.Create<SellOrderResponse>();

            _stocksServiceMock
                .Setup(s => s.CreateSellOrder(sellOrder))
                .ReturnsAsync(expectedSellOrder);

            //Act
            var result = await _controller.CreateSellOrder(sellOrder);

            //Assert 
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(expectedSellOrder, createdResult.Value);
            _stocksServiceMock
                .Verify(
                    s => s.CreateSellOrder(sellOrder),
                    Times.Once
                );
        }

        [Fact]
        public async Task CreateSellOrder_UnValidModel_BadRequestResponse()
        {
            //Arrange
            var sellOrder = _fixture.Create<SellOrderRequest>();

            sellOrder.Quantity = 0;

            _controller.ModelState.AddModelError("Quantity", "The Quantity must be greater than zero.");

            //Act
            var result = await _controller.CreateSellOrder(sellOrder);

            //Assert 
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            _stocksServiceMock
                .Verify(
                    s => s.CreateSellOrder(sellOrder),
                    Times.Never
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetCompanyProfile_EmptyOrWhitespaceSymbol_ReturnsBadRequest(string stockSymbol)
        {
            // Act
            var result = await _controller.GetCompanyProfile(stockSymbol);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            _finnhubServiceMock.Verify(
                s => s.GetCompanyProfile(It.IsAny<string>()),
                Times.Never
            );
        }



        [Fact]
        public async Task GetBuyOrders_ReturnsOk_WithOrders()
        {
            // Arrange
            var buyOrders = _fixture.Create<List<BuyOrderResponse>>();

            _stocksServiceMock
                .Setup(s => s.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            // Act
            var result = await _controller.GetBuyOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var value = Assert.IsAssignableFrom<IEnumerable<BuyOrderResponse>>(okResult.Value);
            Assert.Equal(buyOrders, value);

            _stocksServiceMock.Verify(s => s.GetBuyOrders(), Times.Once);
        }


        [Fact]
        public async Task GetBuyOrders_EmptyList_ReturnsOk()
        {
            // Arrange
            var emptyList = new List<BuyOrderResponse>();

            _stocksServiceMock
                .Setup(s => s.GetBuyOrders())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetBuyOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var value = Assert.IsAssignableFrom<IEnumerable<BuyOrderResponse>>(okResult.Value);
            Assert.Empty(value);

            _stocksServiceMock.Verify(s => s.GetBuyOrders(), Times.Once);
        }


        [Fact]
        public async Task GetSellOrders_ReturnsOk_WithOrders()
        {
            // Arrange
            var sellOrders = _fixture.Create<List<SellOrderResponse>>();

            _stocksServiceMock
                .Setup(s => s.GetSellOrders())
                .ReturnsAsync(sellOrders);

            // Act
            var result = await _controller.GetSellOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var value = Assert.IsAssignableFrom<IEnumerable<SellOrderResponse>>(okResult.Value);
            Assert.Equal(sellOrders, value);

            _stocksServiceMock.Verify(s => s.GetSellOrders(), Times.Once);
        }


        [Fact]
        public async Task GetSellOrders_EmptyList_ReturnsOk()
        {
            // Arrange
            var emptyList = new List<SellOrderResponse>();

            _stocksServiceMock
                .Setup(s => s.GetSellOrders())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetSellOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var value = Assert.IsAssignableFrom<IEnumerable<SellOrderResponse>>(okResult.Value);
            Assert.Empty(value);

            _stocksServiceMock.Verify(s => s.GetSellOrders(), Times.Once);
        }


        [Fact]
        public async Task GetAllOrders_ReturnsOk_WithAggregatedOrders()
        {
            // Arrange
            var buyOrders = _fixture.Create<List<BuyOrderResponse>>();
            var sellOrders = _fixture.Create<List<SellOrderResponse>>();

            _stocksServiceMock
                .Setup(s => s.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            _stocksServiceMock
                .Setup(s => s.GetSellOrders())
                .ReturnsAsync(sellOrders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var orders = Assert.IsType<Orders>(okResult.Value);
            Assert.Equal(buyOrders, orders.BuyOrders);
            Assert.Equal(sellOrders, orders.SellOrders);

            _stocksServiceMock.Verify(s => s.GetBuyOrders(), Times.Once);
            _stocksServiceMock.Verify(s => s.GetSellOrders(), Times.Once);
        }


        [Fact]
        public async Task GetAllOrders_EmptyLists_ReturnsOk()
        {
            // Arrange
            _stocksServiceMock
                .Setup(s => s.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrderResponse>());

            _stocksServiceMock
                .Setup(s => s.GetSellOrders())
                .ReturnsAsync(new List<SellOrderResponse>());

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var orders = Assert.IsType<Orders>(okResult.Value);
            Assert.Empty(orders.BuyOrders);
            Assert.Empty(orders.SellOrders);

            _stocksServiceMock.Verify(s => s.GetBuyOrders(), Times.Once);
            _stocksServiceMock.Verify(s => s.GetSellOrders(), Times.Once);
        }


    }
}
