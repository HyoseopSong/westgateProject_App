using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using westgateproject.View.PageForEachFloor.first;
using westgateproject.View.PageForEachFloor.second;
using westgateproject.View.PageForEachFloor;
using westgateproject.View.PageForEachFloor.fifth;
using westgateproject.View.PageForEachFloor.forth;
using westgateproject.View.PageForEachFloor.myungpoom;
using westgateproject.View.PageForEachFloor.office;
using westgateproject;

namespace westgateproject
{
    public class WestGateMarketMap : Map
    {
        //public event EventHandler<MapTapEventArgs> Tapped;

        public List<Position> ShapeCoordinates { get; set; }

        public List<AdvertisementPin> AdvertisementPins { get; set; }



        public WestGateMarketMap()
        {
            ShapeCoordinates = new List<Position>();
        }
        public WestGateMarketMap(MapSpan region) : base(region)
        { }

        public void OnTap(string building)
        {
            Navigation.PushAsync(new buildingInfo(building));
            //OnTap(new MapTapEventArgs { Position = coordinate });
        }

        //protected virtual async void OnTap(MapTapEventArgs e)
        //{
        //    var handler = Tapped;
        //    var lat = e.Position.Latitude;
        //    var lon = e.Position.Longitude;

        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //    //1지구
        //    if ((lat < 35.869672 && lat > 35.869096) && (lon < 128.580168 && lon > 128.579522))
        //        await Navigation.PushAsync(new buildingInfo("1지구"));
        //    //2지구
        //    else if ((lat < 35.869008 && lat > 35.868594) && (lon < 128.580183 && lon > 128.578874))
        //    {
        //        await Navigation.PopAsync();
        //        await Navigation.PushAsync(new buildingInfo("2지구"));
        //    }
        //    //4지구
        //    else if ((lat < 35.869739 && lat > 35.869154) && (lon < 128.581192 && lon > 128.580377))
        //        await Navigation.PushAsync(new buildingInfo("4지구"));
        //    //5지구
        //    else if ((lat < 35.868368 && lat > 35.867772) && (lon < 128.578844 && lon > 128.578100))
        //        await Navigation.PushAsync(new buildingInfo("5지구"));
        //    //동산상가
        //    else if ((lat < 35.869158 && lat > 35.868736) && (lon < 128.582094 && lon > 128.581328))
        //        await Navigation.PushAsync(new buildingInfo("동산상가"));
        //    //아진상가
        //    else if ((lat < 35.869788 && lat > 35.869218) && (lon < 128.582119 && lon > 128.581262))
        //        await Navigation.PushAsync(new buildingInfo("아진상가"));
        //    //상가연합회
        //    else if ((lat < 35.869070 && lat > 35.868706) && (lon < 128.581173 && lon > 128.580369))
        //        await Navigation.PushAsync(new buildingInfo("상가연합회"));
        //    //명품프라자
        //    else if ((lat < 35.870096 && lat > 35.869770) && (lon < 128.581137 && lon > 128.580232))
        //        await Navigation.PushAsync(new buildingInfo("명품프라자"));
        //    //건해산물상가
        //    else if ((lat < 35.868555 && lat > 35.868204) && (lon < 128.581540 && lon > 128.579632))
        //        await Navigation.PushAsync(new buildingInfo("건해물상가"));
        //    else
        //        return;

        //}
    }

    public class MapTapEventArgs : EventArgs
    {
        public Position Position { get; set; }
    }
}
