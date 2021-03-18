using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.Extensions;
using HandyCrypto.Model;
using HandyCrypto.View_Holders;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using PortableCryptoServices;
using Square.Picasso;

namespace HandyCrypto.Fragments
{
    public class WalletDetailFragment : Android.Support.V4.App.Fragment
    {
        WalletModel cryptoItem;
        private WalletItemModel viewModel;
        ImageButton backBtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cryptoItem = JsonConvert.DeserializeObject<WalletModel>(Arguments.GetString("wallet-data"));
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.wallet_layout, container, false);
            viewModel = new WalletItemModel(view);
            backBtn = view.FindViewById<ImageButton>(Resource.Id.wallet_detail_back_btn);
            backBtn.Click += (sender, e) =>
             {
                 Activity.OnBackPressed();
             };
            return view;
        }

        static string FormatNumber(decimal n)
        {
            if (n < 1000)
                return n.ToString();

            if (n < 10000)
                return String.Format("{0:#,.##}K", n - 5);

            if (n < 100000)
                return String.Format("{0:#,.#}K", n - 50);

            if (n < 1000000)
                return String.Format("{0:#,.}K", n - 500);

            if (n < 10000000)
                return String.Format("{0:#,,.##}M", n - 5000);

            if (n < 100000000)
                return String.Format("{0:#,,.#}M", n - 50000);

            if (n < 1000000000)
                return String.Format("{0:#,,.}M", n - 500000);

            return String.Format("{0:#,,,.##}B", n - 5000000);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            cryptoItem.AggregatedData = await GetAggregatedData(cryptoItem.Info.Symbol);
            
            viewModel.Budget.Text = cryptoItem.CurrentBudget.ToString();
            viewModel.HeaderCoinTitle.Text = cryptoItem.Info.Name;
            viewModel.PercentDay.Text = "PCT Change(1H) " + Math.Round(cryptoItem.AggregatedData.ChangePCTDay.Value, 4).ToString();
            viewModel.PercentHour.Text = "PCT Change(1D) " + Math.Round(cryptoItem.AggregatedData.ChangePCT24Hour.Value, 4).ToString();
            viewModel.PercentWeek.Text = "PCT Change(1M) " + Math.Round(cryptoItem.AggregatedData.ChangePCTDay.Value, 4).ToString();
            viewModel.Rank.Text = "Rank #" + cryptoItem.Info.SortOrder.ToString();
            viewModel.TotalSupply.Text = "Total Volume " + FormatNumber(cryptoItem.AggregatedData.TotalVolume24H.Value);
            viewModel.MarketCap.Text = "Market Cap " + FormatNumber(cryptoItem.AggregatedData.MarketCap.Value);
            viewModel.PercentAllTime.Text = Math.Round(cryptoItem.PercentAllTime,4).ToString()+"%";
            viewModel.PercentProfitHour.Text = Math.Round(cryptoItem.PercentHour, 4).ToString() + "%";

            Picasso.With(this.Context).Load(Constant.BaseCoinUri + cryptoItem.Info.ImageUrl).Into(viewModel.Image);
            ChartFragment chartFragment = new ChartFragment();
            Bundle data = new Bundle();
            data.PutString("walletObject", JsonConvert.SerializeObject(cryptoItem));
            var palette = PaletteCreator.GetPalette(viewModel.Image);
            data.PutIntArray("color_data", new int[] { palette.VibrantColor, palette.DarkVibrantColor });
            chartFragment.Run(Resource.Id.wallet_chart_container, (AppCompatActivity)Activity, data);


        }
        private async Task<CoinFullAggregatedData> GetAggregatedData(string symbol)
        {
            return await HandyCryptoClient.Instance.GeneralCoinInfo.GetPriceData(symbol);
        }
    }
}