using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using westgateproject.Data;
using westgateproject.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace westgateproject
{
    public class App : Application
	{
		public static double ScreenWidth;
		public static double ScreenHeight;
		public static string userEmail;
        static MarketInformationDatabase database;
        static MobileServiceClient client;

		public App ()
		{
            // The root page of your application
            MainPage = new NavigationPage(new InitialPage());

		}

        public static MarketInformationDatabase Database
        {
            get
            {
                if(database == null)
                {
                    database = new MarketInformationDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("MarketInformationSQLite.db3"));
                }
                return database;
            }
        }

        public static MobileServiceClient Client
        {
            get
            {
                if(client == null)
                {
		        	client = new MobileServiceClient(Constants.ApplicationURL);
                }
                return client;
            }
        }

        public int ResumeAtShopInformationID { get; set; }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		
	}

}

