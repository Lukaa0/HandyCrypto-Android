using System;
using HandyCrypto.Model;

namespace HandyCrypto
{
    internal class OhlcDataModel : BaseHistoricalModel
    {
        public decimal Close { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }

        public OhlcDataModel(DateTime date,decimal close, decimal open, decimal high, decimal low) : base(date)
        {
            this.Close = close;
            this.Open= open;
            this.Low= low;
            this.High = high;
            Date = date;
        }
    }
}