using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using HandyCrypto.DataHelper;
using HandyCrypto.Model;
using Java.Util;
using PortableCryptoLibrary;

namespace HandyCrypto.Services
{

    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootBroadcast : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            MainActivity.SetAlarmForBackgroundServices(context);
        }
    }
    [BroadcastReceiver]
    class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                var backgroundServiceIntent = new Intent(context, typeof(SensorService));
                context.StartService(backgroundServiceIntent);
            }
            catch (Exception)
            {

            }
        }
    }
    [Service]
    public class SensorService : Service
    {
        private bool _isRunning;
        private Context _context;
        private Task _task;

        #region overrides

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            _context = this;

            _isRunning = false;
            _task = new Task(DoWork);

        }

        public override void OnDestroy()
        {
            _isRunning = false;

            //if (_task != null && _task.Status == TaskStatus.RanToCompletion)
            //{
            //    _task.Dispose();
            //}
            Intent broadcastIntent = new Intent(this, typeof(BootBroadcast));
            SendBroadcast(broadcastIntent);

        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            

            if (!_isRunning && _task.Status != TaskStatus.Running)
            {
                _isRunning = true;
                _task = new Task(DoWork);
                _task.Start();
            }

            return StartCommandResult.Sticky;
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);

            Intent broadcastIntent = new Intent(this, typeof(BootBroadcast));
            SendBroadcast(broadcastIntent);
        }
        #endregion
       
        private async void DoWork()
        {
            try
            {
                var items = await LocalDbService<AlarmItemModel>.Instance.SelectItems();

                var prices = await HandyCryptoClient.Instance.GeneralCoinInfo.GetPriceData(items.Select(x => x.Symbol).ToArray());
                for (int i = 0; i < items.Count; i++)
                {
                    for (int j = 0; j < prices.Count; j++)
                    {
                        if (items[i].Symbol == prices[j].FromSymbol)
                        {
                            switch (items[i].TypeId)
                            {
                                case 0:
                                    {
                                        if (items[i].Value < prices[j].Price.Value)
                                        {
                                            ShowNotification(items[i].Symbol + " increased!", items[i].Symbol + 
                                                " has become more than " + items[i].Value + 
                                                " and is now " + prices[j].Price);
                                            await LocalDbService<AlarmItemModel>.Instance.DeleteAsync(items[i]);
                                        }

                                        break;
                                    }
                                case 1:
                                    {
                                        if(items[i].Value > prices[j].Price.Value)
                                        {
                                            ShowNotification(items[i].Symbol + " decreased!", items[i].Symbol +
                                                " has become less than " + items[i].Value +
                                                " and is now " + prices[j].Price);
                                            await LocalDbService<AlarmItemModel>.Instance.DeleteAsync(items[i]);

                                        }
                                        break;

                                    }

                            }

                        }
                    }
                }

            }
            catch (Exception)
            {

            }

            _isRunning = false;
        }

        private void ShowNotification(string title,string content)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "255")
                 .SetContentTitle(title)
                 .SetContentText(content)
                 .SetDefaults(1)
                 .SetSmallIcon(Resource.Drawable.handycrypto_logo);
            
            Notification notification = builder.Build();
            
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    } 
}