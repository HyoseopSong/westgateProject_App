using System;
using System.Diagnostics;
using westgateproject.Models;
using westgateproject.View.PageForEachFloor.dongsan;
using westgateproject.View.PageForEachFloor.fifth;
using westgateproject.View.PageForEachFloor.first;
using westgateproject.View.PageForEachFloor.office;
using westgateproject.View.PageForEachFloor.second;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor
{
    public partial class buildingInfo : ContentPage
    {
        bool onProcessing;
        public buildingInfo(string building)
        {
            InitializeComponent();
            syncText(building);
            Title = building;
            onProcessing = false;
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
                    break;
                case "1지구":
                case "5지구":
                    forthFloor.BackgroundColor = Color.LightGray;
                    thirdFloor.BackgroundColor = Color.LightGray;
                    //secondFloor.BackgroundColor = infoFromSQLite.Second;
                    //firstFloor.BackgroundColor = infoFromSQLite.First;
                    baseFirstFloor.BackgroundColor = Color.LightGray;
                    break;
                case "동산상가":
                    forthFloor.BackgroundColor = Color.LightGray;
                    thirdFloor.BackgroundColor = Color.YellowGreen;
                    //secondFloor.BackgroundColor = infoFromSQLite.Second;
                    //firstFloor.BackgroundColor = infoFromSQLite.First;
                    baseFirstFloor.BackgroundColor = Color.YellowGreen;
                    break;
                case "아진상가":
                    break;
                case "건해산물상가":
                    break;
                case "명품프라자":
                    break;
                case "상가연합회":
                    forthFloor.BackgroundColor = Color.LightGray;
                    thirdFloor.BackgroundColor = Color.LightGray;
                    secondFloor.BackgroundColor = Color.LightGray;
                    //firstFloor.BackgroundColor = infoFromSQLite.First;
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
                            await Navigation.PushAsync(new firstFirst());
                            //await DisplayAlert(sender.Text, "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        break;
                    case "2지구":
                        if (sender == forthFloor)
                        {
                            await Navigation.PushAsync(new secondEnter(4));
                        }
                        else if (sender == thirdFloor)
                        {
                            await Navigation.PushAsync(new secondEnter(3));
                        }
                        else if (sender == secondFloor)
                        {
                            await Navigation.PushAsync(new secondEnter(2));
                        }
                        else if (sender == firstFloor)
                        {
                            await Navigation.PushAsync(new secondEnter(1));
                        }
                        else if (sender == baseFirstFloor)
                        {
                            await Navigation.PushAsync(new secondEnter(-1));
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
                            await Navigation.PushAsync(new fifthSecond());
                        }
                        else if (sender == firstFloor)
                        {
                            await Navigation.PushAsync(new fifthFirst());
                        }
                        break;
                    case "동산상가":
                        if (sender == thirdFloor)
                        {
                            await DisplayAlert("3층", "지도 정보가 아직 준비되지 않았습니다.", "확인");
                        }
                        else if (sender == secondFloor)
                        {
                            await Navigation.PushAsync(new dongsanSecond());
                        }
                        else if (sender == firstFloor)
                        {
                            await Navigation.PushAsync(new dongsanFirst());
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
    }
}
