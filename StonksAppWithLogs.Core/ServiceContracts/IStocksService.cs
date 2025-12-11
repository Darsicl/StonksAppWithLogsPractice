using StonksAppWithLogs.Core.Domain.Entities;

namespace StonksAppWithLogs.Core.ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        Task<List<BuyOrder>> GetBuyOrders();

        Task<List<SellOrder>> GetSellOrders();
    }

}
