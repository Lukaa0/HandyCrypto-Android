using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using HandyCrypto.Activities;

namespace HandyCrypto.Fragments
{
    public class PredictDialogFragment : Android.Support.V4.App.DialogFragment
    {
        private Button _arimaForecastButton;
        private Button _lstmForecastButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.predict_dialog_layout, container, false);
            _arimaForecastButton = view.FindViewById<Button>(Resource.Id.arima_forecast_btn);
            _lstmForecastButton = view.FindViewById<Button>(Resource.Id.lstm_forecast_btn);
            _arimaForecastButton.Click += PredictArima;
            _lstmForecastButton.Click += PredictLstm;
            base.OnCreateView(inflater, container, savedInstanceState);
            return view;
        }

        private void PredictLstm(object sender, EventArgs e)
        {
            var intent = new Intent(this.Context, typeof(PredictionActivity));
            intent.PutExtra("ptype", 1);
            Dismiss();
            StartActivity(intent);
        }
        private void PredictArima(object sender, EventArgs e)
        {
            var intent = new Intent(this.Context, typeof(PredictionActivity));
            intent.PutExtra("ptype", 0);
            Dismiss();
            StartActivity(intent);
        }
    }
}