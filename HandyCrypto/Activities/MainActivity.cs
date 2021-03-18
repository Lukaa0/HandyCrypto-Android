using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.Activities;
using HandyCrypto.Adapters;
using HandyCrypto.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using PortableCryptoLibrary;
using Android.Support.V4.View;
using Com.Andremion.Floatingnavigationview;
using Com.Ajithvgiri.Searchdialog;
using HandyCrypto.Services;
using AFollestad.MaterialDialogs;
using static Android.Support.Design.Widget.NavigationView;
using Xamarin.Essentials;
using HandyCrypto.Model;
using Android.Gms.Common;
using Android.Util;

namespace HandyCrypto
{
    [Activity(Label = "HandyCrypto")]
    public class MainActivity : AppCompatActivity, IOnSearchItemSelected, IOnNavigationItemSelectedListener
    {
        public FavoritesFragment FavoritesFragment;
        public const string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        public AllCoinsFragment CoinsFragment;
        public ViewPagerAdapter ViewPagerAdapter;
        private FloatingNavigationView FabView;
        private Android.Support.V7.Widget.Toolbar toolbar;
        private ViewPager viewPager;
        private TabLayout tabLayout;
        private int _themeId;
        public override void SetTheme(int resid)
        {
            base.SetTheme(resid);
            _themeId = resid;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ThemeValidator.ValidateTheme(this);
            SetContentView(Resource.Layout.Main);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "MTIzNDQyQDMxMzcyZTMyMmUzMGlETHdiQnRZNGFiTXJaa2lzQnRyMlY3L2ZLQXA5RXhxZ2k5Z2g4ZmgvRFU9");
            
            Initialize();
            FindViews();
            SetSupportActionBar(toolbar);
            SetupViewPager(viewPager);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.all_coins);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.ic_favorite_red_dark_36dp);
            FabView.Click += (sender, e) => { FabView.Open(); };
            FabView.SetNavigationItemSelectedListener(this);
            SetAlarmForBackgroundServices(this);
            //searchView.SetOnQueryTextListener(this);
            //var coinNames = coinsFragment.coins.Select(x => x.CoinName);
            //searchView.AddSuggestions(coinNames);
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
            CreateNotificationChannel();


        }

        protected override void OnResume()
        {
            base.OnResume();
            //ThemeValidator.ValidateRecreation(this, _themeId);
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {

                return;
            }

            var channelName = new Java.Lang.String("HandyCrypto");
            var channelDescription = "HandyCrypto Notifications";
            var channel = new NotificationChannel("255", channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        protected override void OnDestroy()
        {
            Intent alarmIntent = new Intent(this.ApplicationContext, typeof(AlarmReceiver));

            StopService(alarmIntent);
            base.OnDestroy();
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.wallet_nav:
                    {
                        StartActivity(typeof(WalletActivity));
                        break;
                    }

                case Resource.Id.set_icon:
                    {
                        StartActivity(typeof(ChatActivity));
                        break;
                    }
                case Resource.Id.search_nav:
                    {
                        var searchListItems = new List<SearchListItem>();

                        var coins = HandyCryptoClient.Instance.GeneralCoinInfo.Coins.ToList().OrderBy(x => x.SortOrder);
                        foreach (var coin in coins)
                        {
                            searchListItems.Add(new SearchListItem(Convert.ToInt32(coin.Id), coin.Symbol));
                        }
                        var searchableDialog = new SearchableDialog(this, searchListItems, "Search");
                        searchableDialog.Show();
                        searchableDialog.SetOnItemSelected(this);

                        break;
                    }



            }
            return base.OnOptionsItemSelected(item);
        }

        public void SetupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter.AddFragment(CoinsFragment, "All Coins");
            ViewPagerAdapter.AddFragment(FavoritesFragment, "Favorites");
            viewPager.Adapter = ViewPagerAdapter;


        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);

            return true;

        }
        private void Initialize()
        {
            CoinsFragment = new AllCoinsFragment();
            ViewPagerAdapter = new ViewPagerAdapter(SupportFragmentManager);
            FavoritesFragment = new FavoritesFragment();
        }

        private void FindViews()
        {
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            FabView = FindViewById<FloatingNavigationView>(Resource.Id.floating_navigation_view);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);


        }
        public static void SetAlarmForBackgroundServices(Context context)
        {
            Intent alarmIntent = new Intent(context.ApplicationContext, typeof(AlarmReceiver));
            var broadcast =
                PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.NoCreate);
            if (broadcast == null)
            {
                var pendingIntent = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, 0);
                var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, 1000, 5000,
                    pendingIntent);
            }
        }
        
        public async void OnClick(int p0, SearchListItem p1)
        {
            try
            {
                var item = await HandyCryptoClient.Instance.GeneralCoinInfo.GetCryptoItemModelBySymbol(p1.Title);
                if (item == null)
                    return;
                Bundle Data = new Bundle();
                Data.PutString("symbol", item.Info.Symbol);

                var view = LayoutInflater.From(this).Inflate(Resource.Layout.item_modal_dialog, null);
                var dialogBuilder = new MaterialDialog.Builder(this);
                dialogBuilder.CustomView(view, false);

                ItemModalDialogFragment itemModalDialogFragment = new ItemModalDialogFragment(view, item, this);
                itemModalDialogFragment.Create();
                var dialog = dialogBuilder.Build();
                dialog.Show();
                itemModalDialogFragment.OnCloseClick += (sender, e) => dialog.Dismiss();
            }
            catch (Exception)
            {
                Toast.MakeText(this, $"No data found for {p1.Title}", ToastLength.Long).Show();
            }

        }
        
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_alert:
                    {
                        var alarmDialog = new AlarmDialogFragment();
                        var data = new Bundle();
                        data.PutString("alarm_symbol", "BTC");

                        alarmDialog.Arguments = data;
                        alarmDialog.Show(SupportFragmentManager.BeginTransaction(), "alarm_dialog_from_main");
                        break;
                    }
                case Resource.Id.nav_chat:
                    {
                        StartActivity(typeof(ChatActivity));
                        break;
                    }
                case Resource.Id.nav_coins:
                    {
                        FabView.Close();
                        viewPager.SetCurrentItem(0, true);
                        break;
                    }
                    ;
                case Resource.Id.nav_favorites:
                    {
                        FabView.Close();
                        viewPager.SetCurrentItem(1, true); break;
                    }
                case Resource.Id.nav_wallet:
                    {
                        StartActivity(typeof(WalletActivity));
                        break;
                    }
                case Resource.Id.nav_profile:
                    {
                        ProfileDialogFragment profileDialog = new ProfileDialogFragment();
                        profileDialog.Show(SupportFragmentManager, "profile-main");
                        break;
                    }
                case Resource.Id.nav_bug_report:
                    {
                        BugReportingDialogFragment bugReportingDialog = new BugReportingDialogFragment();
                        bugReportingDialog.Show(SupportFragmentManager, "bug-main");
                        break;
                    }
                case Resource.Id.nav_settings:
                    {
                        
                        StartActivity(typeof(SettingsActivity));
                        break;
                    }

            }
            return true;
        }




    }
}
    

