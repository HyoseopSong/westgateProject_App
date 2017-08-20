using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace westgateproject.Models
{
	public class MyEntity : TableEntity
	{
		public MyEntity() { }

		public MyEntity(string blobName, string shopName, string text)
		{
			PartitionKey = blobName;
			RowKey = shopName;
			Text = text;
		}

		public string Text { get; set; }
	}
}
