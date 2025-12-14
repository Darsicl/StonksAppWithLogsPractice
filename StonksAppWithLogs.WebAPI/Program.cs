using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StonksAppWithLogs.Core.Domain.Options;
using StonksAppWithLogs.Core.Domain.RepositoryContracts;
using StonksAppWithLogs.Core.ServiceContracts;
using StonksAppWithLogs.Core.Services;
using StonksAppWithLogs.Infrastructure.DbContexts;
using StonksAppWithLogs.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();

builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen();

builder.Services.Configure<FinnhubOptions>(
    builder.Configuration.GetSection("Finnhub"));

builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
