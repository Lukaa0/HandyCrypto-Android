using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using HandyCrypto.View_Holders;
using PortableCryptoLibrary;
using System.Text;
using Android.App;
using Square.Picasso;
using Android.Content;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using System.Threading.Tasks;
using System.Linq;
using Android.OS;
using AFollestad.MaterialDialogs;
using HandyCrypto.Fragments;

namespace HandyCrypto.Adapters
{
    public class FavoriteCoinRecyclerAdapter : RecyclerView.Adapter
    {
        List<CryptoItemModel> items;
        private string coinImage;
        private readonly Activity activity;

        public FavoriteCoinRecyclerAdapter(List<CryptoItemModel> data,Activity activity)
        {
            items = data;
            this.activity = activity;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItem, parent, false);

            var vh = new CoinViewHolder(itemView,OnClick,OnFavoriteClick);


            return vh;
        }

        private async void OnFavoriteClick(int position, ToggleButton toggleButton)
        {
            var crypto = items[position];
            var favoriteCoin = new FavoriteCoin(crypto.Info.Symbol);
            await DeleteItemAsync(favoriteCoin);
            items.Remove(crypto);
            NotifyItemRemoved(position);
        }
        private async System.Threading.Tasks.Task DeleteItemAsync(FavoriteCoin item)
        {
            if(item!=null)
            {
                 await LocalDbService<FavoriteCoin>.Instance.DeleteAsync(item);

                await ((MainActivity)activity).CoinsFragment.coinRecyclerViewAdapter.Untoggle(item.Symbol);
            }
        }
        public async Task DeleteItemAsync(CryptoItemModel item)
        {
            if (item != null&&items.Select(x=>x.Info.Symbol).Contains(item.Info.Symbol))
            {
                var favoriteCoin = new FavoriteCoin(item.Info.Symbol);
                await LocalDbService<FavoriteCoin>.Instance.DeleteAsync(favoriteCoin);
                NotifyItemRemoved(items.IndexOf(item));
                items.Remove(item);

            }
        }
        private void OnClick(int position)
        {
            var item = items[position];

            Bundle Data = new Bundle();
            Data.PutString("symbol", item.Info.Symbol);

            var view = LayoutInflater.From(activity).Inflate(Resource.Layout.item_modal_dialog, null);
            var dialog = new MaterialDialog.Builder(this.activity);
            dialog.CustomView(view, false);

            ItemModalDialogFragment itemModalDialogFragment = new ItemModalDialogFragment(view, item, activity);
            itemModalDialogFragment.Create();
            dialog.Show();
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            var holder = viewHolder as CoinViewHolder;
            coinImage = item.Info.ImageUrl;
            StringBuilder imageStringBuilder = new StringBuilder("https://www.cryptocompare.com");
            imageStringBuilder.Append(coinImage);
            var imageUrl = imageStringBuilder.ToString();
            Picasso.With(activity).Load(imageUrl).Into(holder.CoinImage);
            holder.PriceText.Text = item.AggregatedData.Price.ToString() ?? "Loading...";
            holder.Title.Text = item.Info.Name;
            holder.Favorite.Checked = true;
        }

        internal void ReplaceAndRefresh(List<CryptoItemModel> items)
        {
            int startPosition = items.Count + 1;
            this.items = items;
            NotifyItemRangeInserted(startPosition, items.Count);
        }
        public void Add(CryptoItemModel item)
        {
            items.Add(item);
            NotifyItemInserted(items.Count-1);
        }
        public override int ItemCount => items.Count;


    }
    


}