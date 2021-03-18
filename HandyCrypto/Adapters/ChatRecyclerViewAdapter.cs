using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Github.BubbleViewLib;
using HandyCrypto.Model;

namespace HandyCrypto.Adapters
{
    class ChatRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ChatRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ChatRecyclerViewAdapterClickEventArgs> ItemLongClick;
        private Activity mainActivity;
        private User user;
        private List<MessageContent> lstMessage;
        private RelativeLayout.LayoutParams lp;
        
        public ChatRecyclerViewAdapter(Activity mainActivity, List<MessageContent> lstMessage, User user)
        {
            this.mainActivity = mainActivity;
            this.lstMessage = lstMessage;
            lp = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.WrapContent,
                RelativeLayout.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            this.user = user;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.chat_list_item, parent, false);
            

            var vh = new ChatRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }
        public void AddMessage(MessageContent messageContent)
        {
            this.lstMessage.Add(messageContent);
            NotifyItemInserted(lstMessage.Count);
        }
      
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = lstMessage[position];

            var holder = viewHolder as ChatRecyclerViewAdapterViewHolder;
            holder.Username.Text = item.Username;
            holder.Content.Text = item.Message;
            if (item.Avatar == "man")
                holder.Avatar.SetBackgroundResource(Resource.Drawable.man);
            else
                holder.Avatar.SetBackgroundResource(Resource.Drawable.girl);
            if (item.Username == user.Username)
                holder.chatContainer.LayoutParameters = lp;
        }

        public override int ItemCount => lstMessage.Count;

        void OnClick(ChatRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ChatRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }
    

    public class ChatRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
       public TextView Username, Time, Content;
        public ImageView Avatar;
        public RelativeLayout chatContainer;
        public ChatRecyclerViewAdapterViewHolder(View itemView, Action<ChatRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ChatRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            chatContainer = ItemView.FindViewById<RelativeLayout>(Resource.Id.chat_main_container);
            Username = ItemView.FindViewById<TextView>(Resource.Id.chat_nickname);
            Content = ItemView.FindViewById<BubbleTextView>(Resource.Id.chat_bubbleview);
            Avatar = ItemView.FindViewById<ImageView>(Resource.Id.chat_image);
            itemView.Click += (sender, e) => clickListener(new ChatRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ChatRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ChatRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}