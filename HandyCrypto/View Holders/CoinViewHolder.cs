using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace HandyCrypto.View_Holders
{
    public class CoinViewHolder : RecyclerView.ViewHolder
    {
        public View mainView { get; set; }
        public TextView Title { get; set; }
        public TextView PriceText { get; set; }
        public ImageView CoinImage { get; set; }
        public ToggleButton Favorite{ get; set; }
        public CoinViewHolder(View view, Action<int>viewClickListener,Action<int,ToggleButton> favoriteClickListener) : base(view)
        {
            Title = view.FindViewById<TextView>(Resource.Id.Title);
            mainView = view;
            PriceText = view.FindViewById<TextView>(Resource.Id.Description);
            CoinImage = view.FindViewById<ImageView>(Resource.Id.Thumbnail);
            Favorite = view.FindViewById<ToggleButton>(Resource.Id.favButton);
            view.Click += (sender, e) => viewClickListener(base.LayoutPosition);
            Favorite.Click += (sender, e) => favoriteClickListener(base.LayoutPosition,Favorite);
        }

     
    }
}