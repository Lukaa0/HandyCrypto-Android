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
using HandyCrypto.Services;
using DialogFragment = Android.Support.V4.App.DialogFragment;
namespace HandyCrypto.Fragments
{
    public class BugReportingDialogFragment : DialogFragment
    {
        EditText _email;
        EditText _title;
        EditText _description;
        Button _sendBtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           //View view = inflater.Inflate(Resource.Layout.bug_reporting_layout, container, false);
           // _email = view.FindViewById<EditText>(Resource.Id.bug_email);
           // _title = view.FindViewById<EditText>(Resource.Id.bug_title);
           // _description = view.FindViewById<EditText>(Resource.Id.bug_description);
           // _sendBtn = view.FindViewById<Button>(Resource.Id.bug_send_btn);
            _sendBtn.Click += _sendBtn_Click;



            return new View(this.Activity);
        }

        private void _sendBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_title.Text) || string.IsNullOrEmpty(_description.Text))
            {
                Toast.MakeText(Activity, "Bug title or bug description is empty",ToastLength.Long).Show();
                return;
            }
            MailSendingService.Send(_title.Text, _description.Text, _email.Text);
            this.Dismiss();
        }
    }
}