using System;
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
using Java.Lang;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Permissions;
using westgateproject.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

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

        TextView mStatus;

        bool mIsResolving = false;

        bool mShouldResolve = false;

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "onConnectionFailed:" + result);

            if (!mIsResolving && mShouldResolve)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult(this, RC_SIGN_IN);
                        mIsResolving = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        Log.Error(TAG, "Could not resolve ConnectionResult.", e);
                        mIsResolving = false;
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    ShowErrorDialog(result, "OnConnectionFailed inner else");
                }
            }
            else
            {
                ShowErrorDialog(result, "OnConnectionFailed outer else");
            }
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

        void ShowDialog(string title, string text)
        {


            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
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


            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestIdToken(Constants.WebClientID)
                                                             .RequestEmail()
                                                             .Build();

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                                                  .EnableAutoManage(this, this)
                                                  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                                  .Build();



            if (CrossConnectivity.Current.IsConnected)
            {
                Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
                StartActivityForResult(signInIntent, RC_SIGN_IN);
            }
            else
            {
                ShowDialog("No network", "Please connect to network");
            }


        }

        protected override void OnStart()
        {
            base.OnStart();
            Auth.GoogleSignInApi.SilentSignIn(mGoogleApiClient).SetResultCallback(this);
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (mGoogleApiClient.IsConnected)
            {
                Auth.GoogleSignInApi.RevokeAccess(mGoogleApiClient).SetResultCallback(this);
            }
        }


        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                handleSignInResult(result);
            }
            System.Diagnostics.Debug.WriteLine("OnActivityResult of MainActivity");
            System.Diagnostics.Debug.WriteLine("requestCode : " + requestCode);
            System.Diagnostics.Debug.WriteLine("resultCode : " + resultCode);
            //if (data != null)
            //Toast.MakeText(ApplicationContext, "Intent Extra result value : " + data.GetStringExtra("result"), ToastLength.Long).Show();

            //AlertDialog.Builder alert = new AlertDialog.Builder(this);
            //alert.SetTitle(resultCode.ToString());
            //if (data == null)
            //alert.SetMessage("Intent Extra result value : null");
            //else
            //    alert.SetMessage("Intent Extra result value : " + data.GetStringExtra("result"));
                
            //alert.SetPositiveButton("Delete", (senderAlert, args) => {
            //    Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
            //});

            //alert.SetNegativeButton("Cancel", (senderAlert, args) => {
            //    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            //});

            //Dialog dialog = alert.Create();
            //dialog.Show();

            if (resultCode == Result.Ok)
            {
                MessagingCenter.Send<object>(this, "OK");
            }
            else if(resultCode == Result.Canceled)
            {
                MessagingCenter.Send<object>(this, "Canceled");
            }
            else
            {
                MessagingCenter.Send<object>(this, "NotDefined");
            }

            

        }
        async private void handleSignInResult(GoogleSignInResult result)
        {

            if (result.IsSuccess)
            {
                GoogleSignInAccount acct = result.SignInAccount;
                var googleToken = acct.IdToken;
                //Console.WriteLine("acct value : " + acct);
                //Console.WriteLine("Account value : " + acct.Account);
                //Console.WriteLine("DisplayName value : " + acct.DisplayName);
                //Console.WriteLine("Email value : " + acct.Email);
                //Console.WriteLine("FamilyName value : " + acct.FamilyName);
                //Console.WriteLine("acGivenNamect value : " + acct.GivenName);
                //Console.WriteLine("Id value : " + acct.Id);
                //Console.WriteLine("IdToken value : " + acct.IdToken);
                //Console.WriteLine("PhotoUrl value : " + acct.PhotoUrl);
                //Console.WriteLine("ServerAuthCode value : " + acct.ServerAuthCode);

                //           foreach (var temp in result.SignInAccount.GrantedScopes)
                //           {
                //Console.WriteLine("ServerAuthCode value : " + temp);
                //}

                App.userEmail = acct.Email;

                var token = new JObject
                {
                    { "id_token", googleToken }
                };
                App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);
                //IDictionary<string, string> resultOfInvoke = new Dictionary<string, string>();
                //try
                //{
                //  resultOfInvoke = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
                //  foreach (var temp in resultOfInvoke)
                //  {
                //      Console.WriteLine("Key : " + temp.Key + ", Value : " + temp.Value);
                //  }
                //}
                //catch (System.Exception ex)
                //{
                //  Console.WriteLine(ex.GetType());
                //  Console.WriteLine("서버에서 정보를 불러올 수 없습니다.");
                //}
                Intent intent = new Intent();
                intent.PutExtra("result", "ok");
                SetResult(Result.Ok, intent);

            }
            else
            {
                Intent intent = new Intent();
                intent.PutExtra("result", "notok");
                SetResult(Result.Canceled, intent);
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
            Console.WriteLine("You are loged out. param : " + result);
        }
    }
}

