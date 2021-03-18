using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Orhanobut.Dialogplus;
using CryptoCompare;
using HandyCrypto.DataHelper;
using HandyCrypto.Fragments;
using HandyCrypto.Model;
using HandyCrypto.View_Holders;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using Square.Picasso;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace HandyCrypto.Adapters
{
    public class CoinRecyclerViewAdapter : RecyclerView.Adapter
    {
        private List<CryptoItemModel> Coins;
        private  Activity activity;
        FragmentManager fragmentManager;
        List<FavoriteCoin> favorites;
        private CryptoItemModel cryptoItem;
        private bool isOnFavorites;
        public CoinRecyclerViewAdapter(List<CryptoItemModel> coins,Activity act , FragmentManager supportFragmentManager,List<FavoriteCoin> favorites,bool isOnFavorites)
        {
            activity = act;
            Coins = coins;
            fragmentManager = supportFragmentManager;
            this.favorites = favorites;
            this.isOnFavorites = isOnFavorites;

        }

        public override int ItemCount => Coins.Count;
        public void AddAndRefresh(List<CryptoItemModel> data)
        {
            int startPosition = Coins.Count;
            Coins.AddRange(data);
            NotifyItemRangeInserted(startPosition,data.Count);
            
        }

        public void ReplaceAndRefresh(List<CryptoItemModel> data)
        {
            var itemCount = Coins.Count;
            Coins = null;
            NotifyItemRangeRemoved(0,itemCount);
            Coins = data;
            NotifyItemRangeInserted(0, Coins.Count);
        }
       

        public  void OnClick(int position)
        {
            var item = Coins[position];
            
            Bundle Data = new Bundle();
            Data.PutString("symbol", item.Info.Symbol);
            
            var view = LayoutInflater.From(activity).Inflate(Resource.Layout.item_modal_dialog, null);
            var dialog = new MaterialDialog.Builder(this.activity);
            dialog.CustomView(view, false);
            
   
            ItemModalDialogFragment itemModalDialogFragment = new ItemModalDialogFragment(view,item,activity);
            itemModalDialogFragment.Create();
            var dial = dialog.Build();
            itemModalDialogFragment.OnCloseClick += (sender, e) => dial.Dismiss();
            dial.Show();

        }
        private  void SetAnim(View view)
        {
            Animation animation = AnimationUtils.LoadAnimation(activity.BaseContext, Resource.Animation.cFlipRightIn);
            view.StartAnimation(animation);
        }
       
        public override    void  OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CoinViewHolder coinViewHolder)
            {
                cryptoItem = Coins[position];
                string img = string.Join("", Constant.BaseCoinUri, cryptoItem.Info.ImageUrl);
                Picasso.With(activity).Load(img).CenterCrop().Fit()
                    .Placeholder(Resource.Drawable.handycrypto_logo).Into(coinViewHolder.CoinImage);
                coinViewHolder.PriceText.Text = string.Join("", "$", Math.Round(cryptoItem.AggregatedData.Price.Value, 4).ToString());
                coinViewHolder.Title.Text = cryptoItem.Info.CoinName;
                coinViewHolder.Favorite.Checked = favorites.Any(x => x.Symbol == cryptoItem.Info.Symbol);


            }
            else
            {

                ProgressViewHolder vp = holder as ProgressViewHolder;
                vp.Progress.Visibility = ViewStates.Visible;
            }


            //SetAnim(coinViewHolder.ItemView);


        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override int GetItemViewType(int position)
        {
           
            if (position >= Coins.Count-1)
                return 0;
            else
                return 1;
            
          
        }
        public void Filter(List<CryptoItemModel> newList)
        {
            Coins = newList;
            Coins.AddRange(newList);
            NotifyDataSetChanged();
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            RecyclerView.ViewHolder viewHolder;
            if (viewType==1)
            {

                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItem, parent, false);
                viewHolder = new CoinViewHolder(view, OnClick, OnFavoriteClick);
                return viewHolder;
            }

            else
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.progressbar_layout, parent, false);
                viewHolder = new ProgressViewHolder(view);
                return viewHolder;

            }

        }

        private async void OnFavoriteClick(int position, ToggleButton item)
        {
            var crypto = Coins[position];
            if (item.Checked)
            {
                var favCoin = new FavoriteCoin(crypto.Info.Symbol);
                await LocalDbService<FavoriteCoin>.Instance.InsertAsync(favCoin);
                favorites.Add(favCoin);
                ((MainActivity)activity).FavoritesFragment.CoinRecyclerViewAdapter.Add(crypto);

            }
            else if(!item.Checked)
            {
                await DeleteItemAsync(crypto);
                await ((MainActivity)activity).FavoritesFragment.CoinRecyclerViewAdapter.DeleteItemAsync(crypto);
                favorites.Remove(new FavoriteCoin(crypto.Info.Symbol));

            }

        }
        public async Task Untoggle(string symbol)
        {

            var favItem = new FavoriteCoin(symbol);
            await LocalDbService<FavoriteCoin>.Instance.DeleteAsync(favItem);
            var item = favorites.FirstOrDefault(x => x.Symbol == symbol);
            if(item!=null)
                favorites.Remove(item);
            var position = Coins.FindIndex(x => x.Info.Symbol == symbol);
            NotifyItemChanged(position);
            

        }
        private async System.Threading.Tasks.Task DeleteItemAsync(CryptoItemModel item)
        {
            var match = favorites.FirstOrDefault(x => x.Symbol == item.Info.Symbol);
            if (match != null)
            {
                favorites.Remove(match);
                await LocalDbService<FavoriteCoin>.Instance.DeleteAsync(match);
            }
        }
    }

    public class ProgressViewHolder : RecyclerView.ViewHolder
    {
        public ProgressBar Progress { get; set; }
        public ProgressViewHolder(View itemView) : base(itemView)
        {
            Progress = itemView.FindViewById<ProgressBar>(Resource.Id.recyclerProgress);
        }
    }
}
