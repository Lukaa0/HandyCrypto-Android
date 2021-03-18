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

namespace HandyCrypto.Model
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://handycryptonamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=+NYMFa3jJkrUfcRU+T/zEJsHoCimv8Tp52bNWucM0a8=";
        public const string NotificationHubName = "Endpoint=sb://handycryptonamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Ow4PawxgnoJBw5tdH5R3oKL0Ri8fWrOmxR56suT4QN4=";
    }
}