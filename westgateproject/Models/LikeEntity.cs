using System;
namespace westgateproject.Models
{
	public class LikeEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
		public LikeEntity() { }

		public LikeEntity(string id, string blobName)
		{
			PartitionKey = id;
			RowKey = blobName;
		}

	}
}
