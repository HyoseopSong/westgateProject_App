using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using westgateproject.Models;

namespace westgateproject
{
	public partial class MainPage : ContentPage
	{

		IDictionary<string, string> postDictionary = new Dictionary<string, string>();
		IDictionary<string, string> getDictionary = new Dictionary<string, string>();

		public MainPage()
		{
			InitializeComponent();
			postDictionary.Add("Temp", "Post Dictionary");
            postDictionary.Add("Temp2", "Added Dictionary");
			getDictionary.Add("temp", "Get Dictionary");
		}

		async void VGetClicked(object sender, EventArgs e)
		{
			var result = await App.Client.InvokeApiAsync<string>("Values", System.Net.Http.HttpMethod.Get, null, System.Threading.CancellationToken.None);
			text.Text = result;
		}

		async void VPostClicked(object sender, EventArgs e)
		{
			var result = await App.Client.InvokeApiAsync<string>("Values");
			text.Text = result;
		}

		async void SGetClicked(object sender, EventArgs e)
		{
			var result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("sample", System.Net.Http.HttpMethod.Get, getDictionary);
            Debug.WriteLine("Sample Get: ");
            Debug.WriteLine("지하1층 : " + result["지하1층"]);
            Debug.WriteLine("지상1층 : " + result["지상1층"]);
            Debug.WriteLine("지상2층 : " + result["지상2층"]);
            Debug.WriteLine("지상3층 : " + result["지상3층"]);
            Debug.WriteLine("지상4층 : " + result["지상4층"]);
            //foreach(var key in result.Keys)
                //Debug.WriteLine(key);
		}

		async void SPostClicked(object sender, EventArgs e)
		{
            IDictionary<string, string> result = new Dictionary<string, string>();
			using (var scope = new ActivityIndicatorScope(syncIndicator, true))
			{
				result = await App.Client.InvokeApiAsync<IDictionary<string,string>>("sample", System.Net.Http.HttpMethod.Post, postDictionary);
			}
            foreach (var key in postDictionary.Keys)
                Debug.WriteLine(key);

			Debug.WriteLine("Sample Post: ");
			Debug.WriteLine(result["First"] + " " + result["Second"]);
		}

		void OnErase(object sender, EventArgs e)
		{
			text.Text = "Erased!!";
		}

		private class ActivityIndicatorScope : IDisposable
		{
			private bool showIndicator;
			private ActivityIndicator indicator;
			private Task indicatorDelay;

			public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
			{
				this.indicator = indicator;
				this.showIndicator = showIndicator;

				if (showIndicator)
				{
					indicatorDelay = Task.Delay(0);
					SetIndicatorActivity(true);
				}
				else
				{
					indicatorDelay = Task.FromResult(0);
				}
			}

			private void SetIndicatorActivity(bool isActive)
			{
				this.indicator.IsVisible = isActive;
				this.indicator.IsRunning = isActive;
			}

			public void Dispose()
			{
				if (showIndicator)
				{
					indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}
	}
}
