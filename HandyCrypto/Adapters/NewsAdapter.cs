using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using CryptoCompare;
using Square.Picasso;

namespace HandyCrypto.Adapters
{
    class NewsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<NewsAdapterClickEventArgs> ItemLongClick;
        private Context context;
        NewsEntity[] items;

        public NewsAdapter(NewsEntity[] data,Context context)
        {
            items = data;
            this.context = context;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.news_item_layout, parent, false);

            var vh = new NewsAdapterViewHolder(itemView, OnClick, OnLongClick);


            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            var holder = viewHolder as NewsAdapterViewHolder;
            holder.Title.Text = item.Title;
            Picasso.With(context).Load(item.ImageUrl).Into(holder.Image);
            holder.Body.Text = item.Body;
            
        }

        public override int ItemCount => items.Length;

        void OnClick(NewsAdapterClickEventArgs args)
        {
            var item = items[args.Position];
            ((Activity)context).Finish();
            context.StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse(item.Url)));
            
        }
        void OnLongClick(NewsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class NewsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Body { get; set; }
        public ImageView Image { get; set; }


        public NewsAdapterViewHolder(View itemView, Action<NewsAdapterClickEventArgs> clickListener,
                            Action<NewsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.newsTitle);
            Body = itemView.FindViewById<TextView>(Resource.Id.newsBody);
            Image = itemView.FindViewById<ImageView>(Resource.Id.newsImage);
            itemView.Click += (sender, e) => clickListener(new NewsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new NewsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class NewsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}