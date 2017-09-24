using System;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Util;
using Firebase.Messaging;

namespace westgateproject.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class MyFirebaseMessagingService : FirebaseMessagingService
	{
		const string TAG = "MyFirebaseMsgService";
		public override void OnMessageReceived(RemoteMessage message)
		{
			Log.Debug(TAG, "From: " + message.From);
			Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            SendNotification(message.GetNotification().Body);
		}

		void SendNotification(string messageBody)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

			var notificationBuilder = new NotificationCompat.Builder(this)
				.SetSmallIcon(Resource.Drawable.HeartFilled)
				.SetContentTitle("서문시장.net")
				.SetContentText(messageBody)
				.SetAutoCancel(true)
				.SetContentIntent(pendingIntent);

			var notificationManager = NotificationManager.FromContext(this);
			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}
