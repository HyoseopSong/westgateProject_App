using System;
using SQLite;

namespace westgateproject.Models
{
    public class ShopInformation
    {
        public ShopInformation()
        {}

        public ShopInformation(string building, string floor, string location, string shopname, string phonenumber)
        {
            Building = building;
            Floor = floor;
            Location = location;
            ShopName = shopname;
            PhoneNumber = phonenumber;
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }

        public string Location { get; set; }
        public string ShopName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
