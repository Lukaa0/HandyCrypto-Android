using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HandyCrypto.Model
{
    public class CryptoScrollView : ScrollView
    {

        public CryptoScrollView(Context context,Android.Util.IAttributeSet attributeSet):base(context,attributeSet)
        {
            
        }

        public delegate void OnScrollUpEventHandler(object sender, EventArgs e);
        public event OnScrollUpEventHandler OnScrollUp;


        public delegate void ScrollReachedBottomEventHandler(object sender, EventArgs e);
        public event ScrollReachedBottomEventHandler OnScrollDown;

        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            base.OnScrollChanged(l, t, oldl, oldt);

           

            if (t<oldt)
            {
                if (OnScrollUp != null) OnScrollUp.Invoke(this, new EventArgs());

            }

            else if (t > oldt)
            {
                if (OnScrollDown != null) OnScrollDown.Invoke(this, new EventArgs());

            }
        }
        
    }
}