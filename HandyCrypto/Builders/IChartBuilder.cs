
using Com.Syncfusion.Charts;
using Syncfusion.Android.Buttons;

namespace HandyCrypto.Builders
{
    public interface IChartBuilder
    {
        IChartBuilder SetChartView(SfChart chart);
        IChartBuilder SetSeries(ChartSeries series);

        IChartBuilder SetTimeSegmentedView(SfSegmentedControl segmentedControl);
        IChartBuilder SetTypeSegmentedView(SfSegmentedControl segmentedControl);


    }
}