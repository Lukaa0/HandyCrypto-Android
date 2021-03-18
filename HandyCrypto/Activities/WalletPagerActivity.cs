//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.Icu.Text;
//using Android.OS;
//using Android.Runtime;
//using Android.Support.V4.View;
//using Android.Support.V7.App;
//using Android.Support.V7.Widget;
//using Android.Util;
//using Android.Views;
//using Android.Widget;
//using HandyCrypto.Adapters;
//using HandyCrypto.DatabaseHelper;
//using HandyCrypto.Model;
//using Com.Syncfusion.Charts;
//using Java.Util;
//using Jazzy;
//using PortableCryptoLibrary;
//using PortableCryptoServices;
//using Refractored.Fab;

//namespace HandyCrypto.Activities
//{
//    [Activity(Label = "WalletPagerAdapter")]
//    public class WalletPagerActivity : AppCompatActivity
//    {
      
//        private List<Wallet> wallets;
//        LinearLayoutManager layoutManager;
//        DateTime now = DateTime.Now;
//        JazzyViewPager jazzyViewPager;
//        FloatingActionButton floatingAction;
//        TextView upTitle;
//        TextView profit;
//        private List<CryptoItem> coins;

//        public string WalletPath { get; private set; }
//        public string ImagesPath { get; private set; }

//        readonly string WalletDbName = "WalletDb.db";
//        readonly string ImagesDbName = "ImagesDb.db";
//        PortableCryptoServices.CryptoItemService cryptoItemService;
//        string OriginalPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
//        private DataHelper<Wallet> db;
//        private DataHelper<Images> dbImages;
//        private CategoryAxis xCoordinate;

//        public NumericalAxis YCoordinate;

//        View view;
//        private List<Images> imgs;


//        protected async override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            view = LayoutInflater.Inflate(Resource.Layout.wallet_pager_main, null);
//            SetContentView(view);
//            upTitle = FindViewById<TextView>(Resource.Id.upTitle2);
//            jazzyViewPager = FindViewById<JazzyViewPager>(Resource.Id.viewPager);
//            //floatingAction = FindViewById<FloatingActionButton>(Resource.Id.fab);
//            profit = FindViewById<TextView>(Resource.Id.wallet_profit2);
//            coins = new List<CryptoItem>();
//            WalletPath = Path.Combine(OriginalPath, WalletDbName);
//            ImagesPath = Path.Combine(OriginalPath, ImagesDbName);
//            db = new DataHelper<Wallet>();
//            dbImages = new DataHelper<Images>();
//            cryptoItemService = new PortableCryptoServices.CryptoItemService();
//            db.createDatabase(WalletPath);
//            dbImages.createDatabase(ImagesPath);
//            wallets = await DbInitAsync();
//            imgs = new List<Images>();
//            imgs = dbImages.selectItems(ImagesDbName).Result;



//            layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);

//           var s =  jazzyViewPager.Adapter = new WalletViewPagerAdapter(jazzyViewPager, coins, this, wallets, imgs,view);
//            jazzyViewPager.TransitionEffect = JazzyEffects.Zoom;
            


                
//        }
       

           
//        private async Task<List<Wallet>> DbInitAsync()
//        {
//            var myWallet = new List<Wallet>();
//            myWallet = await db.selectItems(WalletDbName);
//            foreach (var item in myWallet)
//            {
//                coins.Add(cryptoItemService.GetById(item.CoinID));
//            }
//            return myWallet;
//        }
//        private void ChartInit()
//        {


//        }
     
//    }
    
//}