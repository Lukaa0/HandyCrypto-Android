using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using CryptoCompare;
using HandyCrypto.Adapters;
using HandyCrypto.DataHelper;
using HandyCrypto.Extensions;
using HandyCrypto.Fragments;
using HandyCrypto.Model;
using PortableCryptoLibrary;
using PortableCryptoServices;

namespace HandyCrypto.Activities
{
    [Activity(Label = "Wallet")]
    public partial class WalletActivity : AppCompatActivity

    {
        RecyclerView recyclerView;
        private List<WalletModel> walletModels;
        public List<Wallet> wallets { get; private set; }
        LinearLayoutManager layoutManager;
        WalletRecyclerViewAdapter coinRecyclerViewAdapter;
        public FrameLayout mainContainer;
        private Android.Support.V7.Widget.Toolbar toolbar;
        private List<CoinInfo> coins;
        private WalletService walletService;





        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ThemeValidator.ValidateTheme(this);
            SetContentView(Resource.Layout.wallet_main_layout);
            coins = new List<CoinInfo>();
            walletService = new WalletService();
            coins = await LocalDbService<CoinInfo>.Instance.SelectItems();
            wallets = await DbInitAsync();


            layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.walletRecyclerView);
            mainContainer = FindViewById<FrameLayout>(Resource.Id.wallet_frame);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.walletToolbar);
            SetSupportActionBar(toolbar);

            walletModels = new List<WalletModel>();
            LoadItems();
            SetAdapter(recyclerView);
            await CalculateAllAsync();
            //recyclerView.AddOnScrollListener(ScrollListener);




        }

        private async Task CalculateAllAsync()
        {
            try
            {
                using (WalletService service = new WalletService())
                {
                    List<Task> tasks = new List<Task>();
                    tasks.Add(CalculateBudgetAsync());
                    tasks.Add(CalculateProfitAllAsync(service.CalculateProfitAll,"All"));
                    tasks.Add(CalculateProfitAllAsync(service.CalculateProfitHour,"Hour"));
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                new NoConnectionFragment().Run(Resource.Id.wallet_frame, this);

            }
        }

        //private async Task UpdateHistoricalData(WalletModel item)
        //{
        //    var date = DateTime.Parse(item.Wallet.InvestDate,
        //        CultureInfo.InvariantCulture);
        //    var histoData = await walletService.CalculateHistorical(date, "USD", item.Info.Symbol,
        //        item.Wallet.Investment.ToDecimalWithCulture(), item.Wallet.CoinPrice);
        //    coinRecyclerViewAdapter.AddChartData(histoData, item.Wallet.Id);
        //}
        private async Task UpdateBudget(WalletModel item)
        {

            var date = DateTime.Parse(item.Wallet.InvestDate, CultureInfo.InvariantCulture);
            var result = await walletService.CalculateCurrent(date, item.Wallet, "USD",
                item.Info.Symbol,
                item.Wallet.Investment);
            coinRecyclerViewAdapter.AddBudgetValue(result, item.Wallet.Id);
        }

        private async Task CalculateBudgetAsync()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                if (walletModels != null)
                {
                    for (int i = 0; i < walletModels.Count; i++)
                    {
                        var task = this.UpdateBudget(walletModels[i]);
                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks);


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task UpdateProfitAll(Func<DateTime,string,Task<decimal>> func, WalletModel item,string tag)
        {
            
                var profitAll = await func(item.Wallet.InvestDate.ToDateTime(), item.Info.Symbol);
            
                coinRecyclerViewAdapter.AddProfitAll(profitAll, item.Wallet.Id,tag);
            
        }

        private async Task CalculateProfitAllAsync(Func<DateTime, string, Task<decimal>> func, string tag)
        {
            try
            {
                List<Task> tasks = new List<Task>();
                if (walletModels != null)
                {
                    for (int i = 0; i < walletModels.Count; i++)
                    {
                        var task = this.UpdateProfitAll(func,walletModels[i],tag);
                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks);


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_wallet, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add_wallet_from_wallet:
                    {
                        WalletDialogFragment dialogFragment = new WalletDialogFragment();
                        dialogFragment.Show(SupportFragmentManager.BeginTransaction(), "wallet-dialog-fragment");
                    }
                    break;
                
            }
            return base.OnOptionsItemSelected(item);
        }


        //private async Task CalculateChartDataAsync()
        //{
        //    try
        //    {

        //        if (walletModels != null)
        //        {
        //            List<Task> tasks = new List<Task>();
        //            for (int i = 0; i < walletModels.Count; i++)
        //            {
        //                tasks.Add(this.UpdateHistoricalData(walletModels[i]));
        //            }

        //            await Task.WhenAll(tasks);
        //        }





        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}



        private async void SListener_LoadMoreEvent(object sender, EventArgs e, int position)
        {
            //coinRecyclerViewAdapter.AddAndRefresh(cryptoModel);

        }
        private void LoadItems()
        {
            foreach (var item in wallets)
            {
                var coinItem = coins.FirstOrDefault(x => x.Symbol == item.Symbol);
                if (coinItem != null)
                {
                    WalletModel model = new WalletModel();
                    model.Info = coinItem;
                    model.Wallet = item;


                    walletModels.Add(model);
                }

            }
        }

        private void SetAdapter(RecyclerView recycler)
        {
            var context = recycler.Context;
            recycler.SetLayoutManager(layoutManager);
            SnapHelper snap = new LinearSnapHelper();

            snap.AttachToRecyclerView(recycler);

            coinRecyclerViewAdapter = new WalletRecyclerViewAdapter(walletModels, this, recyclerView);
            recycler.SetAdapter(coinRecyclerViewAdapter);
        }



        private async Task<List<Wallet>> DbInitAsync()
        {
            var myWallet = await LocalDbService<Wallet>.Instance.SelectItems();
            return myWallet;
        }




    }
}