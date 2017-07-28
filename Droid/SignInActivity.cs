
using System;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Java.Lang;
using Newtonsoft.Json.Linq;

namespace westgateproject.Droid
{
    [Activity(Label = "SignInActivity",
			   Theme = "@style/MyTheme")]
	//[Activity(Label = "서문시장",
		//Icon = "@drawable/icon",
		//MainLauncher = true,
		//ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
			   //Theme = "@android:style/Theme.Holo.Light", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignInActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IOnConnectionFailedListener, IResultCallback
    {
		//private static readonly String TAG = "SignInActivity";
        private static readonly int RC_SIGN_IN = 9001;

		private GoogleApiClient mGoogleApiClient;
		//private TextView mStatusTextView;
		//private ProgressDialog mProgressDialog;

        public void OnConnectionFailed(ConnectionResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestIdToken(Constants.WebClientID)
                                                             .RequestEmail()
                                                             .Build();

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                                                  .EnableAutoManage(this, this)
                                                  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                                  .Build();

            if (Intent.GetStringExtra("action") == "login")
            {
                Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
                StartActivityForResult(signInIntent, RC_SIGN_IN);
            }
            else
            {
                Auth.GoogleSignInApi.SignOut(mGoogleApiClient).SetResultCallback(this);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                handleSignInResult(result);
            }
        }

        async private void handleSignInResult(GoogleSignInResult result)
        {

            if(result.IsSuccess)
            {
                GoogleSignInAccount acct = result.SignInAccount;
                var googleToken = acct.IdToken;
				Console.WriteLine("acct value : " + acct);
				Console.WriteLine("Account value : " + acct.Account);
				Console.WriteLine("DisplayName value : " + acct.DisplayName);
				Console.WriteLine("Email value : " + acct.Email);
				Console.WriteLine("FamilyName value : " + acct.FamilyName);
				Console.WriteLine("acGivenNamect value : " + acct.GivenName);
				Console.WriteLine("Id value : " + acct.Id);
				Console.WriteLine("IdToken value : " + acct.IdToken);
				Console.WriteLine("PhotoUrl value : " + acct.PhotoUrl);
				Console.WriteLine("ServerAuthCode value : " + acct.ServerAuthCode);
				var token = new JObject();
				token.Add("id_token", googleToken);


				App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);

            }
            else
            {
                
            }
            Finish();
        }

        public void OnResult(Java.Lang.Object result)
        {
            Console.WriteLine("You are loged out. param : " + result);
        }
    }
}
