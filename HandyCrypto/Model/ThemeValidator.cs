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
using Xamarin.Essentials;

namespace HandyCrypto.Model
{
    public static class ThemeValidator
    {
        public static void VerifyNightMode(Window window)
        {
            if (IsOnNightMode())
            {
                window.SetBackgroundDrawableResource(Resource.Color.backgroundNight);
            }
        }
        public static bool IsOnNightMode() => Preferences.Get("is_on_night_node", false);
        public static void ValidateTheme(Context context)
        {
           // if (Preferences.Get("is_on_night_mode", false))
                //context.SetTheme(Resource.Style.AppThemeDark);
            //else
                context.SetTheme(Resource.Style.AppTheme);
        }
        public static void ValidateRecreation(Activity context, int themeId)
        {
            //if (themeId == Resource.Style.AppThemeDark && !Preferences.Get("is_on_night_mode", false))
               // context.Recreate();
        }
        public static void ValidateStyle(Android.Support.V4.App.DialogFragment context)
        {
            //if (Preferences.Get("is_on_night_mode", false))
                //context.SetStyle(1, Resource.Style.AppThemeDark);
            //else
                context.SetStyle(1, Resource.Style.AppTheme);
        }
    }
}