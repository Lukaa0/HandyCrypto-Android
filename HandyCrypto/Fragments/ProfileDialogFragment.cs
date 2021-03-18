using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Flexbox;
using HandyCrypto.Activities;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;

namespace HandyCrypto.Fragments
{
    public class ProfileDialogFragment : Android.Support.V4.App.DialogFragment,View.IOnClickListener
    {
        FlexboxLayout flexContainer;
        TextInputEditText username;
        TextInputEditText eMail;
        Button saveBtn;
        string selectedAvatarId = "man";

        public void OnClick(View v)
        {
            int avatarId = Resource.Drawable.man;
            int.TryParse(v.Tag.ToString(), out avatarId);
            if (avatarId == Resource.Drawable.girl)
                selectedAvatarId = "girl";
            ClearBackgrounds();
            v.SetBackgroundColor(Color.Green);
            
            

        }

        private void View_Click(object sender, EventArgs e)
        {
            
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if(Activity is ChatActivity)
            {
                ((IDialogInterfaceOnDismissListener)Activity).OnDismiss(dialog);
            }
        }
        public void ClearBackgrounds()
        {
            for (int i = 0; i < flexContainer.ChildCount; i++)
            {
                var v = flexContainer.GetChildAt(i);
                v.SetBackgroundColor(Color.Transparent);
            }
        }
    public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.profile_conf_layout, container, false);
            flexContainer = view.FindViewById<FlexboxLayout>(Resource.Id.flexboxlayout);
            username = view.FindViewById<TextInputEditText>(Resource.Id.profile_username);
            eMail = view.FindViewById<TextInputEditText>(Resource.Id.profile_email);
            saveBtn = view.FindViewById<Button>(Resource.Id.profile_save_btn);
            SetClickListeners();
            saveBtn.Click += SaveBtn_Click;
            return view;
        }
        private void SetClickListeners()
        {
            for (int i = 0; i < flexContainer.ChildCount; i++)
            {
                var view = flexContainer.GetChildAt(i);
                switch (i)
                {
                    case 0:
                        {
                            view.Tag = Resource.Drawable.man;
                                break;
                        }
                   
                    case 1:
                        {
                            view.Tag = Resource.Drawable.girl;
                                break;
                        }
                
                    default:
                        {
                            view.Tag = Resource.Drawable.man;
                            break;
                        }
                }
                view.SetOnClickListener(this);
            }
        }
        private async void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                Toast.MakeText(Activity, "Missing username", ToastLength.Short).Show();
                return;
            }
            User user = new User(username.Text, selectedAvatarId);
            await LocalDbService<User>.Instance.DeleteAll();
            await LocalDbService<User>.Instance.InsertAsync(user);
            
            this.Dismiss();
        }
    }
}