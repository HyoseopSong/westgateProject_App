using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Util;
using Android.Widget;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Firebase.Messaging;
using Firebase.Iid;
using westgateproject.View;

namespace westgateproject.Droid
{
    [Activity (Label = "서문시장.net",    Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
               Theme = "@style/MyTheme",
                //ScreenOrientation = ScreenOrientation.User),
               ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IResultCallback
    {
        const string TAG = "MainActivity";

        const int RC_SIGN_IN = 9001;

        const string KEY_IS_RESOLVING = "is_resolving";
        const string KEY_SHOULD_RESOLVE = "should_resolve";

        GoogleApiClient mGoogleApiClient;

        bool isOnStart = false;

        public void OnConnectionFailed(ConnectionResult result)
        {
            //Log.Debug(TAG, "onConnectionFailed:" + result);

            //if (!mIsResolving && mShouldResolve)
            //{
            //    if (result.HasResolution)
            //    {
            //        try
            //        {
            //            result.StartResolutionForResult(this, RC_SIGN_IN);
            //            mIsResolving = true;
            //        }
            //        catch (IntentSender.SendIntentException e)
            //        {
            //            Log.Error(TAG, "Could not resolve ConnectionResult.", e);
            //            mIsResolving = false;
            //            mGoogleApiClient.Connect();
            //        }
            //    }
            //    else
            //    {
            //        ShowErrorDialog(result, "OnConnectionFailed inner else");
            //    }
            //}
            //else
            //{
            //    ShowErrorDialog(result, "OnConnectionFailed outer else");
            //}
        }

        void ShowErrorDialog(ConnectionResult connectionResult, string text)
        {
            int errorCode = connectionResult.ErrorCode;

            //Toast.MakeText(ApplicationContext, "Intent Extra result value : " + data.GetStringExtra("result"), ToastLength.Long).Show();

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("ErrorCode : " + errorCode);
            alert.SetMessage(text);
            alert.SetPositiveButton("Delete", (senderAlert, args) => {
                Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();

        }

        protected override void OnCreate (Bundle savedInstanceState)
        {

			Log.Debug(TAG, "google app id: " + Resource.String.google_app_id);

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
            base.OnCreate (savedInstanceState);

            // Initialize Azure Mobile Apps
            // Initialize Xamarin Forms
            global::Xamarin.Forms.Forms.Init (this, savedInstanceState);
            global::Xamarin.FormsMaps.Init (this, savedInstanceState);

            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;


            // Load the main application
            LoadApplication (new App ());
			if (Intent.Extras != null)
			{
				foreach (var key in Intent.Extras.KeySet())
				{
					var value = Intent.Extras.GetString(key);
					Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
				}
			}


			//IsPlayServicesAvailable();

            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestIdToken(Constants.WebClientID)
                                                             .RequestEmail()
                                                             .Build();

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                                                  .EnableAutoManage(this, this)
                                                  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                                  .Build();

			if (CrossConnectivity.Current.IsConnected && !mGoogleApiClient.IsConnected)
			{
				isOnStart = true;
				Auth.GoogleSignInApi.SilentSignIn(mGoogleApiClient).SetResultCallback(this);
			}

			MessagingCenter.Subscribe<object>(this, "GoogleDisconnect", (sender) =>
			{

				if (mGoogleApiClient.IsConnected)
				{
					Auth.GoogleSignInApi.RevokeAccess(mGoogleApiClient).SetResultCallback(this);
				}

			});

			MessagingCenter.Subscribe<object>(this, "ActivityFinish", (sender) =>
			{
                Finish();
			});

			MessagingCenter.Subscribe<object>(this, "InstanceIDToken", (sender) =>
			{
                Register.FirebaseInstanceID = FirebaseInstanceId.Instance.Token;
                Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
			});

			MessagingCenter.Subscribe<object>(this, "Subscribe", (sender) =>
			{
				FirebaseMessaging.Instance.SubscribeToTopic("news");
				Log.Debug(TAG, "Subscribed to remote notifications");
			});
        }

        protected override void OnDestroy()
        {
			base.OnDestroy();

			if (mGoogleApiClient.IsConnected)
			{
				Auth.GoogleSignInApi.SignOut(mGoogleApiClient).SetResultCallback(this);
			}
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            System.Diagnostics.Debug.WriteLine("ResultCode : " + resultCode);
            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
				handleSignInResult(result);


				if (resultCode == Result.Ok)
				{
					MessagingCenter.Send<object>(this, "OK");
				}
				else
				{
					Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
					StartActivityForResult(signInIntent, RC_SIGN_IN);
				}
            }
            

        }
        async private void handleSignInResult(GoogleSignInResult result)
        {

            if (result.IsSuccess)
            {
                GoogleSignInAccount acct = result.SignInAccount;
                var googleToken = acct.IdToken;

                App.userEmail = acct.Email;

                var token = new JObject
                {
                    { "id_token", googleToken }
                };
                App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);


            }

        }

        public void OnConnected(Bundle connectionHint)
        {
            Log.Debug(TAG, "onConnected:" + connectionHint);
        }

        public void OnConnectionSuspended(int cause)
        {
            Log.Warn(TAG, "onConnectionSuspended:" + cause);
        }

        public void OnResult(Java.Lang.Object result)
        {
            if(isOnStart)
            {
				var res = (GoogleSignInResult)result;
                if(!res.IsSuccess)
                {
				    Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
				    StartActivityForResult(signInIntent, RC_SIGN_IN);
				}
                else
				{

					handleSignInResult(res);
                    MessagingCenter.Send<object>(this, "OK");
                }
				isOnStart = false;
            }

        }

		public bool IsPlayServicesAvailable()
		{
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success)
			{
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(this, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Short).Show();
                }
				else
				{
                    Toast.MakeText(this, "This device is not supported", ToastLength.Short).Show();
				}
				return false;
			}
			else
			{
                Toast.MakeText(this, "Google Play Services is available.", ToastLength.Short).Show();
				return true;
			}
		}
    }
}

