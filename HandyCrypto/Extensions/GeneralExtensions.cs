using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using HandyCrypto.Model;

namespace HandyCrypto.Extensions
{
    public static class GeneralExtensions
    {
        public static void Run(this Android.Support.V4.App.Fragment fragment, int resourceId, AppCompatActivity context, Bundle data = null, 
            bool addToBackStack = false , int startAnim = Resource.Animation.abc_popup_enter, int endAnim = Resource.Animation.popup_exit)
        {
            Android.Support.V4.App.FragmentTransaction ft = context.SupportFragmentManager.BeginTransaction();
            ft.SetCustomAnimations(startAnim,endAnim);
            if(data!=null)
                fragment.Arguments = data;
                
            ft.Replace(resourceId, fragment, fragment.GetType().Name);
            if (addToBackStack)
                ft.AddToBackStack(null);
            ft.Commit();
        }
        public static SfChart Initialize(this SfChart sfChart)
        {
            sfChart.Series.Clear();
            sfChart.Behaviors.Clear();
            DateTimeCategoryAxis dateTimeAxis = new DateTimeCategoryAxis();
            dateTimeAxis.IntervalType = DateTimeIntervalType.Auto;
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
            return sfChart;
        }
    }
}