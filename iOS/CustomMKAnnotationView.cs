using System;
using MapKit;

namespace westgateproject.iOS
{
    public class CustomMKAnnotationView : MKAnnotationView
    {
		public string Id { get; set; }

		public float width { get; set; }

		public CustomMKAnnotationView(IMKAnnotation annotation, string id)
			: base(annotation, id)
		{
		}
    }
}
