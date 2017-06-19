using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly UITapGestureRecognizer _tapRecogniser;
		private IMKAnnotation[] annotations;
		public WestGateMarketMapRenderer()
		{
			_tapRecogniser = new UITapGestureRecognizer(OnTap)
			{
				NumberOfTapsRequired = 1,
				NumberOfTouchesRequired = 1
			};
		}


		private void OnTap(UITapGestureRecognizer recognizer)
		{
			var cgPoint = recognizer.LocationInView(Control);

			var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

			((WestGateMarketMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{

			base.OnElementChanged(e);
			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				nativeMap.RemoveGestureRecognizer(_tapRecogniser);

				nativeMap.GetViewForAnnotation = null;
			}

			if (e.NewElement != null)
			{
				var formsMap = (WestGateMarketMap)e.NewElement;
				var nativeMap = Control as MKMapView;



				nativeMap.AddGestureRecognizer(_tapRecogniser);
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
				var isRemoved = false;

				nativeMap.RegionChanged += (sender, _e) =>
				{
					if ((nativeMap.Region.Span.LongitudeDelta < 0.007) && (isRemoved == true))
					{
						foreach (var _an in annotations)
						{

							//nativeMap.AddAnnotation(_an);
						}
						isRemoved = false;
					}
					else if ((nativeMap.Region.Span.LongitudeDelta >= 0.007) && (isRemoved == false))
					{
						annotations = nativeMap.Annotations;
						nativeMap.RemoveAnnotations(nativeMap.Annotations);
						isRemoved = true;
					}
					var currentRegion = nativeMap.Region;
					if (currentRegion.Span.LongitudeDelta > 0.007)
					{
						nativeMap.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(35.8687925, 128.5801115), new MKCoordinateSpan(0, 0.00453209582661884)), true);
					}
					else
					{
						var halfLatDel = currentRegion.Span.LatitudeDelta / 2;
						var halfLonDel = currentRegion.Span.LongitudeDelta / 2;
						var currentSpan = currentRegion.Span;

						var up = currentRegion.Center.Latitude + halfLatDel;
						var down = currentRegion.Center.Latitude - halfLatDel;
						var left = currentRegion.Center.Longitude - halfLonDel;
						var right = currentRegion.Center.Longitude + halfLonDel;

						if (up > 35.8717059984586 || down < 35.8657124078645 || left < 128.577723763883 || right > 128.582757264376)
							nativeMap.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(35.8687925, 128.5801115), currentSpan), true);

					}
				};

				nativeMap.ZoomEnabled = true;
				nativeMap.ScrollEnabled = true;
				nativeMap.RotateEnabled = false;
				nativeMap.PitchEnabled = false;
				nativeMap.ShowsCompass = false;
				nativeMap.ShowsScale = false;
				nativeMap.ShowsTraffic = false;

			}
		}
	}
}
