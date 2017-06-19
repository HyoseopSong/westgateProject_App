using System;
using System.Collections.Generic;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using westgateproject;
using westgateproject.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(WestGateMarketMap), typeof(WestGateMarketMapRenderer))]
namespace westgateproject.Droid
{
	public class WestGateMarketMapRenderer: MapRenderer, IOnMapReadyCallback, IOnCameraIdleListener, IOnMarkerClickListener
	{
		// We use a native google map for Android
		public GoogleMap map;
		List<AdvertisementPin> advertisementPins;
		List<Position> shapeCoordinates;
		bool isRemoved;
		List<Marker> markerList;


		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{

			}

			if (e.NewElement != null)
			{
				var formsMap = (WestGateMarketMap)e.NewElement;
				shapeCoordinates = formsMap.ShapeCoordinates;
				advertisementPins = formsMap.AdvertisementPins;
				((MapView)Control).GetMapAsync(this);
				markerList = new List<Marker>();

			}
		}



		public void OnMapReady(GoogleMap googleMap)
		{
			map = googleMap;

			map.SetOnMarkerClickListener(this);
			map.SetOnCameraIdleListener(this);

			if (map != null)
			map.MapClick += googleMap_MapClick;

			var polygonOptions = new PolygonOptions();
			polygonOptions.InvokeFillColor(0x40FF0000);
			polygonOptions.InvokeStrokeColor(0x660000ff);
			polygonOptions.InvokeStrokeWidth(0);

			foreach (var position in shapeCoordinates)
			{
			polygonOptions.Add(new LatLng(position.Latitude, position.Longitude));
			}

			map.AddPolygon(polygonOptions);


			map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8687925, 128.5801115), 17.3f));

			foreach (var pin in advertisementPins)
			{

			var marker = new MarkerOptions();
			marker.SetPosition(new LatLng(35.868084, 128.578505));
			marker.SetTitle(pin.Pin.Label);
			marker.SetSnippet(pin.Pin.Address);
			marker.SetIcon(createPureTextIcon("5지구"));
			//marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
			//markerList.Add(map.AddMarker(marker));

			}

			map.UiSettings.ZoomControlsEnabled = false;
			map.UiSettings.ZoomGesturesEnabled = true;
			map.UiSettings.ScrollGesturesEnabled = true;
			map.UiSettings.IndoorLevelPickerEnabled = false;
			map.UiSettings.CompassEnabled = false;
			map.UiSettings.RotateGesturesEnabled = false;
			map.UiSettings.TiltGesturesEnabled = false;

			map.SetMinZoomPreference(16.8f);

			isRemoved = false;


		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);




			//Console.WriteLine("e.Propertyname is " + e.PropertyName);


			//if (e.PropertyName.Equals("VisibleRegion") && !isDrawn)
			//if (!isDrawn)
			//{
			//	map.Clear();
			//	Console.WriteLine("It doens't work again.");
			//	var i = 0;
			//	foreach (var pin in customPins)
			//	{
			//		Console.WriteLine( i++ );
			//		var marker = new MarkerOptions();
			//		marker.SetPosition(new LatLng(35.868084, 128.578505));
			//		marker.SetTitle(pin.Pin.Label);
			//		marker.SetSnippet(pin.Pin.Address);
			//		//marker.SetIcon(createPureTextIcon("Hi there?"));
			//		marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
			//		map.AddMarker(marker);

			//	}
			//	isDrawn = true;
			//}

		}


		private BitmapDescriptor createPureTextIcon(String text)
		{
			Paint textPaint = new Paint();

			textPaint.TextSize = 75;

			float textWidth = textPaint.MeasureText(text);
			float textHeight = textPaint.TextSize;
			int width = (int)(textWidth);
			int height = (int)(textHeight);

			Bitmap image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(image);

			canvas.Translate(0, height);

			canvas.DrawColor(Android.Graphics.Color.LightGray);

			canvas.DrawText(text, 0, 0, textPaint);
			BitmapDescriptor icon = BitmapDescriptorFactory.FromBitmap(image);
			return icon;
		}


		private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
		{
			((WestGateMarketMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
		}

		public void OnCameraIdle()
		{
			var mapspan = map.Projection.VisibleRegion.FarRight.Longitude - map.Projection.VisibleRegion.FarLeft.Longitude;
			if (mapspan < 0.007 && isRemoved == true)
			{
				//add marker
				foreach (var mark in markerList)
				mark.Visible = true;
				isRemoved = false;
			}
			else if (mapspan >= 0.007 && isRemoved == false)
			{
				//remove marker
				foreach (var mark in markerList)
				mark.Visible = false;
				isRemoved = true;
			}
			var right = map.Projection.VisibleRegion.FarRight.Longitude;
			var left = map.Projection.VisibleRegion.FarLeft.Longitude;
			var up = map.Projection.VisibleRegion.FarRight.Latitude;
			var down = map.Projection.VisibleRegion.NearLeft.Latitude;

			if (mapspan > 0.00418324023485184)
			{
				map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8687925, 128.5801115), 17.3f));
			}
			else
			{
				LatLng currentCenter = map.CameraPosition.Target;
				float currentZoom = map.CameraPosition.Zoom;
				if (up > 35.8717059984586 || down < 35.8657124078645 || left < 128.577723763883 || right > 128.582757264376)
				map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8687925, 128.5801115), currentZoom));
			}


		}

		public bool OnMarkerClick(Marker marker)
		{
			((WestGateMarketMap)Element).OnTap(new Position(marker.Position.Latitude, marker.Position.Longitude));
			return true;
		}
	}
}
