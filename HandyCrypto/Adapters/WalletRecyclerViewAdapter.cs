using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Clans.Fab;
using Com.Syncfusion.Charts;
using HandyCrypto.Activities;
using HandyCrypto.DataHelper;
using HandyCrypto.Extensions;
using HandyCrypto.Fragments;
using HandyCrypto.Model;
using HandyCrypto.View_Holders;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using Square.Picasso;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace HandyCrypto.Adapters
{
    public class WalletRecyclerViewAdapter : RecyclerView.Adapter
    {
        public List<WalletModel> Coins{ get; set; }

        private Activity activity;
        private RecyclerView recView;
        public event EventHandler<int> ItemClick;
        private WalletModel cryptoItem;
        private ObservableCollection<HistoricalDataModel> historicalDataItem;
        private string CoinUrl;
        private string baseImageUrl = Constant.BaseCoinUri;

        public WalletRecyclerViewAdapter(List<WalletModel> coins, Activity act, RecyclerView _recView)
        {
            activity = act;
            Coins = coins;
            recView = _recView;

        }

        public void AddAndRefresh(List<WalletModel> data)
        {
            Coins.AddRange(data);
            NotifyDataSetChanged();

        }

        public override int ItemCount => Coins.Count;

        public void OnClick(int position)
        {
            if (Coins[position].CurrentBudget == 0 && Coins[position].PercentAllTime == 0)
            {
                Toast.MakeText(activity, "Data not available, will be fixed in next release", ToastLength.Long).Show();
                return;
            }
            WalletDetailFragment walletDetailFragment = new WalletDetailFragment();
            Bundle data = new Bundle();
            data.PutString("wallet-data", JsonConvert.SerializeObject(Coins[position]));
            walletDetailFragment.Run(Resource.Id.wallet_frame, (AppCompatActivity)activity, data,true);


        }

        public void Update(List<WalletModel> data)
        {
            Coins = data;
            NotifyDataSetChanged();

        }
        public void AddBudgetValue(decimal result,int walletId)
        {
            try
            {
                var position = Coins.FindIndex(x => x.Wallet.Id == walletId);
                Coins[position].CurrentBudget = result;
                NotifyItemChanged(position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            
        }
        public void AddProfitAll(decimal result, int walletId,string tag)
        {
            try
            {
                var position = Coins.FindIndex(x => x.Wallet.Id == walletId);
                
                switch (tag)
                {
                    case "All": Coins[position].PercentAllTime = result; break;
                    case "Hour": Coins[position].PercentHour = result;break;

                }
                NotifyItemChanged(position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

        }

        public void AddChartData(ObservableCollection<HistoricalDataModel> historicalData, int walletId)
        {
            try
            {
                var position = Coins.FindIndex(x => x.Wallet.Id == walletId);
                Coins[position].HistoricalData = historicalData;
                activity.RunOnUiThread(() => { NotifyItemChanged(position); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            WalletListViewHolder walletViewHolder = holder as WalletListViewHolder;
            cryptoItem = Coins[position];
            CoinUrl = cryptoItem.Info.ImageUrl;
            string currentImageUrl = baseImageUrl + CoinUrl;
            try
            {

                Picasso.With(activity).Load(currentImageUrl).Into(walletViewHolder.CoinImage);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            try
            {
                    walletViewHolder.Name.Text = cryptoItem.Info.Name.ToUpper();
                    walletViewHolder.Percent.Text = Math.Round(cryptoItem.PercentAllTime,4).ToString() ?? "Loading...";
                    walletViewHolder.Budget.Text = cryptoItem.CurrentBudget.ToString();
                if (cryptoItem.PercentAllTime > 0)
                    walletViewHolder.Trending.SetImageResource(Resource.Drawable.ic_trending_up_green_light_24dp);
                Animation a = AnimationUtils.LoadAnimation(this.activity, Resource.Animation.abc_slide_in_bottom);
                    a.Duration = 500;
                    a.Reset();
                    walletViewHolder.Name.ClearAnimation();
                    walletViewHolder.Name.StartAnimation(a);
                    walletViewHolder.Percent.ClearAnimation();
                    walletViewHolder.Percent.StartAnimation(a);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

       


        //public void ChartAsync(WalletViewHolder viewHolder, int position)
        //{
        //    historicalDataItem = Coins[position].HistoricalData;
        //    if (historicalDataItem == null || historicalDataItem.Count <= 0) return;
        //    try
        //    {
        //        var chart = viewHolder.Chart;
        //        chart.Series.Clear();
                
        //        DateTimeCategoryAxis dateTimeAxis = new DateTimeCategoryAxis()
        //        {
        //            ShowTrackballInfo = true
        //        };
        //        NumericalAxis priceAxis = new NumericalAxis();

        //        chart.PrimaryAxis = dateTimeAxis;
        //        priceAxis.TrackballLabelStyle.LabelAlignment = ChartLabelAlignment.Far;
        //        chart.SecondaryAxis = priceAxis;
        //        CustomTrackBallBehavior trackballBehavior = new CustomTrackBallBehavior
        //        {
        //            ShowLabel = true,
        //            ShowLine = true,

        //        };
        //        ChartZoomPanBehavior zoomPan = new ChartZoomPanBehavior()
        //        {
        //            ScrollingEnabled = false,
        //            ZoomingEnabled = true,


        //        };

        //        chart.Series.Add(GetChartSeries());
        //        chart.Behaviors.Add(trackballBehavior);
        //        chart.Behaviors.Add(zoomPan);
        //        chart.Focusable = false;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        
        private AreaSeries GetChartSeries()
        {
            var series = new AreaSeries()
            {
                ItemsSource = historicalDataItem,

                XBindingPath = "Date",

                YBindingPath = "Price",

                ShowTrackballInfo = true,
                TooltipEnabled = true,
                EnableAnimation=true


            };
            return series;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wallet_base_list_item, parent, false);
            WalletListViewHolder walletViewHolder = new WalletListViewHolder(view, OnClick,OnDeleteClick);
            return walletViewHolder;
        }

        private async void OnDeleteClick(int position)
        {
            var item = Coins[position];
            var accessKey = item.Wallet.Id;
            var walletActivity = (WalletActivity) activity;
            var walletDbItem = walletActivity.wallets.Find(x => x.Id == accessKey);
            if (walletDbItem != null)
            {
                await LocalDbService<Wallet>.Instance.DeleteAsync(walletDbItem);

                Coins.Remove(item);
                this.NotifyItemRemoved(position);
            }
            else
                Toast.MakeText(activity, "Could not delete the item", ToastLength.Long).Show();
        }
        private class WalletListViewHolder :RecyclerView.ViewHolder
        {
            public ImageButton DeleteButton { get; set; }
            public TextView Budget { get; set; }
            public TextView Percent { get; set; }
            public TextView Name { get; set; }


            public ImageView CoinImage { get; set; }
            public ImageView Trending { get; set; }

            // public SfChart chart { get; private set; }
            // public Button chartButton { get; private set; }

            public WalletListViewHolder(View view, Action<int> actionListener, Action<int> delClickListener) : base(view)
            {
                Budget = view.FindViewById<TextView>(Resource.Id.wallet_list_price);
                Percent = view.FindViewById<TextView>(Resource.Id.wallet_list_perc);
                CoinImage = view.FindViewById<ImageView>(Resource.Id.wallet_list_image);
                Trending = view.FindViewById<ImageView>(Resource.Id.wallet_list_trending);
                Name = view.FindViewById<TextView>(Resource.Id.wallet_list_title);
                DeleteButton = view.FindViewById<ImageButton>(Resource.Id.wallet_base_item_delete);
                DeleteButton.Click += (sender, e) => delClickListener(LayoutPosition);
                view.Click += (sender, e) => actionListener(base.LayoutPosition);

            }
        }
    }
}