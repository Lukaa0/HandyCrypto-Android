using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.Activities;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using PortableCryptoLibrary;

namespace HandyCrypto
{
    [Application]
    public class App : Application
    {
        public App(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        public override  void OnCreate()
        {
            base.OnCreate();

            //HandyCryptoClient.Instance.GeneralCoinInfo.OnNetworkError += GeneralCoinInfo_OnNetworkError;
            



             
        }

        private void GeneralCoinInfo_OnNetworkError(object sender, EventArgs e)
        {
            StartActivity(typeof(NoConnectionActivity));
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CurrentDomain_UnhandledException1(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            if(exception is WebException ||  exception is HttpRequestException)
            {
                StartActivity(typeof(NoConnectionActivity));
            }
        }

       
    }
}