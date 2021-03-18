using Android.App;
using Android.OS;

namespace HandyCrypto.Activities
{
    [Activity(Label = "NoConnectionActivity")]
    public class NoConnectionActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.no_connection_layout);
        }
    }
}