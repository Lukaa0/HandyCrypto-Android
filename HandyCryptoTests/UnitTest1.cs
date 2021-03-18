using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoCompare;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using PortableCryptoLibrary;
using PortableCryptoServices;
using Assert = NUnit.Framework.Assert;

namespace HandyCryptoTests
{
    [TestClass]
    public class UnitTest1
    {
        public delegate Task<List<CandleData>> HistoricalDataDelegate(
           string symbol,
           string currency,
           int? limit,
           bool? allData = null,
           DateTimeOffset? toDate = null,
           string exchangeName = "CCCCAG",
           int? aggregate = null,
           bool? tryConvention = null
       );
        [TestMethod]
        public void DelegateTest()
        {
            HistoricalDataDelegate del = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalDayPrices;
            HistoricalDataDelegate del1 = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalHourPrices;
            HistoricalDataDelegate del2 = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalMinutePrices;

            Assert.IsTrue(del.Method.Name.Contains("Day"));
            Assert.IsTrue(del1.Method.Name.Contains("Hour"));
            Assert.IsTrue(del2.Method.Name.Contains("Minute"));


        }
        [TestMethod]
        public async Task TestMethod1()
        {
            var models = await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(0, 150);
            models.AddRange(await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(150, 150));
            models.AddRange(await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(300, 150));
            var modelss = models.OrderBy(x => x.Info.SortOrder).ToList();
            for (int i = 0; i < modelss.Count; i++)
            {
                if (modelss[i].Info.SortOrder == modelss[i + 1].Info.SortOrder)
                {
                    var st = modelss[i].Info;
                }
            }


                var duplicates = models.GroupBy(x => x.Info.Symbol).Where(x => x.Count() > 1).Select(x => x.Key);
            foreach (var i in duplicates)
                Console.WriteLine(i);




        }
        [TestMethod]
        public async Task TestMethod2()
        {
            var data = await PredictionService.Predict();
            Console.WriteLine(data);
        }

    }
}
