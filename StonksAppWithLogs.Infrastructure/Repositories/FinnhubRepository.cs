using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StonksAppWithLogs.Core.Domain.Options;
using StonksAppWithLogs.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace StonksAppWithLogs.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IOptions<FinnhubOptions> _finnhubOptions;
        private readonly IHttpClientFactory _factory;

        public FinnhubRepository(IOptions<FinnhubOptions> finnhubOptions, IHttpClientFactory factory)
        {
            _finnhubOptions = finnhubOptions;
            _factory = factory;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
           using var client = _factory.CreateClient();

            Dictionary<string, object>? companyProfile = await client
                .GetFromJsonAsync<Dictionary<string, object>?>($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_finnhubOptions.Value.Token}");


            if (companyProfile == null)
                throw new InvalidOperationException("No response from server");

            if (companyProfile.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(companyProfile["error"]));

            return companyProfile;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using var client = _factory.CreateClient();

            Dictionary<string, object>? stockPriceQuote = await client
                .GetFromJsonAsync<Dictionary<string, object>?>($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_finnhubOptions.Value.Token}");


            if (stockPriceQuote == null)
                throw new InvalidOperationException("No response from server");

            if (stockPriceQuote.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(stockPriceQuote["error"]));

            return stockPriceQuote;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            using var client = _factory.CreateClient();

            List<Dictionary<string, string>>? stocks = await client
                .GetFromJsonAsync<List<Dictionary<string, string>>?>($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_finnhubOptions.Value.Token}");


            if (stocks == null)
                throw new InvalidOperationException("No response from server");

            return stocks;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            using var client = _factory.CreateClient();

            Dictionary<string, object>? stocks = await client
                .GetFromJsonAsync<Dictionary<string, object>?>($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_finnhubOptions.Value.Token}");


            if (stocks == null)
                throw new InvalidOperationException("No response from server");

            if (stocks.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(stocks["error"]));

            return stocks;
        }
    }
}
