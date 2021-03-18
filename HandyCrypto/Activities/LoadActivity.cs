using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using PortableCryptoLibrary;

namespace HandyCrypto.Activities
{
    [Activity(Theme ="@style/SplashTheme",NoHistory =true,MainLauncher =true)]
    public class LoadActivity : AppCompatActivity
    {
        
        IConnectivity connectivity = CrossConnectivity.Current;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            HandyFirebaseClient.Instance.InitializeApp(this);
            HandyCryptoClient.Instance.RegisterApiKey("9540e07b5ca9ae7f91b95b90170c43735e51905f510cdd7690d2675ceb4770ee");

        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            
            if (!e.IsConnected)
            {
                Toast.MakeText(ApplicationContext, "Network Error, Retrying...", ToastLength.Long).Show();
                
            }
            
        }

        protected override async void OnResume()
        {
            base.OnResume();
            //Task task = new Task(() => { LoadData(); });
            //task.Start();
            Thread.Sleep(500);
            await LocalDbService<Wallet>.Instance.CreateTableAsync();
            await LocalDbService<CoinInfo>.Instance.CreateTableAsync();
            await LocalDbService<FavoriteCoin>.Instance.CreateTableAsync();
            await LocalDbService<AlarmItemModel>.Instance.CreateTableAsync();
            await LocalDbService<User>.Instance.CreateTableAsync();
            this.StartActivity(typeof(MainActivity));
            
        }


        private async void LoadData()
        {
            await Task.Run( () =>
           {
               bool isConnected = connectivity.IsConnected;
               if (!isConnected)
               {

                   this.StartActivity(typeof(NoConnectionActivity));
               }
               else
               {
                   try
                   {
                       
                   }
                   catch 
                   {
                       this.StartActivity(typeof(NoConnectionActivity));

                   }

                   //Intent intent = new Intent();
                   //intent.SetClass(this, typeof(MainActivity));

                   //intent.PutExtra("CryptoCoin", JsonConvert.SerializeObject(coins));
                   this.StartActivity(typeof(MainActivity));
               }
           });
        }
    }
}