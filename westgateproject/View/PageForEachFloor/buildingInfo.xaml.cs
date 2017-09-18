using System;
using System.Diagnostics;
using westgateproject.Models;
using westgateproject.View.PageForEachFloor.first;
using westgateproject.View.PageForEachFloor.office;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor
{
    public partial class buildingInfo : ContentPage
    {
		bool onProcessing;
		bool backTouched;
        public buildingInfo(string building)
        {
            InitializeComponent();
            syncText(building);
            Title = building;
			onProcessing = false;
			backTouched = false;
        }
        async void syncText(string building)
        {
            BuildingInformation infoFromSQLite = new BuildingInformation();
            infoFromSQLite = await App.Database.GetBuildingAsync(building);
                
            baseFirstFloor.Text = infoFromSQLite.BaseFirst;
            
            firstFloor.Text = infoFromSQLite.First;

            secondFloor.Text = infoFromSQLite.Second;

            thirdFloor.Text = infoFromSQLite.Third;

            forthFloor.Text = infoFromSQLite.Forth;
            switch (Title)
            {
				case "2지구":
				case "아진상가":
				case "건해산물상가":
				case "명품프라자":
                    break;
                case "1지구":
                case "5지구":
                    forthFloor.BackgroundColor = Color.LightGray;
                    thirdFloor.BackgroundColor = Color.LightGray;
                    baseFirstFloor.BackgroundColor = Color.LightGray;
                    break;
                case "동산상가":
                    forthFloor.BackgroundColor = Color.LightGray;
                    baseFirstFloor.BackgroundColor = Color.YellowGreen;
                    break;
                case "상가연합회":
                    forthFloor.BackgroundColor = Color.LightGray;
                    thirdFloor.BackgroundColor = Color.LightGray;
                    secondFloor.BackgroundColor = Color.LightGray;
                    baseFirstFloor.BackgroundColor = Color.LightGray;
                    break;
                default:
                    break;
            }

        }

        async void OnTapped(Label sender, EventArgs args)
        {
            var bc= sender.BackgroundColor;
            sender.BackgroundColor = new Color(255, 182, 193);
            if (!onProcessing)
            {
                onProcessing = true;
                switch (Title)
                {
                    case "1지구":
                        if (sender == thirdFloor)
                        {
                            await DisplayAlert(sender.Text, "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        else if (sender == secondFloor)
                        {
                            await DisplayAlert(sender.Text, "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        else if (sender == firstFloor)
                        {
                            await Navigation.PushAsync(new firstFirst(Title, "3층"));
                            //await DisplayAlert(sender.Text, "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        break;
                    case "2지구":
                        if (sender == forthFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "4층"));
                        }
                        else if (sender == thirdFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "3층"));
                        }
                        else if (sender == secondFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "2층"));
                        }
                        else if (sender == firstFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "1층"));
                        }
                        else if (sender == baseFirstFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "지하1층"));
                        }
                        break;
                    case "5지구":
                        //if (sender == thirdFloor)
                        //{
                        //    await Navigation.PushAsync(new Preparing());
                        //}
                        //else 
                        if (sender == secondFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "2층"));
                        }
                        else if (sender == firstFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "1층"));
                        }
                        break;
                    case "동산상가":
                        if (sender == thirdFloor)
                        {
                            await Navigation.PushAsync(new FloorMap(Title, "3층"));
                        }
                        else if (sender == secondFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "2층"));
                        }
                        else if (sender == firstFloor)
						{
							await Navigation.PushAsync(new FloorMap(Title, "1층"));
                        }
                        else if (sender == baseFirstFloor)
                        {
                            await DisplayAlert("지하1층", "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        break;
                    case "아진상가":
                        if (sender == firstFloor)
                        {
                            //await Navigation.PushAsync(new Preparing());
                        }
                        break;
                    case "건해산물상가":
                        if (sender == firstFloor)
                        {
                            //await Navigation.PushAsync(new Preparing());
                        }
                        break;
                    case "명품프라자":
                        if (sender == secondFloor)
                        {
                            //await Navigation.PushAsync(new Preparing());
                        }
                        else if (sender == firstFloor)
                        {
                            //await Navigation.PushAsync(new Preparing());
                        }
                        break;
                    case "상가연합회":
                        if (sender == firstFloor)
						{
							await Navigation.PushAsync(new shopUnion());
                        }
                        break;
                    default:
                        break;
                }
                onProcessing = false;
                sender.BackgroundColor = bc;
            }
        }


		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}

    }
}
