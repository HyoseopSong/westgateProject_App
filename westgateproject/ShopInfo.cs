using System;
using Newtonsoft.Json;

namespace westgateproject
{
	public class ShopInfo
	{
		string partitionkey;
		string rowkey;
		string shopname;
		string phonenumber;
		string managerid;
		string homepage;

		[JsonProperty(PropertyName = "partitionkey")]
		public string PartitionKey
		{
			get { return partitionkey; }
			set { partitionkey = value; }
		}

		[JsonProperty(PropertyName = "rowkey")]
		public string RowKey
		{
			get { return rowkey; }
			set { rowkey = value; }
		}

		[JsonProperty(PropertyName = "shopname")]
		public string ShopName
		{
			get { return shopname; }
			set { shopname = value; }
		}

		[JsonProperty(PropertyName = "phonenumber")]
		public string PhoneNumber
		{
			get { return phonenumber; }
			set { phonenumber = value; }
		}

		[JsonProperty(PropertyName = "managerid")]
		public string ManagerID
		{
			get { return managerid; }
			set { managerid = value; }
		}

		[JsonProperty(PropertyName = "homepage")]
		public string HomePage
		{
			get { return homepage; }
			set { homepage = value; }
		}
	}
}
