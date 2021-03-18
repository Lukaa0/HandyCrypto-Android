using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CryptoCompare;
using Google.Flexbox;
using HandyCrypto.Fragments;
using HandyCrypto.Model.Interfaces;
using PortableCryptoLibrary;
using PortableCryptoServices;

namespace HandyCrypto.Model
{
    public class WalletService : IWalletService, IDisposable
    {
        public async Task<decimal> CalculateCurrent(DateTime date, Wallet cryptoItem, string currency, string symbol, decimal invest)
        {
            try
            {
                
                var histoPriceTask = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalPrice(symbol, new[] { currency }, date);
                var currentPriceTask = HandyCryptoClient.Instance.GeneralCoinInfo.GetPrice(symbol, currency);
                await Task.WhenAll(histoPriceTask, currentPriceTask);
                var item = await histoPriceTask;
                var currentPrice = await currentPriceTask;
                decimal budget = 0;
                decimal cryptoValue = invest / item;
                budget = (cryptoValue * currentPrice);
                var result = Math.Round(budget, 6);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<decimal> CalculateProfitAll(DateTime investDate,string symbol)
        {
            var currentPrice = await HandyCryptoClient.Instance.GeneralCoinInfo.GetPrice(symbol, "USD");
            var investDatePrice = await HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalPrice(symbol, new[] { "USD" }, investDate);
            if (investDatePrice == 0)
                return 0;
            return (((investDatePrice - currentPrice) / investDatePrice) * 100)*-1;
        }
        public async Task<decimal> CalculateProfitHour(DateTime investDate,string symbol)
        {
            var currentPrice = await HandyCryptoClient.Instance.GeneralCoinInfo.GetPrice(symbol, "USD");
            var oneHourBefore = DateTime.Now.AddHours(-1);
            if (oneHourBefore < investDate)
                return 0;
            var investDatePrice = await HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalPrice(symbol, new[] { "USD" },oneHourBefore );
            if (investDatePrice == 0)
                return 0;
            return ((investDatePrice - currentPrice) / investDatePrice) * 100;
        }
        public async Task<ObservableCollection<HistoricalDataModel>> CalculateHistorical(HistoricalDataDelegate timeSpecifier, string currency,
            string symbol, decimal invest, decimal originalPrice,DateTime investDate, int? limit, bool? allData = null, DateTime? toDate = null, bool?
            tryConvention = null, int? aggregate = null, string exchangeName = "CCCAGG")
        {

            var now = DateTime.Now;

           

            ObservableCollection<HistoricalDataModel> historicalPrices = new ObservableCollection<HistoricalDataModel>();
            var data = await timeSpecifier(symbol, currency, limit, allData, toDate, exchangeName, aggregate, tryConvention);
            if (data == null)
                return null;
            var orderedData = data.OrderBy(x => x.Time);


            foreach (var item in orderedData)
            {
                if (investDate <= item.Time.DateTime)
                {
                    if (item.Close == 0 || originalPrice == 0)
                        continue;
                    var price = (item.Close / originalPrice) * invest;
                    var model = new HistoricalDataModel(item.Time.DateTime, price);
                    historicalPrices.Add(model);
                }

            }
            return historicalPrices;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            
        }
        private DateTime GetDate(HistoricalDataDelegate dataDelegate, int limit)
        {
            switch (dataDelegate.Method.Name)
            {
                case string day when day.Contains("Day"):
                    {
                        return DateTime.Now.AddDays(-limit);
                    }
                case string hour when hour.Contains("Hour"):
                    {
                        return DateTime.Now.AddHours(-limit);
                    }
                case string minute when minute.Contains("Minute"):
                    {
                        return DateTime.Now.AddMinutes(-limit);
                    }
                default:
                    return DateTime.Now;
            }


        }
    }
}