using System;

namespace HandyCrypto.Model
{
    public class HistoricalDataModel:BaseHistoricalModel
    {

        public HistoricalDataModel(DateTime date, decimal price) : base(date)
        {
            Date = date;
            Price = price;
        }


        public decimal Price { get; set; }
    }
}