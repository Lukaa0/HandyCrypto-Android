using System;

namespace HandyCrypto.Model
{
    public class BaseHistoricalModel
    {
        public DateTime Date { get; set; }

        public BaseHistoricalModel(DateTime date)
        {
            this.Date = date;
        }
    }
}