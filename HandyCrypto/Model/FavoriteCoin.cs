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
    public class FavoriteCoin
    {
        [PrimaryKey]
        public string Symbol { get; set; }
       

        public FavoriteCoin()
        {
                
        }



        public FavoriteCoin(string symbol)
        {
            Symbol = symbol;
        }
    }
}