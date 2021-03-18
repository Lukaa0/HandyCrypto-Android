using System;
using Android.Support.V7.Widget;

namespace HandyCrypto.Model
{
    public class CoinRecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e,int position);
        public event LoadMoreEventHandler LoadMoreEvent;
        public bool isLoading { get; set; }

        private LinearLayoutManager LayoutManager;
             
        public CoinRecyclerViewScrollListener(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var position = LayoutManager.FindLastVisibleItemPosition();
            var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount&&!isLoading)
            {
                isLoading = true;
                LoadMoreEvent(this, null, position);
            }
        }
    }
}