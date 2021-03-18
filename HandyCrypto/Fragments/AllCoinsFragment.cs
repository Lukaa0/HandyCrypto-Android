using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Sfbusyindicator;
using CryptoCompare;
using HandyCrypto.Activities;
using HandyCrypto.Adapters;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using PortableCryptoLibrary;
using Syncfusion.Android.TabView;
using Syncfusion.SfPullToRefresh;

namespace HandyCrypto.Fragments
{
    public class AllCoinsFragment : Android.Support.V4.App.Fragment
    {
        
        RecyclerView recyclerView;
        public List<CoinInfo> coins { get; set; }
        public CoinRecyclerViewAdapter coinRecyclerViewAdapter;
        private List<CryptoItemModel> cryptoModel;
        bool isLoading = false;
        GridLayoutManager layoutManager;
        private CoinRecyclerViewScrollListener onScrollListener;
        List<FavoriteCoin> favorites;
        CoordinatorLayout mainLayout;
        Bundle Data;
        string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private View view;
        SfBusyIndicator progressBar;
        private LinearLayout mainContainer;
        private Button _forecastButton;
        private SwipeRefreshLayout pullToRefresh;
        private SfTabView tabView;
        private string coinlistPath;
        private string favoritesPath;
        private bool isInside = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            favorites = new List<FavoriteCoin>();



        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             view = inflater.Inflate(Resource.Layout.all_coins_layout, container, false);
            progressBar = view.FindViewById<SfBusyIndicator>(Resource.Id.progressBarMain);
            mainContainer = view.FindViewById<LinearLayout>(Resource.Id.all_coins_container);
            pullToRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            pullToRefresh.Refresh += PullToRefresh_Refreshing;
            FindViews();
            return view;
        }

      

        private async void PullToRefresh_Refreshing(object sender, EventArgs e)
        {
            await InitializeModel();
            coinRecyclerViewAdapter.ReplaceAndRefresh(cryptoModel);
            pullToRefresh.Refreshing = false;
        }

        private async Task InitializeModel()
        {
            try
            {
                cryptoModel = await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(0, 100);
                if (cryptoModel == null || cryptoModel.Count <= 0)
                    Toast.MakeText(Activity, "No internet connection", ToastLength.Long).Show();
            }
            catch (Exception)
            {
                Toast.MakeText(Activity, "No internet connection", ToastLength.Long).Show();

            }
        }
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);


            progressBar.AnimationType = Com.Syncfusion.Sfbusyindicator.Enums.AnimationTypes.MovieTimer;
            progressBar.IsBusy = true;
            progressBar.Visibility = ViewStates.Visible;
            progressBar.Title = "Loading...";
            coins = await DbInit();
            favorites = await LocalDbService<FavoriteCoin>.Instance.SelectItems() ?? new List<FavoriteCoin>();
            HandyCryptoClient.Instance.GeneralCoinInfo.Coins = coins;
            await InitializeModel();

            Data = new Bundle();

            layoutManager = new GridLayoutManager(Activity, 3, GridLayoutManager.Vertical, false);

            onScrollListener = new CoinRecyclerViewScrollListener(layoutManager);
            recyclerView.AddOnScrollListener(onScrollListener);

            onScrollListener.LoadMoreEvent += OnScrollListener_LoadMoreEvent;
            SetAdapterWithAnim(recyclerView);
            progressBar.Visibility = ViewStates.Gone;



        }

        public class GridSpan : GridLayoutManager.SpanSizeLookup
        {
            public CoinRecyclerViewAdapter mAdapter { get; set; }
            public GridSpan(CoinRecyclerViewAdapter adapter)
            {
                mAdapter = adapter;
            }
            public override int GetSpanSize(int position)
            {
                return mAdapter.GetItemViewType(position) == 0 ? 3 : 1;

            }
        }









        private async void OnScrollListener_LoadMoreEvent(object sender, EventArgs e, int position)
        {
            Console.WriteLine(position);
            List<CryptoItemModel> model = null;
            while (true&&!isInside)
            {
                isInside = true;
                try {
                     model = await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(position, 60);
                    break;
                }
                catch (Exception)
                {
                    isInside = true;
                    OnNetworkError();
                }

            }
            if (model != null)
            {
                coinRecyclerViewAdapter.AddAndRefresh(model);
                onScrollListener.isLoading = false;
            }
            isInside = false;
        }

        private void WalletNav_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(Activity, typeof(WalletActivity)));
            Activity.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
        }


        private void SetAdapterWithAnim(RecyclerView recycler)
        {
            var context = recycler.Context;
            layoutManager.ItemPrefetchEnabled = true;
            recycler.SetLayoutManager(layoutManager);
            
            coinRecyclerViewAdapter = new CoinRecyclerViewAdapter(cryptoModel, Activity, 
                Activity.SupportFragmentManager,favorites,false);
            var gridSpan = new GridSpan(coinRecyclerViewAdapter);
           layoutManager.SetSpanSizeLookup(gridSpan);
            recyclerView.SetItemViewCacheSize(25);
            recycler.SetAdapter(coinRecyclerViewAdapter);
            

        }


        private async Task<List<CoinInfo>> DbInit()
        {
            var items = await LocalDbService<CoinInfo>.Instance.SelectItems();



            if (items.Count == 0 || items == null)
            {
                bool isFinished = false;
                while (!isFinished)
                {
                    try
                    {
                        progressBar.Title = "First time load...";
                        var coinData =
                            (await HandyCryptoClient.Instance.GeneralCoinInfo.GetCoinInfo()).OrderBy(
                                x => x.SortOrder);
                        var s1 = coinData.ToList();
                        

                        await LocalDbService<CoinInfo>.Instance.InsertAllAsnyc(coinData);
                        var itemss = await LocalDbService<CoinInfo>.Instance.SelectItems();
                        isFinished = true;
                        return coinData.ToList();
                    }
                    catch (Exception ex)
                    {
                        OnNetworkError();
                    }
                }
            }

            progressBar.Title = "Loading coins...";
            return items;
        }

        private void OnNetworkError()
        {
            Toast.MakeText(this.Activity, "Unresponsive network, retrying...", ToastLength.Long).Show();
        }

        private void FindViews()
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.mainRecyclerView);
            

            //help = FindViewById<ImageView>(Resource.Id.help_toolbar);

        }

        
    }
}