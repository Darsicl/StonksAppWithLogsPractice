using Microsoft.EntityFrameworkCore;
using StonksAppWithLogs.Core.Domain.Entities;

namespace StonksAppWithLogs.Infrastructure.DbContexts
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options)
        : base(options)
        {
        }


        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
