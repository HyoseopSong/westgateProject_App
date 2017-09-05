using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondEnter : ContentPage
    {
        bool onProcessing;
        int floorNum;
        public secondEnter(int floor)
        {
			InitializeComponent();
			onProcessing = false;
            floorNum = floor;
        }

		async void OnTappedEast(object sender, EventArgs args)
		{
            if (!onProcessing)
            {
                onProcessing = true;
                if (floorNum == -1)
                {
                    await Navigation.PushAsync(new secondBaseFirst_east());
                }
				else if (floorNum == 1)
				{
					await Navigation.PushAsync(new secondFirst_east());
				}
				else if (floorNum == 2)
				{
					await Navigation.PushAsync(new secondSecond_east());
				}
				else if (floorNum == 3)
				{
					await Navigation.PushAsync(new secondThird_east());
				}
				else if (floorNum == 4)
				{
					await Navigation.PushAsync(new secondForth_east());
				}
            
                onProcessing = false;
            }
		}
		async void OnTappedWest(object sender, EventArgs args)
		{
            if (!onProcessing)
			{
				onProcessing = true;
				if (floorNum == -1)
				{
					await Navigation.PushAsync(new secondBaseFirst_west());
				}
				else if (floorNum == 1)
				{
					await Navigation.PushAsync(new secondFirst_west());
				}
				else if (floorNum == 2)
				{
					await Navigation.PushAsync(new secondSecond_west());
				}
				else if (floorNum == 3)
				{
					await Navigation.PushAsync(new secondThird_west());
				}
				else if (floorNum == 4)
				{
					await Navigation.PushAsync(new secondForth_west());
				}

                onProcessing = false;
            }
		}
    }
}
