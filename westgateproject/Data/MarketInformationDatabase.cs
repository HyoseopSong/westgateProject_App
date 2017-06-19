using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using westgateproject.Models;

namespace westgateproject.Data
{
    public class MarketInformationDatabase
    {
        readonly SQLiteAsyncConnection database;

        public MarketInformationDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTablesAsync<ShopInformation, BuildingInformation>().Wait();
        }

        public Task<List<ShopInformation>> GetShopAsync()
        {
            return database.Table<ShopInformation>().ToListAsync();
        }

		public Task<List<BuildingInformation>> GetBuildingAsync()
		{
			return database.Table<BuildingInformation>().ToListAsync();
		}

        public Task<List<ShopInformation>> GetShopInfo(string building, string location)
        {
            return database.QueryAsync<ShopInformation>("SELECT * FROM [ShopInformation] WHERE [Building] = '" + building + "' AND [Location] = '" + location + "'");
        }

		public Task<List<BuildingInformation>> GetBuilding(string building)
		{
			return database.QueryAsync<BuildingInformation>("SELECT * FROM [BuildingInformation] WHERE [Building] = '" + building +"'");
		}

        public Task<ShopInformation> GetShopAsync(string building, string floor, string location)
        {
            return database.Table<ShopInformation>().Where(i => i.Location == location && i.Building == building && i.Floor == floor).FirstOrDefaultAsync();
        }

        public Task<BuildingInformation> GetBuildingAsync(string building)
        {
            return database.Table<BuildingInformation>().Where(i => i.Building == building).FirstOrDefaultAsync();
        }

        public Task<int> SaveShopAsync(ShopInformation item)
        {
            if(item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

		public Task<int> SaveBuildingAsync(BuildingInformation info)
		{
            //var key = database.ExecuteScalarAsync<int>("SELECT last_insert_rowid()");
			if (info.ID != 0)
			{
				return database.UpdateAsync(info);
			}
			else
			{
				return database.InsertAsync(info);
			}
		}


		public Task<int> DeleteShopAsync(ShopInformation item)
        {
            return database.DeleteAsync(item);
        }
    }
}
