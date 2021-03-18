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
using Com.Syncfusion.Charts;
using Syncfusion.Android.Buttons;

namespace HandyCrypto.Builders
{
    public class ChartBuilder : IChartBuilder
    {
        private SfSegmentedControl _timeSegmentedView;
        private SfSegmentedControl _typeSegmentedView;
        private SfChart _chart;
        private ChartSeries _series;


        public IChartBuilder SetChartView(SfChart chart)
        {
            _chart = chart;
            return this;
        }

        public IChartBuilder SetSeries(ChartSeries series)
        {
            _series = series;
            return this;
        }

        public IChartBuilder SetTimeSegmentedView(SfSegmentedControl segmentedControl)
        {
            _timeSegmentedView = segmentedControl;
            return this;
        }

        public IChartBuilder SetTypeSegmentedView(SfSegmentedControl segmentedControl)
        {
            _typeSegmentedView = segmentedControl;
            return this;
        }
    }
}