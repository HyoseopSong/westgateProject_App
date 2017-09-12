using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace westgateproject.Models
{
	public class ContentsEntity : TableEntity
	{
		public ContentsEntity(string id, string blobName, string shopName, string text)
		{
			PartitionKey = id;
			RowKey = blobName;
			ShopName = shopName;
			Context = text;
            LikeMember = "";
            Like = 0;
		}

		public ContentsEntity() { }

		public string ShopName { get; set; }
		public string Context { get; set; }
        public string LikeMember { get; set; }
		public int Like { get; set; }
	}
}
