using System;
using System.Globalization;
using CryptoCompare;
using Calendar = Java.Util.Calendar;

namespace HandyCrypto.Model
{
    public static class AndroidExtensionMethods
    {
       
        public static Calendar ConvertToCalendar(this DateTime date)
        {
            Calendar calendar = Calendar.Instance;
            calendar.Set(date.Year, date.Month - 1, date.Day, date.Hour, date.Minute, date.Second);
            return calendar;
        }
        
        public static void RemoveDeficientData(this CoinInfo coins)
        {

        }

    }
}