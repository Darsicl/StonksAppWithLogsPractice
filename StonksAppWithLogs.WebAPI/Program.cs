using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StonksAppWithLogs.Infrastructure.DbContexts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.Run();
