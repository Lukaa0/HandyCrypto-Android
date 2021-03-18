using HandyCrypto.Fragments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HandyCrypto.Model.Interfaces
{
    public interface IWalletService
    {
        
         Task<decimal> CalculateCurrent(DateTime date, Wallet cryptoItem, string currency, string symbol, decimal invest);
        Task<decimal> CalculateProfitAll(DateTime investDate, string symbol);
        Task<decimal> CalculateProfitHour(DateTime investDate, string symbol);

         Task<ObservableCollection<HistoricalDataModel>> CalculateHistorical(HistoricalDataDelegate timeSpecifier, string currency,
            string symbol, decimal invest, decimal originalPrice, DateTime investDate,int? limit, bool? allData = null, DateTime? toDate = null, bool?
            tryConvention = null, int? aggregate = null, string exchangeName = "CCCAGG");
    }
}