using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class ShopInfoPage : ContentPage
    {
        public ShopInfoPage(ShopInformation info)
        {
            InitializeComponent();
            if (info != null)
            {
                location.Text = info.Building + " " + info.Floor + " " + info.Location;
                name.Text = info.ShopName;
                phoneNumber.Text = info.PhoneNumber;
            }
            else
            {
				location.Text = "준비 중 입니다.";
				name.Text = "준비 중 입니다.";
				phoneNumber.Text = "준비 중 입니다.";

            }
        }
    }
}
