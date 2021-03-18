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

namespace HandyCrypto.DataHelper
{
    public static class LocalDbService<T> where T : new()
    {

        private static SQLiteClient<T> _instance;

        public static SQLiteClient<T> Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SQLiteClient<T>();
                return _instance;
            }
        }

    }
}