using StonksAppWithLogs.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StonksAppWithLogs.Core.Domain.RepositoryContracts
{
    public interface IFinnhubRepository
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        Task<List<Dictionary<string, string>>?> GetStocks();

        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }

    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        Task<List<BuyOrder>> GetBuyOrders();

        Task<List<SellOrder>> GetSellOrders();
    }
}
