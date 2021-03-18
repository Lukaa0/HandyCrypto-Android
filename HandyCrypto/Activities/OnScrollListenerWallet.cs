using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Android.Views;
using HandyCrypto.Adapters;
using HandyCrypto.Model;
using PortableCryptoServices;

namespace HandyCrypto.Activities
{
    public partial class WalletActivity
    {
        public class OnScrollListenerWallet : RecyclerView.OnScrollListener
        {
            LinearLayoutManager ln;
            private SemaphoreSlim chartLocker;
            WalletRecyclerViewAdapter adapter;
            WalletService walletService;

            public OnScrollListenerWallet(LinearLayoutManager lln, WalletRecyclerViewAdapter adapter)
            {
                walletService = new WalletService();
                ln = lln;
                this.adapter = adapter;
                chartLocker = new SemaphoreSlim(1, 1);

            }

            public override async void OnScrollStateChanged(RecyclerView recyclerView, int newState)
            {
                base.OnScrollStateChanged(recyclerView, newState);
                if (newState == RecyclerView.ScrollStateIdle)
                {
                    try
                    {
                        var position = ln.FindFirstVisibleItemPosition();
                        var item = adapter.Coins[position];
                        List<Task> tasks = new List<Task>();
                        if (item.CurrentBudget <= 0m)
                        tasks.Add(CalculateBudgetAsync( adapter, item,position));
                        if(item.HistoricalData==null|| item.HistoricalData.Count <= 0)
                        tasks.Add(CalculateChartDataAsync(adapter, position, item));
                        if(tasks.Count>0)
                        await Task.WhenAll(tasks);





                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }




            private async Task CalculateBudgetAsync(WalletRecyclerViewAdapter adapter,WalletModel item,int position)
            {
                try
                {

                    if (item.CurrentBudget == 0m)
                    {
                        await chartLocker.WaitAsync();
                        var date = DateTime.Parse(item.Wallet.InvestDate, CultureInfo.InvariantCulture);
                        var result = await walletService.CalculateCurrent(date, item.Wallet, "USD",
                            item.Info.Symbol,
                            item.Wallet.Investment);
                        adapter.Coins[position].CurrentBudget = result;
                        adapter.NotifyItemChanged(position);
                    }


                }
                finally
                {
                    chartLocker.Release();
                }
            }

            private async Task CalculateChartDataAsync(WalletRecyclerViewAdapter adapter,int position,WalletModel item)
            {
                try
                {

                        
                            var date = DateTime.Parse(item.Wallet.InvestDate,
                                CultureInfo.InvariantCulture);
                           // var histoData = await walletService.CalculateHistorical(date, "USD", item.Info.Symbol,
                               // item.Wallet.Investment.ToDecimalWithCulture(),item.Wallet.CoinPrice);
                   // adapter.Coins[position].HistoricalData = histoData;
                    adapter.NotifyItemChanged(position);





                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }




        }




    }
}