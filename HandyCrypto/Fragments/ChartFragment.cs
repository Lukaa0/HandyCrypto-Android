using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using CryptoCompare;
using HandyCrypto.Model;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using PortableCryptoServices;
using Syncfusion.Android.Buttons;

namespace HandyCrypto.Fragments
{
    public delegate Task<List<CandleData>> HistoricalDataDelegate(
           string symbol,
           string currency,
           int? limit,
           bool? allData = null,
           DateTimeOffset? toDate = null,
           string exchangeName = "CCCCAG",
           int? aggregate = null,
           bool? tryConvention = null
       );
    public class ChartFragment : Android.Support.V4.App.Fragment
    {
        private SfSegmentedControl segmentedChartTime;
        private SfSegmentedControl segmentedChartType;
        private CryptoItemModel cryptoItem;
        private WalletModel walletModel;
        private int[] colorData;

        //private int[] colorData;
        private SfChart sfChart;
        public TextView testText;
        private HistoricalDataDelegate dayDel;
        private ObservableCollection<OhlcDataModel> ohlcData;
        private HistoricalDataDelegate hourDel;
        private HistoricalDataDelegate minuteDel;
        private ChartSeries _series;
        private ChartColorModel seriesCustomColors;
        private Button _forecastButton;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            cryptoItem = Arguments.GetString("cryptoDetails") == null ? null : JsonConvert.DeserializeObject<CryptoItemModel>(Arguments.GetString("cryptoDetails"));
            walletModel = Arguments.GetString("walletObject") == null ? null : JsonConvert.DeserializeObject<WalletModel>(Arguments.GetString("walletObject"));

            colorData = Arguments.GetIntArray("color_data");

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.chart_detail_layout, container, false);
            if (cryptoItem == null && walletModel == null)
                DetailActivity.OpenFragment(new NoConnectionFragment(), Activity);
            FindViews(view);
            dayDel = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalDayPrices;
            minuteDel = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalMinutePrices;
            hourDel = HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalHourPrices;
            ConfigureSegment(view);
            InitChart();
            testText = view.FindViewById<TextView>(Resource.Id.test_date_text);
            _forecastButton = view.FindViewById<Button>(Resource.Id.fcast_btn);
            _forecastButton.Click += _forecastButton_Click;
            seriesCustomColors = new ChartColorModel(ChartColorPalette.Custom);
            if (walletModel == null & cryptoItem?.Info.Symbol == "BTC")
                _forecastButton.Visibility = ViewStates.Visible;

            //seriesCustomColors.CustomColors.Add(new Color(colorData[0]));
            //seriesCustomColors.CustomColors.Add(new Color(colorData[1]));
            if (cryptoItem == null)
                segmentedChartType.Visibility = ViewStates.Gone;
            _series = new AreaSeries()
            {
                ItemsSource = ohlcData,
                Color = new Color(colorData[0]),
                XBindingPath = "Date",
                YBindingPath = "Close",
                
            };
            
            sfChart.Series.Add(_series);
            return view;

        }

        private void _forecastButton_Click(object sender, EventArgs e)
        {

            Android.Support.V4.App.FragmentTransaction transaction = Activity.SupportFragmentManager.BeginTransaction();
            new PredictDialogFragment().Show(transaction, "forecast_dialog");
        }

        private void FindViews(View view)
        {
            segmentedChartTime = view.FindViewById<SfSegmentedControl>(Resource.Id.segment_chartType);
            segmentedChartType = view.FindViewById<SfSegmentedControl>(Resource.Id.segment_chartTime);
            sfChart = view.FindViewById<SfChart>(Resource.Id.detail_chart);

        }
        private async Task GetHitoricalData(HistoricalDataDelegate chartData, string
            currency, int? limit, bool? allData = null, DateTime? toDate = null, bool? 
            tryConvention = null, int? aggregate = null, string exchangeName = "CCCAGG")
        {
            try
            {
                ohlcData = new ObservableCollection<OhlcDataModel>();
                if (cryptoItem != null)
                {
                    string symbol = cryptoItem.Info.Symbol;
                    var histoData = await chartData(symbol, currency, limit, allData, toDate, exchangeName, aggregate, tryConvention);
                    if (histoData == null)
                    {
                        DetailActivity.OpenFragment(new NoConnectionFragment(), Activity);
                        return;
                    }

                    foreach (var t in histoData)
                    {
                        ohlcData.Add(new OhlcDataModel(t.Time.DateTime, t.Close, t.Open, t.High, t.Low));
                    }
                }
                else
                {
                    using (WalletService service = new WalletService())
                    {
                        string symbol = walletModel.Info.Symbol;


                        var histoData = await service.CalculateHistorical(chartData, currency,
                            symbol, walletModel.Wallet.Investment.ToDecimalWithCulture(),
                            walletModel.Wallet.CoinPrice, walletModel.Wallet.InvestDate.ToDateTime(), limit, allData, toDate,
                            tryConvention, aggregate, exchangeName);
                        if (histoData == null)
                        {
                            DetailActivity.OpenFragment(new NoConnectionFragment(), Activity);
                            return;
                        }

                        foreach (var t in histoData)
                        {
                            ohlcData.Add(new OhlcDataModel(t.Date, t.Price, 0, 0, 0));
                        }
                    }

                }
            }
            catch (Exception)
            {
                DetailActivity.OpenFragment(new NoConnectionFragment(), Activity);
            }


        }

        private double GetMin()
        {
            if(ohlcData!=null||ohlcData.Count!=0)
             return Convert.ToDouble(ohlcData.Min(x => x.Close));
            return 0;
        }
    

        public void InitChart()
        {
            sfChart.Series.Clear();
            sfChart.Behaviors.Clear();
            DateTimeCategoryAxis dateTimeAxis = new DateTimeCategoryAxis();
            dateTimeAxis.IntervalType =DateTimeIntervalType.Auto;
            dateTimeAxis.TrackballLabelStyle.LabelAlignment = ChartLabelAlignment.Near;
            dateTimeAxis.LabelStyle.LabelFormat = "dd/MMM HH:mm";
            dateTimeAxis.ShowTrackballInfo = true;
            NumericalAxis numericalAxis = new NumericalAxis()
            {
                AutoIntervalOnZoomingEnabled = true,
                RangePadding = NumericalPadding.Additional,
                ShowTrackballInfo = true,
                
            };
            numericalAxis.TrackballLabelStyle.LabelAlignment = ChartLabelAlignment.Far;


            sfChart.PrimaryAxis = dateTimeAxis;
            sfChart.SecondaryAxis = numericalAxis;
            CustomTrackBallBehavior trackballBehavior = new CustomTrackBallBehavior
            {
                ShowLabel = true,
                ShowLine = true,
                LabelDisplayMode = TrackballLabelDisplayMode.FloatAllPoints
            };
            trackballBehavior.LabelStyle.LabelFormat = "0.####";
            trackballBehavior.LabelDisplayMode = TrackballLabelDisplayMode.NearestPoint;
            trackballBehavior.MarkerStyle.MarkerType = MarkerType.Pentagon;
            
            ChartZoomPanBehavior zoomPan = new ChartZoomPanBehavior()
            {
                ScrollingEnabled = true,
                ZoomingEnabled = false,



            };
            sfChart.Behaviors.Add(trackballBehavior);
            sfChart.Behaviors.Add(zoomPan);
            
        }


        private void TypeSegmentedControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    {
                        sfChart.Series.Clear();
                        _series = new AreaSeries()
                        {
                            ItemsSource = ohlcData,
                            
                            Color = new Color(colorData[0]),
                            XBindingPath = "Date",
                            YBindingPath = "Close"
                        };
                        sfChart.Series.Add(_series);
                        break;
                    }
                case 1:
                    {
                        sfChart.Series.Clear();
                        var series = new HiLoOpenCloseSeries()
                        {
                            ItemsSource = ohlcData,
                            XBindingPath = "Date",
                            Open = "Open",
                            High = "High",
                            Low = "Low",
                            Close = "Close",
                            Name = "OHLC",
                            EnableAnimation = false,

                        };
                        
                        sfChart.Series.Add(series);
                        break;
                    }

            }
        }

        private async void SegmentedChartTimeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (e.Index)
            {
                case 0:
                    {
                        await GetHitoricalData(minuteDel, "USD", 60);
                        RefreshChartSeries();

                        break;
                    }
                case 1:
                    {
                        await GetHitoricalData(hourDel, "USD", 24);

                        RefreshChartSeries();
                        break;
                    }
                case 2:
                    {
                        await GetHitoricalData(dayDel, "USD", 7);
                        RefreshChartSeries();
                        break;
                    }
                case 3:
                    {
                        await GetHitoricalData(dayDel, "USD", 360);
                        RefreshChartSeries();
                        break;
                    }
                case 4:
                    {
                        await GetHitoricalData(dayDel, "USD", null, true);
                        RefreshChartSeries();
                        break;
                    }
            }
        }

        private void RefreshChartSeries()
        {
            sfChart.Series[0].ItemsSource = ohlcData;
            ((NumericalAxis)sfChart.SecondaryAxis).Minimum = GetMin();
            sfChart.RedrawChart();
        }
        private void ConfigureSegment(View view)
        {
            try
            {
                var typedValue = new Android.Util.TypedValue();
                this.Activity.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, typedValue, true);
                var colorObject = Application.Context.GetDrawable(typedValue.ResourceId);
                var colorPrimary = ((Android.Graphics.Drawables.ColorDrawable)colorObject).Color;
                segmentedChartTime.SelectionChanged += SegmentedChartTimeSelectionChanged;
                segmentedChartTime.FontColor = Color.Black;
                segmentedChartTime.BorderColor = new Color(colorPrimary);
                segmentedChartTime.VisibleSegmentsCount = 5;
                segmentedChartTime.DisplayMode = SegmentDisplayMode.Text;
                string[] items = new string[5] { "1 Hour", "1 Day", "1 Week", "1 Year", "All time" };
                segmentedChartTime.ItemsSource = items;
                segmentedChartType.Visibility = ViewStates.Visible;
                segmentedChartType.DisplayMode = SegmentDisplayMode.Image;
                ImageView[] images = new ImageView[2];
                images[0] = new ImageView(this.Activity);
                images[0].SetImageResource(Resource.Drawable.ic_timeline_yellow_dark_24dp);
                images[1] = new ImageView(this.Activity);
                images[1].SetImageResource(Resource.Drawable.candlestick_icon);
                //segmentedChartTime.SelectionTextColor = new Color(vibrantColor);
                segmentedChartType.BorderColor = new Color(colorPrimary);
                segmentedChartType.FontColor = new Color(colorPrimary);
                segmentedChartType.VisibleSegmentsCount = 2;
                segmentedChartType.ItemsSource = images;
                segmentedChartType.SelectionChanged += TypeSegmentedControl_SelectionChanged;
                segmentedChartTime.SelectedIndex = 2;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }
  
}