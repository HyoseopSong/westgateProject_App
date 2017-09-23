using System;
using System.ComponentModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace westgateproject.Models
{
	public class ContentsEntity : TableEntity, INotifyPropertyChanged
	{
        string likeMember;
        int like;
		public ContentsEntity(string id, string blobName, string shopName, string text)
		{
			PartitionKey = id;
			RowKey = blobName;
			ShopName = shopName;
			Context = text;
            likeMember = "";
            like = 0;
		}


		public ContentsEntity() { }

		public string ShopName { get; set; }
		public string Context { get; set; }
		public int Like
        {
            get
            {
                return like;
            }
            set
            {
                like = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Like"));
            }
        }
		public string LikeMember
        {
            get
            {
                return likeMember;
            }
            set
            {
                likeMember = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LikeMember"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
