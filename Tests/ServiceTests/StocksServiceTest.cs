using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using StonksAppWithLogs.Core.DTO;
using StonksAppWithLogs.Core.Domain.Entities;
using StonksAppWithLogs.Core.Domain.RepositoryContracts;
using StonksAppWithLogs.Core.Services;
using Xunit;

namespace Tests.ServiceTests
{
    public class StocksServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly StocksService _service;

        public StocksServiceTest()
        {
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _service = new StocksService(_stocksRepositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateBuyOrder_NullRequest_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateBuyOrder(null));
            _stocksRepositoryMock.Verify(r => r.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateBuyOrder_ValidRequest_CreatesAndReturnsResponse()
        {
            var request = _fixture.Create<BuyOrderRequest>();

            // Capture the BuyOrder passed to repository and return it
            _stocksRepositoryMock
                .Setup(r => r.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync((BuyOrder bo) => bo)
                .Verifiable();

            var result = await _service.CreateBuyOrder(request);

            Assert.NotNull(result);
            Assert.Equal(request.StockSymbol, result.StockSymbol);
            Assert.Equal(request.StockName, result.StockName);
            Assert.Equal(request.DateAndTimeOfOrder, result.DateAndTimeOfOrder);
            Assert.Equal(request.Quantity, result.Quantity);
            Assert.Equal(request.Price, result.Price);
            Assert.NotEqual(Guid.Empty, result.BuyOrderID);

            _stocksRepositoryMock.Verify(r => r.CreateBuyOrder(It.Is<BuyOrder>(b =>
                b.StockSymbol == request.StockSymbol &&
                b.StockName == request.StockName &&
                b.Quantity == request.Quantity &&
                b.Price == request.Price &&
                b.BuyOrderId != Guid.Empty
            )), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateSellOrder_NullRequest_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateSellOrder(null));
            _stocksRepositoryMock.Verify(r => r.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateSellOrder_ValidRequest_CreatesAndReturnsResponse()
        {
            var request = _fixture.Create<SellOrderRequest>();

            _stocksRepositoryMock
                .Setup(r => r.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync((SellOrder so) => so)
                .Verifiable();

            var result = await _service.CreateSellOrder(request);

            Assert.NotNull(result);
            Assert.Equal(request.StockSymbol, result.StockSymbol);
            Assert.Equal(request.StockName, result.StockName);
            Assert.Equal(request.DateAndTimeOfOrder, result.DateAndTimeOfOrder);
            Assert.Equal(request.Quantity, result.Quantity);
            Assert.Equal(request.Price, result.Price);
            Assert.NotEqual(Guid.Empty, result.SellOrderID);

            _stocksRepositoryMock.Verify(r => r.CreateSellOrder(It.Is<SellOrder>(s =>
                s.StockSymbol == request.StockSymbol &&
                s.StockName == request.StockName &&
                s.Quantity == request.Quantity &&
                s.Price == request.Price &&
                s.SellOrderId != Guid.Empty
            )), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetBuyOrders_ReturnsMappedResponses()
        {
            var buyEntities = _fixture.CreateMany<BuyOrder>(3).ToList();
            // ensure BuyOrderId and Date are set
            for (int i = 0; i < buyEntities.Count; i++)
            {
                buyEntities[i].BuyOrderId = Guid.NewGuid();
                buyEntities[i].DateAndTimeOfOrder = DateTime.UtcNow.AddMinutes(-i);
            }

            _stocksRepositoryMock
                .Setup(r => r.GetBuyOrders())
                .ReturnsAsync(buyEntities);

            var results = await _service.GetBuyOrders();

            Assert.Equal(buyEntities.Count, results.Count);

            for (int i = 0; i < buyEntities.Count; i++)
            {
                Assert.Equal(buyEntities[i].BuyOrderId, results[i].BuyOrderID);
                Assert.Equal(buyEntities[i].StockSymbol, results[i].StockSymbol);
                Assert.Equal(buyEntities[i].StockName, results[i].StockName);
                Assert.Equal(buyEntities[i].DateAndTimeOfOrder, results[i].DateAndTimeOfOrder);
                Assert.Equal(buyEntities[i].Quantity, results[i].Quantity);
                Assert.Equal(buyEntities[i].Price, results[i].Price);
            }

            _stocksRepositoryMock.Verify(r => r.GetBuyOrders(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetBuyOrders_EmptyList_ReturnsEmpty()
        {
            _stocksRepositoryMock
                .Setup(r => r.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());

            var results = await _service.GetBuyOrders();

            Assert.NotNull(results);
            Assert.Empty(results);
            _stocksRepositoryMock.Verify(r => r.GetBuyOrders(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetSellOrders_ReturnsMappedResponses()
        {
            var sellEntities = _fixture.CreateMany<SellOrder>(3).ToList();
            for (int i = 0; i < sellEntities.Count; i++)
            {
                sellEntities[i].SellOrderId = Guid.NewGuid();
                sellEntities[i].DateAndTimeOfOrder = DateTime.UtcNow.AddMinutes(-i);
            }

            _stocksRepositoryMock
                .Setup(r => r.GetSellOrders())
                .ReturnsAsync(sellEntities);

            var results = await _service.GetSellOrders();

            Assert.Equal(sellEntities.Count, results.Count);

            for (int i = 0; i < sellEntities.Count; i++)
            {
                Assert.Equal(sellEntities[i].SellOrderId, results[i].SellOrderID);
                Assert.Equal(sellEntities[i].StockSymbol, results[i].StockSymbol);
                Assert.Equal(sellEntities[i].StockName, results[i].StockName);
                Assert.Equal(sellEntities[i].DateAndTimeOfOrder, results[i].DateAndTimeOfOrder);
                Assert.Equal(sellEntities[i].Quantity, results[i].Quantity);
                Assert.Equal(sellEntities[i].Price, results[i].Price);
            }

            _stocksRepositoryMock.Verify(r => r.GetSellOrders(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetSellOrders_EmptyList_ReturnsEmpty()
        {
            _stocksRepositoryMock
                .Setup(r => r.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());

            var results = await _service.GetSellOrders();

            Assert.NotNull(results);
            Assert.Empty(results);
            _stocksRepositoryMock.Verify(r => r.GetSellOrders(), Times.Once);
        }
    }
}
