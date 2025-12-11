using StonksAppWithLogs.Core.Domain.RepositoryContracts;
using StonksAppWithLogs.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StonksAppWithLogs.Core.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
                throw new ArgumentException("Stock symbol cannot be null or empty.", nameof(stockSymbol));

            Dictionary<string, object>? companyProfile = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            
            return companyProfile;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
                throw new ArgumentException("Stock symbol cannot be null or empty.", nameof(stockSymbol));

            Dictionary<string, object>? stock = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            return stock;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
           
            List<Dictionary<string, string>>? stock = await _finnhubRepository.GetStocks();

            return stock;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            if (string.IsNullOrWhiteSpace(stockSymbolToSearch))
                throw new ArgumentException("Stock symbol cannot be null or empty.", nameof(stockSymbolToSearch));

            Dictionary<string, object>? stock = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            return stock;
        }
    }
}
