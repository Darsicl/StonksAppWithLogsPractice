using System;
using System.Collections.Generic;
using System.Text;

namespace StonksAppWithLogs.Core.ServiceContracts
{
    public interface IFinnhubService
    {
        Dictionary<string, object>? GetCompanyProfile(string stockSymbol);

        Dictionary<string, object>? GetStockPriceQuote(string stockSymbol);

        Task<List<Dictionary<string, string>>?> GetStocks();

        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }

}
