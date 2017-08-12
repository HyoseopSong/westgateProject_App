using System;
namespace westgateproject.Models
{
    public class UserInfoEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
		public UserInfoEntity(string id, string shopName, string shopBuilding, string shopFloor, string shopLocation, string phoneNumber)
		{
			PartitionKey = id;
			RowKey = shopName;

			ShopBuilding = shopBuilding;
			ShopFloor = shopFloor;
			ShopLocation = shopLocation;
			PhoneNumber = phoneNumber;
		}

		public UserInfoEntity() { }


		public string ShopBuilding { get; set; }
		public string ShopFloor { get; set; }
		public string ShopLocation { get; set; }
		public string PhoneNumber { get; set; }
    }
}
