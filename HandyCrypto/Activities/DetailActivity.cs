using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Graphics;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using Com.Syncfusion.Sfbusyindicator;
using HandyCrypto.DataHelper;
using HandyCrypto.Fragments;
using HandyCrypto.Model;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using PortableCryptoServices;
using Square.Picasso;
using Syncfusion.Android.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CryptoCompare;
using Android.Util;
using System.Linq;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using HandyCrypto.Adapters;
using System.Diagnostics;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using BottomNavigationBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Fragment = Android.Support.V4.App.Fragment;
using HandyCrypto.Activities;

namespace HandyCrypto
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : AppCompatActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener
    {
        Bitmap bitmap = null;
        Android.Support.V7.Widget.Toolbar toolbar;
        private DetailsFragment detailFragment;
        private ImageView image;
        private TextView title;
        private BottomBar _bottomBar;
        private TextView toolbarPrice;
        public ChartFragment ChartFragment = new ChartFragment();
        private View mainLayout;
        private CryptoItemModel cryptoItem;
        private string cryptoId;
        private string imgUrl;

        
        //LinearLayout fragStrip;
        //LinearLayout tabStrip;
        Bundle chartData;
        Bundle coinInfoData;
        List<Android.Support.V4.App.Fragment> fragments;

        private int vibrantColor;
        private int mutedColor;
        private int mutedDarkColor;
        private int mutedLightColor;
        private int darkVibrantColor;
        private int lightVibrantColor;
        private WalletDialogFragment walletDialogFragment;
        private Android.Support.V4.App.FragmentTransaction fragmentTransaction;

        protected override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ThemeValidator.ValidateTheme(this);
            SetContentView(Resource.Layout.DetailPage);
            Initialization();

            _bottomBar = BottomBar.Attach(this, savedInstanceState);
            

            // ConfigureSegment();


        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            _bottomBar.OnSaveInstanceState(outState);
            
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await InitializeCryptoData();
            imgUrl = string.Concat(Constant.BaseCoinUri, cryptoItem.Info.ImageUrl);
            Picasso.With(this).Load(imgUrl).Into(image);
            toolbarPrice.Text = cryptoItem.AggregatedData.Price.ToString();
            SetPalette();

            SetSupportActionBar(toolbar);
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(vibrantColor)));
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            FillData();

            title.Text = cryptoItem.Info.Name;
            _bottomBar.SetOnMenuTabClickListener(this);
            _bottomBar.SetItems(Resource.Menu.menu_navigation_bar);
            _bottomBar.SetActiveTabColor("#3F51B5");
            _bottomBar.MapColorForTab(1, Color.Blue);
            _bottomBar.MapColorForTab(2, Color.Yellow);
            OnMenuTabSelected(Resource.Id.navbar_item_charts);

        }

        private async Task InitializeCryptoData()
        {
            try
            {
                cryptoItem.Info = await HandyCryptoClient.Instance.GeneralCoinInfo.GetInfoByIdAsync(cryptoId);
                cryptoItem.AggregatedData = await HandyCryptoClient.Instance.GeneralCoinInfo.GetPriceData(cryptoItem.Info.Symbol);
            }
            catch (Exception)
            {

            }
        }


     
        private void FillData()
        {
            coinInfoData.PutString("cryptoDetails", JsonConvert.SerializeObject(cryptoItem));
            coinInfoData.PutIntArray("color_data", new int[] { vibrantColor, mutedColor });

            ChartFragment.Arguments = coinInfoData;
            walletDialogFragment.Arguments = coinInfoData;
            detailFragment.Arguments = coinInfoData;
        }





        private void SetPalette()
        {
            BitmapDrawable bitmapDrawable = (BitmapDrawable)image.Drawable;
            bitmap = bitmapDrawable.Bitmap;
            var palette = new Palette.Builder(bitmap).Generate();
            SetPalette(palette);
        }
        private void Initialization()
        {
            cryptoId = Intent.Extras.GetString("cryptoItem");
            cryptoItem = new CryptoItemModel();
            detailFragment = new DetailsFragment();

            walletDialogFragment = new WalletDialogFragment();
            fragmentTransaction = SupportFragmentManager.BeginTransaction();

            fragments = new List<Android.Support.V4.App.Fragment>();
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarDetail);
            //tabStrip = FindViewById<LinearLayout>(Resource.Id.tabStrip);
            // infoButton = FindViewById<Button>(Resource.Id.infoStrip);
            image = FindViewById<ImageView>(Resource.Id.detailPageImage);
            title = FindViewById<TextView>(Resource.Id.detailPageTitle);
            mainLayout = FindViewById(Resource.Id.detailLayout);
            toolbarPrice = FindViewById<TextView>(Resource.Id.detailPageToolbarPrice);
            chartData = new Bundle();
            coinInfoData = new Bundle();

        }
       



       

       

        public void SetPalette(Palette palette)
        {
            var defaultcolor = Color.ParseColor("#3F51B5");
            vibrantColor = palette.GetVibrantColor(defaultcolor);
            mutedColor = palette.GetMutedColor(defaultcolor);
            darkVibrantColor = palette.GetDarkVibrantColor(defaultcolor);
            lightVibrantColor = palette.GetLightVibrantColor(defaultcolor);
            mutedColor = palette.GetMutedColor(defaultcolor);
            mutedDarkColor = palette.GetDarkMutedColor(defaultcolor);
            mutedLightColor = palette.GetLightMutedColor(defaultcolor);


        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.addto_wallet:
                    {
                        Android.Support.V4.App.FragmentTransaction dialogTransaction = SupportFragmentManager.BeginTransaction();
                        walletDialogFragment.Show(dialogTransaction, "dialog");
                        break;
                    }

                case Resource.Id.set_icon:
                    {
                        StartActivity(typeof(ChatActivity));
                        break;
                    }
                case Resource.Id.add_alarm:
                    {
                        var alarmFragment = new AlarmDialogFragment();
                        var data = new Bundle();
                        data.PutString("alarm_symbol", cryptoItem.Info.Symbol);
                        alarmFragment.Arguments = data;
                        alarmFragment.Show(SupportFragmentManager.BeginTransaction(),"alert-fragment");
                        break;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_detail, menu);
            return true;

        }
        public void OnMenuTabSelected(int menuItemId)
        {
            if (cryptoItem == null || cryptoItem.Info == null || cryptoItem.AggregatedData == null)
            {
                OpenFragment(new NoConnectionFragment(),this);
                return;
            }
            switch (menuItemId)
            {

                case Resource.Id.navbar_item_charts:
                {
                      
                    OpenFragment(ChartFragment,this);
                    break;
                }
                case Resource.Id.navbar_item_details:
                {
                    OpenFragment(detailFragment,this);
                    break;
                }
                case Resource.Id.navbar_item_news:
                {
                    OpenFragment(new NewsFragment(),this);
                    break;
                }

            }
        }
        public static void OpenFragment(Fragment fragment,Activity activity)
        {
          
            FragmentTransaction transaction = ((AppCompatActivity)activity).SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.detail_container, fragment);
            transaction.Commit();

        }
        public void OnMenuTabReSelected(int menuItemId)
        {
        }

        }
    }
