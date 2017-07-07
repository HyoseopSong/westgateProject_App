using System;

namespace westgateproject
{
	public static class Constants
	{
		// Replace strings with your Azure Mobile App endpoint.
		public static string ApplicationURL = @"https://westgateproject.azurewebsites.net";
        public static string AppName = "westgateproject";

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "62511156556-7j6rnn91soeivdki3uvsvrtajqjg6vdh.apps.googleusercontent.com";
		public static string AndroidClientId = "62511156556-0nn8sfrn74vcf7ivbgh659q78go0vpdn.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.62511156556-7j6rnn91soeivdki3uvsvrtajqjg6vdh:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.62511156556-0nn8sfrn74vcf7ivbgh659q78go0vpdn:/oauth2redirect";
	}
}

