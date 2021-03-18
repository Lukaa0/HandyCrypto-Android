using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Syncfusion.Sfbusyindicator;
using HandyCrypto.Adapters;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using PortableCryptoLibrary;

namespace HandyCrypto.Fragments
{
    public class FavoritesFragment : Android.Support.V4.App.Fragment
    {
        private List<FavoriteCoin> favCoins;
        List<CryptoItemModel> items;
        SfBusyIndicator progressBar;
        private GridLayoutManager layoutManager;
        public FavoriteCoinRecyclerAdapter CoinRecyclerViewAdapter;
        private RecyclerView recyclerView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            layoutManager = new GridLayoutManager(Activity, 3, GridLayoutManager.Vertical, false);
            items = new List<CryptoItemModel>();
            favCoins = new List<FavoriteCoin>();


            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.all_coins_layout, container, false);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.mainRecyclerView);
            progressBar = view.FindViewById<SfBusyIndicator>(Resource.Id.progressBarMain);
            return view;

        }
        public override async void SetMenuVisibility(bool menuVisible)
        {
            base.SetMenuVisibility(menuVisible);
            if (menuVisible)
            {
                //progressBar.Visibility = ViewStates.Visible;
                //await favoritesDb.createDatabase();
                //favCoins = await favoritesDb.selectItems() ?? new List<FavoriteCoin>();
                //items = await DataUtility.PopulateAndGetData(favCoins);
                //coinRecyclerViewAdapter.ReplaceAndRefresh(items);
                //progressBar.Visibility = ViewStates.Gone;
            }
        }
        public override  async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                var favoriteCoins = await LocalDbService<FavoriteCoin>.Instance.SelectItems() ?? null;
                if (favoriteCoins != null)
                {
                    var data = await HandyCryptoClient.Instance.GeneralCoinInfo.GetInfoBySymbolAsync(favoriteCoins.Select(x => x.Symbol));
                    items = await HandyCryptoClient.Instance.GeneralCoinInfo.Construct(data);

                }
            }
            catch (Exception)
            {
            }
            SetAdapter(recyclerView);

            progressBar.Visibility = ViewStates.Gone;
        }
        private void SetAdapter(RecyclerView recycler)
        {
            var context = recycler.Context;
            LayoutAnimationController animation = null;
            animation = AnimationUtils.LoadLayoutAnimation(context, Resource.Animation.layoutFallDown);
            recycler.SetLayoutManager(layoutManager);
            CoinRecyclerViewAdapter = new FavoriteCoinRecyclerAdapter(items, Activity);
            recycler.SetItemViewCacheSize(20);
            recycler.DrawingCacheEnabled = true;
            recycler.SetAdapter(CoinRecyclerViewAdapter);
            //coinRecyclerViewAdapter.ItemClick += CoinRecyclerViewAdapter_ItemClick;

            //recycler.LayoutAnimation = animation;
            //recycler.GetAdapter().NotifyDataSetChanged();
            //recycler.ScheduleLayoutAnimation();

        }
    }
}