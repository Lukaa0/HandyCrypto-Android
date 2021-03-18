//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.Graphics;
//using Android.OS;
//using Android.Runtime;
//using Android.Support.V7.App;
//using Android.Views;
//using Android.Widget;
//using HandyCrypto.Activities;
//using HandyCrypto.DataHelper;
//using HandyCrypto.Fragments;
//using HandyCrypto.Model;
//using Com.Syncfusion.Charts;
//using Java.Lang;
//using Java.Util;
//using Jazzy;
//using Newtonsoft.Json;
//using PortableCryptoLibrary;
//using Square.Picasso;

//namespace HandyCrypto.Adapters
//{
//    class WalletViewPagerAdapter : JazzyPagerAdapter
//    {
//        private Activity activity;
//        private List<CryptoItem> Coins;
//        public ObservableCollection<ChartData> chartData { get; set; }
//        private TextView PercentHour;
//        private TextView PercentDay;
//        private WalletChartFragment walletChartFragment;
//        private TextView PercentWeek;
//        private TextView Rank;
//        Button chartButton;
//        private TextView MarketCap;
//        private TextView TotalSupply;
//        private ProgressBar progressBar;
//        private WalletService walletService;
//        private ImageView Image;
//        private SfChart chart;
//        private List<double> profit;
//        private List<Wallet> wallets;
//        private TextView upCoinTtle;
//        private TextView budgetTxt;
//        private static readonly object syncLock = new object();
//        private Images images;
//        private List<Images> imgData;
//        private CryptoItem cryptoItem;
//        private Wallet walletItem;

//        public WalletViewPagerAdapter(JazzyViewPager pager, List<CryptoItem> coins, Activity act, List<Wallet> wallets, List<Images> images, View view) : base(pager)
//        {
//            activity = act;
//            Coins = coins;
//            this.wallets = wallets;
//            imgData = images;
//            walletService = new WalletService();
//        }




//        public override int Count => wallets.Count();
//        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
//        {

//            LayoutInflater inflater = (LayoutInflater)activity.BaseContext.GetSystemService(Context.LayoutInflaterService);

//            View pagerView = inflater.Inflate(Resource.Layout.wallet_layout, container, false);
//            cryptoItem = Coins[position];
//            walletItem = wallets[position];
//            ViewInstantiation(pagerView);
//            ViewInitialization(cryptoItem, walletItem, pagerView);
//            SetObjectForPosition(pagerView, position);
//            container.AddView(pagerView);
//            chartButton.Tag = position;


//                //chartButton.Click += (sender, args)=>
//                //{
//                //    int pos = (int)((Button)sender).Tag;
//                //    var wItem = wallets[pos];
//                //    Bundle data = new Bundle();
//                //    data.PutString("wallet", JsonConvert.SerializeObject(wItem));
//                //    walletChartFragment.Arguments = data;
//                //    Android.App.FragmentTransaction ft = activity.FragmentManager.BeginTransaction();
//                //    walletChartFragment.Show(ft, "dialog");

//                //};





//            return pagerView;
//        }
        
//        public  async Task GenerateChart(Wallet wal, SfChart chart)
//        {
//            var data = await Wallet.CalculateHistorical(Convert.ToDateTime(wal.InvestDate), "USD", wal.Symbol.ToUpper(), Convert.ToDecimal(wal.Investment));

//            DateTimeAxis dateTimeAxis = new DateTimeAxis();
//            var s = data.OrderBy(x => x.Date);
//            var minimum = data.Min(r => r.Date);
//            Toast.MakeText(activity, minimum.ToString(), ToastLength.Short).Show();
//            var maximum = data.Max(r => r.Date);

//            var str = minimum.ToString();
//            List<string> dates = new List<string>();
//            var str2 = maximum.ToString();
          
//            Calendar calendar = new GregorianCalendar(minimum.Year, minimum.Month, minimum.Day);
//            dateTimeAxis.Minimum = calendar.Time;
//            calendar.Set(maximum.Year, maximum.Month, maximum.Day);
//            dateTimeAxis.Maximum = calendar.Time;
//            NumericalAxis priceAxis = new NumericalAxis();
//            chart.PrimaryAxis = dateTimeAxis;
//            chart.SecondaryAxis = priceAxis;
//            chart.Series.Add(new LineSeries()
//            {


//                ItemsSource = data,

//                XBindingPath = "Date",

//                YBindingPath = "Price"


//            });
//            NotifyDataSetChanged();
//        }
//        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
//        {
//            var pagerView = FindViewFromObject(position);
//            container.RemoveView(pagerView);
//        }
//        public void ViewInstantiation(View pagerView)
//        {
//            PercentHour = pagerView.FindViewById<TextView>(Resource.Id.percentHour);
//            PercentDay = pagerView.FindViewById<TextView>(Resource.Id.percentDay);
//            PercentWeek = pagerView.FindViewById<TextView>(Resource.Id.percentWeek);
//            walletChartFragment = new WalletChartFragment();
//            TotalSupply = pagerView.FindViewById<TextView>(Resource.Id.totalSupply);
//            Image = pagerView.FindViewById<ImageView>(Resource.Id.walletCoinImage);
//            upCoinTtle = pagerView.FindViewById<TextView>(Resource.Id.upTitle2);
//            budgetTxt = pagerView.FindViewById<TextView>(Resource.Id.wallet_profit2);
//            //chartButton = pagerView.FindViewById<Button>(Resource.Id.chartButton);


//        }


//        public  void ViewInitialization(CryptoItem cryptoItem, Wallet wal, View view)
//        {
//            progressBar.Visibility = ViewStates.Visible;
//            activity.RunOnUiThread(async () =>
//            {
//                var result = await Wallet.CalculateCurrent(Convert.ToDateTime(wal.InvestDate), cryptoItem, "USD", cryptoItem.symbol.ToUpper(), Convert.ToDecimal(wal.Investment));
//               // budgetTxt.Text = result;
//                PercentDay.Text = $"Percent Change(1D): {cryptoItem.percent_change_24h}";
//                PercentHour.Text = $"Percent Change(1H): {cryptoItem.percent_change_1h}";
//                PercentWeek.Text = $"Percent Change(1W): {cryptoItem.percent_change_7d}";
//                TotalSupply.Text = $"Total Supply: {cryptoItem.total_supply}";
//                MarketCap.Text = $"Market Cap: {cryptoItem.market_cap_usd}";
//                upCoinTtle.Text = cryptoItem.name;
//                await GenerateChart(wal, chart);
//                NotifyDataSetChanged();

//            });


//            string BaseUrl, CoinUrl;
//            BaseUrl = "https://www.cryptocompare.com";
//            try
//            {
//                images = imgData.FirstOrDefault(x => x.CoinName.ToLower() == cryptoItem.symbol.ToLower());
//            }
//            catch
//            {
//                images = imgData[0];

//            }
//            if (images == null)
//            {
//                images = imgData[0];
//            }
//            CoinUrl = images.ImageUrl;
//            string currentImageUrl = BaseUrl + CoinUrl;

//            //var data = await Wallet.CalculateHistorical(Convert.ToDateTime(wal.InvestDate), "USD", wal.Symbol.ToUpper(), Convert.ToDecimal(wal.Investment));
//            //GenerateChart(wal, view,data);
            
//            progressBar.Visibility = ViewStates.Gone;
//        }


//    }

//    class WalletViewPagerAdapterViewHolder : Java.Lang.Object
//    {
//        //Your adapter views to re-use
//        //public TextView Title { get; set; }
//    }
//}
