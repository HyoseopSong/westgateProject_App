using System;
namespace westgateproject.Models
{
    public class UserInfoEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
		public UserInfoEntity(string id, string shopLocation, string shopName, string phoneNumber, string addInfo, string payment, string homepage)
		{
			PartitionKey = id;
			RowKey = shopLocation;

			ShopName = shopName;
			PhoneNumber = phoneNumber;
			AddInfo = addInfo;
            Payment = payment;
            Homepage = homepage;
		}

		public UserInfoEntity() { }


		public string ShopName { get; set; }
		public string PhoneNumber { get; set; }
		public string AddInfo { get; set; }
		public string Payment { get; set; }
        public string Homepage { get; set; }
        public DateTime Period { get; set; }
        public bool Paid { get; set; }
    }
}
