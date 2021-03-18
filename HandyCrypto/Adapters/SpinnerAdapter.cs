using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using Java.Lang;
using Square.Picasso;

namespace HandyCrypto.Adapters
{
    public  class SpinnerAdapter : BaseAdapter<CoinInfo>
    {
        LayoutInflater inflater;
        private Context context;
        public List<CoinInfo> cryptoItems { get; set; }
        public SpinnerAdapter(Context context, List<CoinInfo> CryptoItems)
        {
            this.context = context;
            this.cryptoItems = CryptoItems;

        }
        public override CoinInfo this[int position] => cryptoItems[position];

        public override int Count => cryptoItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }
        public CoinInfo GetCoin(int position)
        {
            return cryptoItems[position];
        }



        public override  View GetView(int position, View convertView, ViewGroup parent)
        {
            var cryptoItem = cryptoItems[position];
            //var imageData = cryptoImages.Data;
            //string imgurl;
            //if (imageData.ContainsKey(cryptoItem.symbol.ToUpper()))
            //{
            //    imgurl = cryptoImages.BaseImageUrl + imageData[cryptoItem.symbol.ToUpper()].ImageUrl;
            //}
            //else
            //{
            //    imgurl = cryptoImages.BaseImageUrl + imageData["BTC"].ImageUrl;
            //}
            string BaseUrl,CoinUrl;
            BaseUrl = "https://www.cryptocompare.com";
            CoinUrl = cryptoItem.ImageUrl;
            string currentImageUrl =BaseUrl + CoinUrl;


            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.spinner_item, null);
            }
            
            convertView.FindViewById<TextView>(Resource.Id.spinnerCoinText).Text = cryptoItem.CoinName;
           // convertView.FindViewById<TextView>(Resource.Id.Description).Text = "$" + cryptoItem.price_usd;
            var imgView = convertView.FindViewById<ImageView>(Resource.Id.spinnerCoinImage);
            Picasso.With(context).Load(currentImageUrl).Into(imgView);
            return convertView;
        }
    }
}