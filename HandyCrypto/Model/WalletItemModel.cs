using System;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Clans.Fab;
using Com.Syncfusion.Charts;
using HandyCrypto.Model;

namespace HandyCrypto.View_Holders
{
    public class WalletItemModel
    {
        public TextView PercentHour { get; set; }
        public TextView PercentDay { get; set; }

        public TextView PercentWeek { get; set; }
        public TextView Rank { get; set; }
        public TextView MarketCap { get; set; }
        public TextView PriceTrackBall { get; set; }
        public TextView DateTrackBall { get; set; }
        public SfChart Chart { get; set; }
        public TextView TotalSupply { get; set; }
        public CryptoScrollView scrollView { get; set; }
        public ImageView Image { get; set; }
        public TextView PercentAllTime { get; set; }
        public TextView HeaderCoinTitle { get; private set; }
        public TextView Budget { get; private set; }
        public TextView PercentProfitHour{ get; set; }
        public TextView PercentProfitDay { get; set; }


        // public SfChart chart { get; private set; }
        // public Button chartButton { get; private set; }

        public WalletItemModel(View view) { 
            PercentHour = view.FindViewById<TextView>(Resource.Id.percentHour);
            PercentProfitHour = view.FindViewById<TextView>(Resource.Id.wallet_profit_1h);
            PercentProfitDay = view.FindViewById<TextView>(Resource.Id.wallet_profit_1d);

            PercentDay = view.FindViewById<TextView>(Resource.Id.percentDay);
            PercentWeek = view.FindViewById<TextView>(Resource.Id.percentWeek);
            MarketCap = view.FindViewById<TextView>(Resource.Id.MarketCap);
            Rank = view.FindViewById<TextView>(Resource.Id.Rank);
            Chart = view.FindViewById<SfChart>(Resource.Id.wallet_chart);
            TotalSupply = view.FindViewById<TextView>(Resource.Id.totalSupply);
            Image = view.FindViewById<ImageView>(Resource.Id.walletCoinImage);
            HeaderCoinTitle = view.FindViewById<TextView>(Resource.Id.upTitle2);
            Budget = view.FindViewById<TextView>(Resource.Id.wallet_profit2);
           // chart = view.FindViewById<SfChart>(Resource.Id.walletChartPage);
           // chartButton = view.FindViewById<Button>(Resource.Id.chartButton);
            PercentAllTime = view.FindViewById<TextView>(Resource.Id.percentAllTime);
            PriceTrackBall = view.FindViewById<TextView>(Resource.Id.wallet_track_price_txt);
            DateTrackBall = view.FindViewById<TextView>(Resource.Id.wallet_track_date_txt);
            scrollView = view.FindViewById<CryptoScrollView>(Resource.Id.wallet_item_scrollview);






        }

       
    }
}