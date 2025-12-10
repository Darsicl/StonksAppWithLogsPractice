using Microsoft.EntityFrameworkCore;
using StonksAppWithLogs.Core.Domain.Entities;
using StonksAppWithLogs.Core.Domain.RepositoryContracts;
using StonksAppWithLogs.Infrastructure.DbContexts;

namespace StonksAppWithLogs.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {

        private readonly StockMarketDbContext _context;
        public StocksRepository(StockMarketDbContext context)
        {
            _context = context;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _context.BuyOrders.Add(buyOrder);
            await _context.SaveChangesAsync();

            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _context.SellOrders.Add(sellOrder);
            await _context.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _context.BuyOrders
                .AsNoTracking()
                .OrderByDescending(r => r.DateAndTimeOfOrder)
                .ToListAsync();

            return buyOrders;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _context.SellOrders
                    .AsNoTracking()
                    .OrderByDescending(r => r.DateAndTimeOfOrder)
                    .ToListAsync();

            return sellOrders;
        }
    }
}
