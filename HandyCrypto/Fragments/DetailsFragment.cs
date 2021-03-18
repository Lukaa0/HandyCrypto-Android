using Android;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HandyCrypto.Activities;
using HandyCrypto.Adapters;
using HandyCrypto.Model;
using Newtonsoft.Json;
using PortableCryptoLibrary;
using Square.Picasso;

namespace HandyCrypto.Fragments
{
    public class DetailsFragment : Android.Support.V4.App.Fragment
    {
        
        private CryptoItemModel cryptoItem;
        private int[] colorData;
        private RecyclerView recyclerView;
        private LinearLayout mainContainer;
        private ImageView headerImage;
        private TextView headerTitle;
        private DetailItemRecyclerViewAdapter adapter;


        public override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cryptoItem = Arguments.GetString("cryptoDetails")==null?null:JsonConvert.DeserializeObject<CryptoItemModel>(Arguments.GetString("cryptoDetails"));
            colorData = Arguments.GetIntArray("color_data");
            

            // Create your fragment here
        }
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            
            View view = inflater.Inflate(Resource.Layout.detail_page_items_layout, container, false);
            if (cryptoItem == null)
                DetailActivity.OpenFragment(new NoConnectionFragment(),Activity);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.detail_page_recycler_view);
            mainContainer = view.FindViewById<LinearLayout>(Resource.Id.detail_page_main_container);
            headerImage = view.FindViewById<ImageView>(Resource.Id.profile_image);
            headerTitle = view.FindViewById<TextView>(Resource.Id.detail_header_title);
            Picasso.With(Context).Load(Constant.BaseCoinUri + cryptoItem.Info.ImageUrl).CenterCrop().Fit()
                                .Placeholder(Resource.Drawable.handycrypto_logo).Into(headerImage);
            headerTitle.Text = cryptoItem.Info.FullName;
            GradientDrawable gd = new GradientDrawable(
          GradientDrawable.Orientation.TopBottom, colorData);
            mainContainer.Background = gd;
            var layoutManager = new LinearLayoutManager(this.Activity, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);
            adapter = new DetailItemRecyclerViewAdapter(cryptoItem.AggregatedData);
            recyclerView.NestedScrollingEnabled = false;
            recyclerView.SetAdapter(adapter);
            return view;
        }
   
        
        
    }
}