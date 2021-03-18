using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using PortableCryptoLibrary;

namespace HandyCrypto.Fragments
{
    public class AlarmDialogFragment : Android.Support.V4.App.DialogFragment, IVerifyNightMode
    {
        Spinner coinsSpinner;
        Spinner typeSpinner;
        EditText amount;
        Button confirmBtn;
        string symbol;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            symbol = Arguments.GetString("alarm_symbol");
            // Create your fragment here
        }
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.alarm_layout, container, false);
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            coinsSpinner = view.FindViewById<Spinner>(Resource.Id.alarm_spinner_coin);
            typeSpinner = view.FindViewById<Spinner>(Resource.Id.alarm_spinner_type);
            confirmBtn = view.FindViewById<Button>(Resource.Id.alarm_confirm_btn);
            confirmBtn.Click += ConfirmBtn_Click;
            amount = view.FindViewById<EditText>(Resource.Id.alarm_edittext);
            ThemeValidator.VerifyNightMode(Dialog.Window);
            return view;
        }

        

        private async void ConfirmBtn_Click(object sender, EventArgs e)
        {
            var model = new AlarmItemModel(Convert.ToDouble(amount.Text), typeSpinner.SelectedItemPosition,symbol);
            await LocalDbService<AlarmItemModel>.Instance.InsertAsync(model);
            this.Dismiss();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var coinsList = HandyCryptoClient.Instance.GeneralCoinInfo.Coins.Select(x => x.Symbol).ToArray();
            var position = HandyCryptoClient.Instance.GeneralCoinInfo.Coins.ToList().FindIndex(x => x.Symbol == symbol);
            var arrayCoinAdapter = new ArrayAdapter<string>(this.Activity,Resource.Layout.support_simple_spinner_dropdown_item,coinsList);
            coinsSpinner.Adapter = arrayCoinAdapter;
            coinsSpinner.SetSelection(position);
            var arrayTypeAdapter = new ArrayAdapter<string>(this.Activity, Resource.Layout.support_simple_spinner_dropdown_item, new[] {"more than","less than" });
            typeSpinner.Adapter = arrayTypeAdapter;
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}