using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Xamarin.Database;

namespace HandyCrypto.Model
{
    class HandyFirebaseClient
    {
        private static readonly Lazy<HandyFirebaseClient> lazy =
            new Lazy<HandyFirebaseClient>(() =>
            {

                return new HandyFirebaseClient();

            });

        public static HandyFirebaseClient Instance { get { return lazy.Value; } }

        private HandyFirebaseClient()
        {
            MainClient = new FirebaseClient("https://handycrypto-e1cfa.firebaseio.com/");
        }
        public FirebaseClient MainClient;
        public FirebaseApp FirebaseApp;
        //public FirebaseAuth Authenticator;
        public FirebaseDatabase FirebaseDatabase;
        public void InitializeApp(Context context)
        {
            try
            {
                FirebaseOptions options = new FirebaseOptions.Builder()
                 .SetApiKey("AIzaSyDG68VuAlvPGjff0Viq7V5x3avbET39C0A")
                 .SetApplicationId("1:1025533193132:android:baaa188bbae79959").SetDatabaseUrl("https://handycrypto-e1cfa.firebaseio.com/").Build();
                FirebaseApp = FirebaseApp.InitializeApp(context, options);
                // Authenticator = FirebaseAuth.GetInstance(FirebaseApp);
                FirebaseDatabase = FirebaseDatabase.GetInstance(FirebaseApp);
            }
            catch (Exception)
            {
                //Null-check
            }
        }
    }
}
