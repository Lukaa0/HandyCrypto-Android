using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using HandyCrypto.Fragments;
using HandyCrypto.View_Holders;

namespace HandyCrypto.Model
{
    public class CustomTrackBallBehavior : ChartTrackballBehavior
    {
        protected override View GetView(ChartSeries series, object data, int index)
        {
            return base.GetView(series, data, index);


        }
        protected override void OnLongPress(float x, float y)
        {
            base.OnLongPress(x, y);
        }

        protected override void OnTouchDown(float x, float y)
        {
            OnLongPress(x, y);
        }

        protected override void OnTouchUp(float x, float y)
        {

        }

        protected override void OnTouchMove(float x, float y)
        {
            base.OnTouchMove(x, y);
            Show(x, y);
        }


       
        
    }
    public class TrackballEventArgs : EventArgs
    {
        public object Data{ get; set; }
        public TrackballEventArgs(object data)
        {
            Data = data;
        }
    }
}