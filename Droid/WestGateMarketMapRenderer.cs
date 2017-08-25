using System;
using System.Collections.Generic;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Widget;
using westgateproject;
using westgateproject.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Android.Content;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(WestGateMarketMap), typeof(WestGateMarketMapRenderer))]
namespace westgateproject.Droid
{
    public class WestGateMarketMapRenderer: MapRenderer, IOnMapReadyCallback, IOnCameraIdleListener, IInfoWindowAdapter
	//, IOnMarkerClickListener // OnMarkerClick Listener
	{
		// We use a native google map for Android
		List<AdvertisementPin> advertisementPins;
		List<Position> shapeCoordinates;
		bool isRemoved;
        bool isDrawn;
		List<Marker> markerList;
        WestGateMarketMap formsMap;

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				NativeMap.InfoWindowClick -= OnInfoWindowClick;
			}

			if (e.NewElement != null)
			{
				formsMap = (WestGateMarketMap)e.NewElement;
				shapeCoordinates = formsMap.ShapeCoordinates;
				advertisementPins = formsMap.AdvertisementPins;
                Control.GetMapAsync(this);
				markerList = new List<Marker>();

                isDrawn = false;

			}
		}



		public void OnMapReady(GoogleMap googleMap)
		{

			NativeMap = googleMap;
			NativeMap.InfoWindowClick += OnInfoWindowClick;
			NativeMap.SetInfoWindowAdapter(this);

			//map.SetOnMarkerClickListener(this);
			NativeMap.SetOnCameraIdleListener(this);

			//if (map != null)
			//map.MapClick += googleMap_MapClick;

			//var polygonOptions = new PolygonOptions();
			//polygonOptions.InvokeFillColor(0x40FF0000);
			//polygonOptions.InvokeStrokeColor(0x660000ff);
			//polygonOptions.InvokeStrokeWidth(0);

			//foreach (var position in shapeCoordinates)
			//{
			//    polygonOptions.Add(new LatLng(position.Latitude, position.Longitude));
			//}

			//NativeMap.AddPolygon(polygonOptions);


			//NativeMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8687925, 128.5801115), 17.3f));

			foreach (var pin in advertisementPins)
			{

				var marker = new MarkerOptions();
				//marker.SetPosition(new LatLng(35.868084, 128.578505));
                marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
				marker.SetTitle(pin.Pin.Label);
				marker.SetSnippet(pin.Pin.Address);
				marker.SetIcon(createPureTextIcon(pin.Id));
				//marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
				markerList.Add(NativeMap.AddMarker(marker));

			}

			NativeMap.UiSettings.ZoomControlsEnabled = false;
			NativeMap.UiSettings.ZoomGesturesEnabled = true;
			NativeMap.UiSettings.ScrollGesturesEnabled = true;
			NativeMap.UiSettings.IndoorLevelPickerEnabled = false;
			NativeMap.UiSettings.CompassEnabled = false;
			NativeMap.UiSettings.RotateGesturesEnabled = false;
			NativeMap.UiSettings.TiltGesturesEnabled = false;

			//NativeMap.SetMinZoomPreference(16.8f);

			isRemoved = false;


		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);




			//Console.WriteLine("e.Propertyname is " + e.PropertyName);


			if (e.PropertyName.Equals("VisibleRegion") && !isDrawn)
			{
				NativeMap.Clear();
				foreach (var pin in advertisementPins)
				{
					var marker = new MarkerOptions();
					marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
					marker.SetTitle(pin.Pin.Label);
					marker.SetSnippet(pin.Pin.Address);
					marker.SetIcon(createPureTextIcon(pin.Id));
					//marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
					NativeMap.AddMarker(marker);

				}

				var polygonOptions = new PolygonOptions();
				polygonOptions.InvokeFillColor(0x40FF0000);
				polygonOptions.InvokeStrokeColor(0x660000ff);
				polygonOptions.InvokeStrokeWidth(0);

				foreach (var position in shapeCoordinates)
				{
					polygonOptions.Add(new LatLng(position.Latitude, position.Longitude));
				}

				NativeMap.AddPolygon(polygonOptions);

				isDrawn = true;
			}

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
			switch (text)
			{
				case "2지구":
				case "5지구":
				case "동산상가":
				case "상가연합회":
					canvas.DrawColor(Android.Graphics.Color.Green);
                    textPaint.Color = Android.Graphics.Color.Magenta;
					break;
				case "1지구":
				case "4지구":
				case "명품프라자":
				case "건해산물상가":
				case "아진상가":
				case "4지구 대체상가":
					textPaint.Color = Android.Graphics.Color.Black;
					canvas.DrawColor(Android.Graphics.Color.LightGray);
					break;
			}
			canvas.DrawText(text, 0, -10, textPaint);
			BitmapDescriptor icon = BitmapDescriptorFactory.FromBitmap(image);
			return icon;
		}


		//private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
		//{
		//	((WestGateMarketMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
		//}

		public void OnCameraIdle()
		{
			//if (NativeMap.CameraPosition.Zoom >= 16f && isRemoved == true)
			//{
			//	//add marker
			//	foreach (var mark in markerList)
			//	mark.Visible = true;
			//	isRemoved = false;
			//}
			//else if (NativeMap.CameraPosition.Zoom < 16f && isRemoved == false)
			//{
			//	//remove marker
			//	foreach (var mark in markerList)
			//	mark.Visible = false;
			//	isRemoved = true;
			//}
			System.Diagnostics.Debug.WriteLine("Zoom : " + NativeMap.CameraPosition.Zoom);
            if (NativeMap.CameraPosition.Zoom < 16f)
			{
				//Console.WriteLine("map.Projection.VisibleRegion.FarRight.Longitude : " + NativeMap.Projection.VisibleRegion.FarRight.Longitude);
				//Console.WriteLine("map.Projection.VisibleRegion.FarLeft.Longitude : " + NativeMap.Projection.VisibleRegion.FarLeft.Longitude);
				//Console.WriteLine("mapspan : " + mapspan);
				NativeMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8680505, 128.580742087216), 16.8350f));
			}
			else
			{
				var right = NativeMap.Projection.VisibleRegion.FarRight.Longitude;
				var left = NativeMap.Projection.VisibleRegion.FarLeft.Longitude;
				var up = NativeMap.Projection.VisibleRegion.FarRight.Latitude;
				var down = NativeMap.Projection.VisibleRegion.NearLeft.Latitude;

				System.Diagnostics.Debug.WriteLine("right : " + right);
				System.Diagnostics.Debug.WriteLine("left : " + left);
				System.Diagnostics.Debug.WriteLine("up : " + up);
				System.Diagnostics.Debug.WriteLine("down : " + down);

				//Console.WriteLine("mapspan in else : " + mapspan);
				if (up > 35.8741546545933 || down < 35.8623184652325 || left < 128.575300723314 || right > 128.586008436978)
				{
					LatLng currentCenter = NativeMap.CameraPosition.Target;
					float currentZoom = NativeMap.CameraPosition.Zoom;
                    NativeMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(35.8680505, 128.580742087216), currentZoom));
                }
			}


		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			if (changed)
			{
				isDrawn = false;
			}
		}

		void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			var customPin = GetCustomPin(e.Marker);
			if (customPin == null)
			{
				throw new Exception("Custom pin not found");
			}

			switch (customPin.Id)
			{
				case "2지구":
				case "5지구":
				case "동산상가":
				case "상가연합회":
					formsMap.OnTap(customPin.Id);
					break;
			}
		}

		public Android.Views.View GetInfoContents(Marker marker)
		{
            
			var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
			if (inflater != null)
			{
				Android.Views.View view;

				var customPin = GetCustomPin(marker);
				if (customPin == null)
				{
					throw new Exception("Custom pin not found");
				}


	            switch (customPin.Id)
				{
					case "Xamarin":
                        view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
						break;
	                case "1지구":
	                case "4지구":
	                case "명품프라자":
	                case "건해산물상가":
					case "아진상가":
					case "4지구 대체상가":
						view = inflater.Inflate(Resource.Layout.MapInfoWindow_Preparing, null);
						break;
                    default:
						view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
						break;
	            }

				var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
				var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

				if (infoTitle != null)
				{
					infoTitle.Text = marker.Title;
				}
				if (infoSubtitle != null)
				{
					infoSubtitle.Text = marker.Snippet;
				}

				return view;
			}
			return null;
		}

		public Android.Views.View GetInfoWindow(Marker marker)
		{
			return null;
		}

		AdvertisementPin GetCustomPin(Marker annotation)
		{
			var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
			foreach (var pin in advertisementPins)
			{
				if (pin.Pin.Position == position)
				{
					return pin;
				}
			}
			return null;
		}
        // Block auto centering when click marker
		//public bool OnMarkerClick(Marker marker)
		//{
		//	((WestGateMarketMap)Element).OnTap(new Position(marker.Position.Latitude, marker.Position.Longitude));
		//	return true;
		//}
	}
}
