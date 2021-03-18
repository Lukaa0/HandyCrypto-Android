using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HandyCrypto.Model;
using Xamarin.Essentials;

namespace HandyCrypto.Activities
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private SwitchCompat _nightModeSwitch;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ThemeValidator.ValidateTheme(this);
            SetContentView(Resource.Layout.settings_layout);
            _nightModeSwitch = FindViewById<SwitchCompat>(Resource.Id.night_mode_switch);
            _nightModeSwitch.Checked = Preferences.Get("is_on_night_mode", false);
            _nightModeSwitch.CheckedChange += _nightModeSwitch_CheckedChange;
            // Create your application here
        }

        private void _nightModeSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
                Preferences.Set("is_on_night_mode", true);
            else
                Preferences.Set("is_on_night_mode", false);
            StartActivity(typeof(MainActivity));
            this.FinishAffinity();
        }
    }
}