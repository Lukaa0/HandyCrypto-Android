using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using PortableCryptoLibrary;
using System.Collections.Generic;
using CryptoCompare;
using Android.Content.Res;
using Android.Graphics;

namespace HandyCrypto.Adapters
{
    class DetailItemRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DetailItemRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<DetailItemRecyclerViewAdapterClickEventArgs> ItemLongClick;
        CoinFullAggregatedData item;

        public DetailItemRecyclerViewAdapter(CoinFullAggregatedData data)
        {
            item = data;
            
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.detail_page_item, parent, false);

            var vh = new DetailItemRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            

            // Replace the contents of the view with that element
            var holder = viewHolder as DetailItemRecyclerViewAdapterViewHolder;
            holder.Label.Text = item.GetType().GetProperties()[position].Name;
            holder.Value.Text = item.GetType().GetProperties()[position].GetValue(item).ToString();
            if (holder.Value.Text.Contains('-'))
                holder.Value.SetTextColor(Color.Red);
            if (position % 2 == 0)
                holder.Container.SetCardBackgroundColor(Color.Transparent);

        }

        public override int ItemCount => item.GetType().GetProperties().Length;

        void OnClick(DetailItemRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DetailItemRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class DetailItemRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Label { get; set; }
        public TextView Value { get; set; }
        public CardView Container { get; set; }




        public DetailItemRecyclerViewAdapterViewHolder(View view, Action<DetailItemRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<DetailItemRecyclerViewAdapterClickEventArgs> longClickListener) : base(view)
        {
            Label = view.FindViewById<TextView>(Resource.Id.detail_item_label);
            Value = view.FindViewById<TextView>(Resource.Id.detail_item_value);
            Container = view.FindViewById<CardView>(Resource.Id.detail_item_container);
            
            view.Click += (sender, e) => clickListener(new DetailItemRecyclerViewAdapterClickEventArgs { View = view, Position = AdapterPosition });
            view.LongClick += (sender, e) => longClickListener(new DetailItemRecyclerViewAdapterClickEventArgs { View = view, Position = AdapterPosition });
        }
    }

    public class DetailItemRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}