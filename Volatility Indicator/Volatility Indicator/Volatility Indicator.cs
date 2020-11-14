using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class VolatilityIndicator : Indicator
    {
        [Parameter("Period", DefaultValue = 10)]
        public int Period { get; set; }

        [Output("Volatility", LineColor = "Green", IsHistogram = false, LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries Volatility { get; set; }

        [Output("MidLine", LineColor = "Gray", LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries MidLine { get; set; }

        private double timeInSeconds;

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
            int lastIndex = (index - Period + 1) < 0 ? 0 : (index - Period + 1);
            var volatility = 0.0;
            for (int i = index; i >= lastIndex; i--)
            {
                var time = Bars.LastBar.OpenTime == Bars[i].OpenTime ? (this.Server.Time - Bars.LastBar.OpenTime).TotalSeconds : (Bars[i + 1].OpenTime - Bars[i].OpenTime).TotalSeconds;

                volatility += Math.Abs(this.Bars[i].Close - this.Bars[i].Open) / time;
            }
            this.Volatility[index] = volatility / 0.0001;
            this.MidLine[index] = 0.1;
        }
    }
}
