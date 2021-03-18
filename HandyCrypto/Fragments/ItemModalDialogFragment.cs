using System;
using System.Runtime.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Graphics;
using Android.Views;
using Android.Widget;
using CryptoCompare;
using HandyCrypto.Model;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using Square.Picasso;

namespace HandyCrypto.Fragments
{
    public class ItemModalDialogFragment: Java.Lang.Object,ICallback
    {
        #region VARIABLES
        private ImageView Image;
        private Button detailBtn;
        private Button closeBtn;
        private TextView priceTxt;
        private TextView percentChangeTxt;
        private RelativeLayout Header;
        private Bitmap bitmap;
        private int vibrantColor;
        private int mutedColor;
        int defaultcolor = Color.ParseColor("#03A9F4");
        private int darkVibrantColor;
        private int lightVibrantColor;
        private int mutedDarkColor;
        private int mutedLightColor;
        private CryptoItemModel cryptoItem;
        private Context context;
        private string imgUrl;
        private View view;
        public event EventHandler OnCloseClick;

        #endregion

        public ItemModalDialogFragment(View view, CryptoItemModel cryptoItem,Context context)
        {
            this.view = view;
            this.cryptoItem = cryptoItem;
            this.context = context;
        }
        public void Create()
        {
            imgUrl = string.Concat(Constant.BaseCoinUri, cryptoItem.Info.ImageUrl);
            Init(view);
            
            FindViews(view);
            Setvalues();
            Events();
            Picasso.With(this.context).Load(imgUrl).Error(Resource.Drawable.error_img).Into(Image, this);
        }
        private void Events()
        {
            detailBtn.Click += DetailBtn_Click;
            closeBtn.Click += (sender, e) => OnCloseClick?.Invoke(this,EventArgs.Empty);
        }
        private void FindViews(View view)
        {
            priceTxt = view.FindViewById<TextView>(Resource.Id.modalPriceTxt);
            percentChangeTxt = view.FindViewById<TextView>(Resource.Id.modalPercentChange);
        }
        private void Setvalues()
        {
            if (cryptoItem == null)
                return;
            priceTxt.Text = $"Price : {cryptoItem.AggregatedData.Price}";
            percentChangeTxt.Text = $"PCT Change: {Math.Round(cryptoItem.AggregatedData.ChangePCT24Hour.Value,4)}";
        }
        private void DetailBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this.context, typeof(DetailActivity));
            intent.PutExtra("cryptoItem", cryptoItem.Info.Id);
            
            context.StartActivity(intent);
        }

        private void Init(View view)
        {
            Image = view.FindViewById<ImageView>(Resource.Id.modal_image);
            Header = view.FindViewById<RelativeLayout>(Resource.Id.header_modal);
            detailBtn = view.FindViewById<Button>(Resource.Id.detailBtn);
            closeBtn = view.FindViewById<Button>(Resource.Id.modal_closeBtn);

            // gotoWalletBtn = view.FindViewById<Button>(Resource.Id.addWalletBtn);

        }

        private void SetPalette()
        {
            if (Image?.Drawable != null)
            {
                BitmapDrawable bitmapDrawable = (BitmapDrawable)Image.Drawable;
                bitmap = bitmapDrawable.Bitmap;
                var palette = new Palette.Builder(bitmap).Generate();
                PalleteInit(palette);
            }
        }
        public void PalleteInit(Palette palette)
        {
            vibrantColor = palette.GetVibrantColor(defaultcolor);
            mutedColor = palette.GetMutedColor(defaultcolor);
            darkVibrantColor = palette.GetDarkVibrantColor(defaultcolor);
            lightVibrantColor = palette.GetLightVibrantColor(defaultcolor);
            mutedDarkColor = palette.GetDarkMutedColor(defaultcolor);
            mutedLightColor = palette.GetLightMutedColor(defaultcolor);


        }

        public void OnError()
        {
        }

        public void OnSuccess()
        {
            SetPalette();
            Header.SetBackgroundColor(new Color(vibrantColor));
        }

        public new void Dispose()
        {
            Image.Dispose();
        }
    }
}