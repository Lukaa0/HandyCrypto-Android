using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Clans.Fab;
using Com.Orhanobut.Dialogplus;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using HandyCrypto.Adapters;
using HandyCrypto.Model;
using HandyCrypto.DataHelper;
using AFollestad.MaterialDialogs;
using System.Threading.Tasks;
using HandyCrypto.Fragments;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Animation;

namespace HandyCrypto.Activities
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : AppCompatActivity,IValueEventListener, IDialogInterfaceOnDismissListener
    {
        private List<MessageContent> lstMessage = new List<MessageContent>();
        private RecyclerView lstChat;
        private EditText edtChat;
        private Toolbar _toolbar;
        private Button fab;
        private ChatRecyclerViewAdapter adapter;
        private User _user;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ThemeValidator.ValidateTheme(this);
            SetContentView(Resource.Layout.chat_main_layout);
            _toolbar = FindViewById<Toolbar>(Resource.Id.toolbarChat);
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            _user = await LocalDbService<User>.Instance.GetFirst();
            if (_user == null)
            {
                var profileDialog = new ProfileDialogFragment();
                Android.Support.V4.App.FragmentTransaction dialogTransaction = SupportFragmentManager.BeginTransaction();
                profileDialog.Show(dialogTransaction, "profile-chat");

            }
            else
            {
                await NextPhase();
                


            }


        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId != Resource.Id.home)
                return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }

        private async Task NextPhase()
        {
            
            fab = FindViewById<Button>(Resource.Id.fabulous);
            edtChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<RecyclerView>(Resource.Id.list_of_messages);
            adapter = new ChatRecyclerViewAdapter(this, lstMessage,
                _user);
            lstChat.SetLayoutManager(new LinearLayoutManager(this));
            var animation = AnimationUtils.FastOutSlowInInterpolator;
            
            
            fab.Click += delegate { PostMessage(); };
            await DisplayChatMessages();
            HandyFirebaseClient.Instance.FirebaseDatabase.GetReference("test").AddValueEventListener(this);
        }

       

        private async void PostMessage()
        {
            var items = await HandyFirebaseClient.Instance.MainClient.Child("test").PostAsync(
                new MessageContent(_user.Username, edtChat.Text,_user.AvatarId));
            edtChat.Text = "";
            lstChat.SmoothScrollToPosition(adapter.ItemCount - 1);


        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandyFirebaseClient.Instance.FirebaseDatabase.GetReference("test").RemoveEventListener(this);

        }
        public void OnCancelled(DatabaseError error)
        {

        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            MessageContent model = new MessageContent();
            var obj = snapshot.Children;

            foreach (DataSnapshot snapshotChild in obj.ToEnumerable())
            {
                if (snapshotChild.GetValue(true) == null) continue;

                model.Message = snapshotChild.Child("Message")?.GetValue(true)?.ToString();
                model.Username = snapshotChild.Child("Username")?.GetValue(true)?.ToString();
                model.Avatar = snapshotChild.Child("Avatar")?.GetValue(true)?.ToString();

            }
            this.adapter.AddMessage(model);
            lstChat.SetAdapter(adapter);
            lstChat.SmoothScrollToPosition(adapter.ItemCount - 1);
        }

        private async Task DisplayChatMessages()
        {
            lstMessage.Clear();

            var data = await HandyFirebaseClient.Instance.MainClient.Child("test").OrderByKey().LimitToLast(15)
                .OnceAsync<MessageContent>();
            var items = data.OrderBy(x => x.Object.Time).ToList();
            if (items == null)
            {
                lstChat.SetAdapter(adapter);
                return;
            }
            if(items.Count>1)
                items.RemoveAt(items.Count -1);
            
            foreach (var item in items)
                adapter.AddMessage(item.Object);

            lstChat.SetAdapter(adapter);
            lstChat.SmoothScrollToPosition(adapter.ItemCount - 1);
        }

        public async void OnDismiss(IDialogInterface dialog)
        {
            _user = await LocalDbService<User>.Instance.GetFirst();
            await NextPhase();
        }
    }
    }
