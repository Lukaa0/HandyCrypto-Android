using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.Activities;
using HandyCrypto.Adapters;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using Java.Util;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using Wdullaer.MaterialDateTimePicker.Date;
using static Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog;
using Calendar = Java.Util.Calendar;
using DatePickerDialog = Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog;

namespace HandyCrypto.Fragments
{
    public class WalletDialogFragment : Android.Support.V4.App.DialogFragment, IOnDateSetListener
    {
       List<CoinInfo> cryptoCoins;
        Spinner spinner;
        TextView dateText;
        SpinnerAdapter adapter;
        Button timeButton;
        Button confirmBtn;
        Button cancelBtn;
        DateTime investmentDate;
        EditText investment;
        private View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            view = inflater.Inflate(Resource.Layout.wallet_dialog, container, false);
            Init();
            

            return view;
        }
        
        public override async void OnResume()
        {
            base.OnResume();
            cryptoCoins = await LocalDbService<CoinInfo>.Instance.SelectItems();
            int index = 0;
            if (Arguments !=null&&Arguments.GetString("cryptoDetails") != null)
            {
                var cryptoItem = JsonConvert.DeserializeObject<CryptoItemModel>(Arguments.GetString("cryptoDetails"));
                index = cryptoCoins.FindIndex(x => x.Symbol == cryptoItem.Info.Symbol);

            }
            
            adapter = new SpinnerAdapter(view.Context, cryptoCoins);
            spinner.Adapter = adapter;
            spinner.SetSelection(index);
            adapter?.NotifyDataSetChanged();
        }
      
        private  void Init()
        {
            
            timeButton = view.FindViewById<Button>(Resource.Id.timeBtn);
            investment = view.FindViewById<EditText>(Resource.Id.boughtEditTxt);
            dateText = view.FindViewById<TextView>(Resource.Id.wallet_dialog_date_label);
            confirmBtn = view.FindViewById<Button>(Resource.Id.submitBtn);
            cancelBtn = view.FindViewById<Button>(Resource.Id.cancelbtn);
            cancelBtn.Click += CancelBtn_Click;
            timeButton.Click += TimeButton_Click;
            confirmBtn.Click += ConfirmBtn_Click;
            spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            
        }
    
     

        private async void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (investmentDate == null || investment.Text == string.Empty)
            {
                Toast.MakeText(Activity, "Missing fields", ToastLength.Long).Show();
                return;
            }
            var symbol = adapter.GetCoin(spinner.SelectedItemPosition).Symbol;
            var basePrice =
                await HandyCryptoClient.Instance.GeneralCoinInfo.GetHistoricalPrice(symbol, new[] {"USD"},
                    investmentDate.AddDays(1));
            
            Wallet newWallet = new Wallet(symbol, decimal.Parse(investment.Text), investmentDate.ToString(CultureInfo.InvariantCulture))
            {
                CoinPrice = basePrice
            };


            await LocalDbService<Wallet>.Instance.InsertAsync(newWallet);

            Intent intent = new Intent(this.Activity, typeof(WalletActivity));
            intent.SetFlags(ActivityFlags.NoHistory);
            Activity.Finish();
            this.StartActivity(intent);
        }
                
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        public void OnDateSet(Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog view, int year, int monthOfYear, int dayOfMonth)
        {
            investmentDate = new DateTime(year, monthOfYear+1, dayOfMonth);
            dateText.Text = investmentDate.ToString();
        }



        private void TimeButton_Click(object sender, EventArgs e)
        {
            Calendar now = Calendar.Instance;
            DatePickerDialog datepicker = NewInstance(this,
                now.Get(CalendarField.Year),
                now.Get(CalendarField.Month),
                now.Get(CalendarField.DayOfMonth));
            datepicker.MaxDate = Calendar.Instance;
            datepicker.SetTitle("Enter the date ");
            datepicker.Show(Activity.FragmentManager, "Date");
        }
    }
    }