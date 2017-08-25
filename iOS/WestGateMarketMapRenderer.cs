using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoreGraphics;
using CoreLocation;
using MapKit;
using UIKit;
using westgateproject;
using westgateproject.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

[assembly: ExportRenderer(typeof(WestGateMarketMap), typeof(WestGateMarketMapRenderer))]
namespace westgateproject.iOS
{
	public class WestGateMarketMapRenderer:MapRenderer, IMKMapViewDelegate
	{
		//private readonly UITapGestureRecognizer _tapRecogniser;
		private IMKAnnotation[] annotations;

        WestGateMarketMap formsMap;
		//UIView customPinView;
        List<AdvertisementPin> advertisementPins;
		//public WestGateMarketMapRenderer()
		//{
			//_tapRecogniser = new UITapGestureRecognizer(OnTap)
			//{
			//	NumberOfTapsRequired = 1,
			//	NumberOfTouchesRequired = 1
			//};
		//}


		//private void OnTap(UITapGestureRecognizer recognizer)
		//{
		//	var cgPoint = recognizer.LocationInView(Control);

		//	var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

		//	((WestGateMarketMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
		//}

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{

			base.OnElementChanged(e);
			if (e.OldElement != null)
			{

				var nativeMap = Control as MKMapView;
				//nativeMap.RemoveGestureRecognizer(_tapRecogniser);
				nativeMap.GetViewForAnnotation = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				//nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				//nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;

			}

			if (e.NewElement != null)
			{
				formsMap = (WestGateMarketMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				advertisementPins = formsMap.AdvertisementPins;

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				//nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				//nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;

				//nativeMap.AddGestureRecognizer(_tapRecogniser);
				MKPolygon polygonOverlay = new MKPolygon();
				CLLocationCoordinate2D[] coord = new CLLocationCoordinate2D[formsMap.ShapeCoordinates.Count];

				var index = 0;
				foreach (var position in formsMap.ShapeCoordinates)
				{
					coord[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
					index++;
				}


				polygonOverlay = MKPolygon.FromCoordinates(coord);
				nativeMap.AddOverlay(polygonOverlay);
				var renderer = new MKPolygonRenderer(polygonOverlay)
				{
					FillColor = UIColor.Red,
					Alpha = 0.4f,
					LineWidth = 0
				};
				nativeMap.OverlayRenderer = (view, overlay) => renderer;
				annotations = nativeMap.Annotations;
				//var isRemoved = false;

				nativeMap.RegionChanged += (sender, _e) =>
				{
					//if ((nativeMap.Region.Span.LongitudeDelta < 0.007) && (isRemoved == true))
					//{
					//	foreach (var _an in annotations)
					//	{

					//		nativeMap.AddAnnotation(_an);
					//	}
					//	isRemoved = false;
					//}
					//else if ((nativeMap.Region.Span.LongitudeDelta >= 0.007) && (isRemoved == false))
					//{
					//	annotations = nativeMap.Annotations;
					//	nativeMap.RemoveAnnotations(nativeMap.Annotations);
					//	isRemoved = true;
					//}

				//	var currentRegion = nativeMap.Region;
                    //Debug.WriteLine("LongitudeDelta Region changed : " + nativeMap.Region.Span.LongitudeDelta);
					if (nativeMap.Region.Span.LongitudeDelta > 0.008)
					{
						System.Diagnostics.Debug.WriteLine("LongitudeDelta : " + nativeMap.Region.Span.LongitudeDelta);
						//                    Debug.WriteLine("LongitudeDelta if : " + nativeMap.Region.Span.LongitudeDelta);
						nativeMap.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(35.8680838081858, 128.580841355511), new MKCoordinateSpan(0, 0.00583032772880188)), true);
						//Debug.WriteLine("LongitudeDelta if : " + nativeMap.Region.Span.LongitudeDelta);
					}
					else
					{
                        //Debug.WriteLine("LongitudeDelta else : " + nativeMap.Region.Span.LongitudeDelta);
						var halfLatDel = nativeMap.Region.Span.LatitudeDelta / 2;
						var halfLonDel = nativeMap.Region.Span.LongitudeDelta / 2;
						var currentSpan = nativeMap.Region.Span;

						var up = nativeMap.Region.Center.Latitude + halfLatDel;
						var down = nativeMap.Region.Center.Latitude - halfLatDel;
						var left = nativeMap.Region.Center.Longitude - halfLonDel;
						var right = nativeMap.Region.Center.Longitude + halfLonDel;

						System.Diagnostics.Debug.WriteLine("Center : " + nativeMap.Region.Center.Latitude + ", " + nativeMap.Region.Center.Longitude);
						System.Diagnostics.Debug.WriteLine("right : " + right);
						System.Diagnostics.Debug.WriteLine("left : " + left);
						System.Diagnostics.Debug.WriteLine("up : " + up);
						System.Diagnostics.Debug.WriteLine("down : " + down);
						System.Diagnostics.Debug.WriteLine("currentSpan : " + currentSpan);

                        if (up > 35.8759735915404 || down < 35.8608966567384 || left < 128.574169621781 || right > 128.587380839745)
                        {
                            nativeMap.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(35.8680838081858, 128.580841355511), currentSpan), true);
                        }
					}
				};

				nativeMap.ZoomEnabled = true;
				nativeMap.ScrollEnabled = true;
				nativeMap.RotateEnabled = false;
				nativeMap.PitchEnabled = false;
				nativeMap.ShowsCompass = false;
				nativeMap.ShowsScale = false;
				nativeMap.ShowsTraffic = false;

				//Debug.WriteLine("LongitudeDelta OnElement : " + nativeMap.Region.Span.LongitudeDelta);
			}
		}
		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;

			var anno = annotation as MKPointAnnotation;
			var customPin = GetCustomPin(anno);
			if (customPin == null)
			{
				throw new Exception("Custom pin not found");
			}

			annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
			if (annotationView == null)
			{
				annotationView = new CustomMKAnnotationView(annotation, customPin.Id);

                var customLabel = new UILabel(new CGRect(0, 0, customPin.width, 25))
                {
                    Text = customPin.Pin.Label,

                    // 2. Set the font name and size:
                    Font = UIFont.FromName("Helvetica-Bold", 20f),

                    // 3. Optionally set additional properties that affect the display
                    AdjustsFontSizeToFitWidth = true, // gets smaller if it doesn't fit
                    MinimumFontSize = 12f, // never gets smaller than this size
                    LineBreakMode = UILineBreakMode.TailTruncation,
                    Lines = 1 // 0 means unlimited
                };
                switch (customPin.Id)
                {
                    case "2지구":
                    case "5지구":
                    case "동산상가":
                    case "상가연합회":
                        customLabel.BackgroundColor = UIColor.Green;
                        customLabel.TextColor = UIColor.Magenta;
                        annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                        break;
                    case "1지구":
                    case "4지구":
                    case "명품프라자":
                    case "건해산물상가":
					case "아진상가":
					case "4지구 대체상가":
                        customLabel.BackgroundColor = UIColor.Gray;
                        customLabel.TextColor = UIColor.Black;
                        break;
                }

                UIGraphics.BeginImageContextWithOptions(customLabel.Bounds.Size, true, 0);
                customLabel.Layer.RenderInContext(UIGraphics.GetCurrentContext());
                var img = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();



				annotationView.Image = img;
				annotationView.CalloutOffset = new CGPoint(0, 0);
				annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("arrows.png"));
				((CustomMKAnnotationView)annotationView).Id = customPin.Id;
				((CustomMKAnnotationView)annotationView).width = customPin.width;
			}
			annotationView.CanShowCallout = true;

			return annotationView;
		}

		void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			var customView = e.View as CustomMKAnnotationView;
			switch (customView.Id)
			{
				case "2지구":
				case "5지구":
				case "동산상가":
				case "상가연합회":
                    formsMap.OnTap(customView.Id);
					break;
			}
		}

		//void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		//{
		//	var customView = e.View as CustomMKAnnotationView;
		//	customPinView = new UIView();

		//	if (customView.Id == "5지구")
		//	{
		//		customPinView.Frame = new CGRect(0, 0, 200, 84);
		//		var image = new UIImageView(new CGRect(0, 0, 200, 84));
		//		image.Image = UIImage.FromFile("xamarin.png");
		//		customPinView.AddSubview(image);
		//		customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
		//		e.View.AddSubview(customPinView);
		//	}
		//}

		//void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		//{
		//	if (!e.View.Selected)
		//	{
		//		customPinView.RemoveFromSuperview();
		//		customPinView.Dispose();
		//		customPinView = null;
		//	}
		//}

		AdvertisementPin GetCustomPin(MKPointAnnotation annotation)
		{
			var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
			foreach (var pin in advertisementPins)
			{
				if (pin.Pin.Position == position)
				{
					return pin;
				}
			}
			return null;
		}
	}
}
