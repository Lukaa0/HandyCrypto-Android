using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using HandyCrypto.Extensions;
using PortableCryptoServices;

namespace HandyCrypto.Activities
{
    [Activity(Label = "PredictionActivity")]
    public class PredictionActivity : AppCompatActivity
    {
        ProgressBar _progressBar;
        SfChart _chart;
        private AreaSeries _series;
        private ObservableCollection<OhlcDataModel> data;
        public override void OnBackPressed()
        {
            StartActivity(new Intent(this,typeof(MainActivity)));
        }
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.prediction_result_layout);
            _chart = FindViewById<SfChart>(Resource.Id.arima_prediction_chart);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.arima_prediction_progressbar);
            _progressBar.Visibility = ViewStates.Visible;
            _chart = _chart.Initialize();
            var type = Intent.Extras.GetInt("ptype", 1);
            data = await GetData(type);

            _series = new AreaSeries()
            {
                ItemsSource = data,

                XBindingPath = "Date",
                YBindingPath = "Close"
            };

            _chart.Series.Add(_series);
            ((NumericalAxis)_chart.SecondaryAxis).Minimum = GetMin(data);
            _progressBar.Visibility = ViewStates.Gone;
        }

        private async Task<ObservableCollection<OhlcDataModel>> GetData(int type)
        {
            double[] data;
            if(type==0)
                 data = await PredictionService.PredictArima();
            else 
                 data = await PredictionService.PredictLstm();
            ObservableCollection<OhlcDataModel> ohlcData = new ObservableCollection<OhlcDataModel>();
            for (int i = 0; i < data.Length; i++)
            {
                ohlcData.Add(new OhlcDataModel(DateTime.Today.AddDays(i+1), (decimal)(data[i]), 0, 0, 0));
            }
            return ohlcData;
        }
        private double GetMin(ObservableCollection<OhlcDataModel> ohlcData)
        {
            if (ohlcData != null || ohlcData.Count != 0)
                return Convert.ToDouble(ohlcData.Min(x => x.Close));
            return 0;
        }

    }
}