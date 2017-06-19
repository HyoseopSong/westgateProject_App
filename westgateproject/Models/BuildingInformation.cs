using System;
using SQLite;

namespace westgateproject.Models
{
    public class BuildingInformation
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Building { get; set; }

		public string BaseFirst { get; set; }
		public string First { get; set; }
		public string Second { get; set; }
		public string Third { get; set; }
		public string Forth { get; set; }
    }
}
