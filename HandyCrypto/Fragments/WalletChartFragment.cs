using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using Syncfusion.Android.Buttons;

namespace HandyCrypto.Fragments
{
    public class WalletChartFragment : Android.Support.V4.App.Fragment
    {
        private SfSegmentedControl _segmentedChartTime;
        private SfSegmentedControl _segmentedChartType;
        private SfChart _sfChart;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.chart_detail_layout, container, false);
            _segmentedChartTime = view.FindViewById<SfSegmentedControl>(Resource.Id.segment_chartType);
            _segmentedChartType = view.FindViewById<SfSegmentedControl>(Resource.Id.segment_chartTime);
            _sfChart = view.FindViewById<SfChart>(Resource.Id.detail_chart);
            return view;
        }
        private void ConfigureSegment(View view)
        {
            try
            {
                var typedValue = new Android.Util.TypedValue();
                this.Activity.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, typedValue, true);
                var colorObject = Application.Context.GetDrawable(typedValue.ResourceId);
                var colorPrimary = ((Android.Graphics.Drawables.ColorDrawable)colorObject).Color;
               // _segmentedChartTime.SelectionChanged += SegmentedChartTimeSelectionChanged;
                _segmentedChartTime.FontColor = Color.Black;
                _segmentedChartTime.BorderColor = new Color(colorPrimary);
                _segmentedChartTime.VisibleSegmentsCount = 5;
                _segmentedChartTime.DisplayMode = SegmentDisplayMode.Text;
                string[] items = new string[5] { "1 Hour", "1 Day", "1 Week", "1 Year", "All time" };
                _segmentedChartTime.ItemsSource = items;
                _segmentedChartType.Visibility = ViewStates.Visible;
                _segmentedChartType.DisplayMode = SegmentDisplayMode.Image;
                ImageView[] images = new ImageView[2];
                images[0] = new ImageView(this.Activity);
                images[0].SetImageResource(Resource.Drawable.ic_timeline_yellow_dark_24dp);
                images[1] = new ImageView(this.Activity);
                images[1].SetImageResource(Resource.Drawable.candlestick_icon);
                //segmentedChartTime.SelectionTextColor = new Color(vibrantColor);
                _segmentedChartType.BorderColor = new Color(colorPrimary);
                _segmentedChartType.FontColor = new Color(colorPrimary);
                _segmentedChartType.VisibleSegmentsCount = 2;
                _segmentedChartType.ItemsSource = images;
                //_segmentedChartType.SelectionChanged += TypeSegmentedControl_SelectionChanged;
                _segmentedChartTime.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }

    }
}