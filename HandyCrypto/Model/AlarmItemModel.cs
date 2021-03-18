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
using SQLite;

namespace HandyCrypto.Model
{
    public class AlarmItemModel
    {
        public AlarmItemModel(double value, int typeId,string symbol)
        {
            Value = value;
            TypeId = typeId;
            Symbol = symbol;
        }
        public AlarmItemModel()
        {

        }

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public double Value { get; set; }
        public string Symbol { get; set; }
        public int TypeId { get; set; }

    }
}