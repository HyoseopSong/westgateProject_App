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
            database.CreateTablesAsync<ShopInforSQLDb, BuildingInformation>().Wait();
        }

        public Task<List<ShopInforSQLDb>> GetShopAsync()
        {
            return database.Table<ShopInforSQLDb>().ToListAsync();
        }

		public Task<List<BuildingInformation>> GetBuildingAsync()
		{
			return database.Table<BuildingInformation>().ToListAsync();
		}

        public Task<List<ShopInforSQLDb>> GetShopInfo(string building, string location)
        {
            return database.QueryAsync<ShopInforSQLDb>("SELECT * FROM [ShopInformation] WHERE [Building] = '" + building + "' AND [Location] = '" + location + "'");
        }

		public Task<List<BuildingInformation>> GetBuilding(string building)
		{
			return database.QueryAsync<BuildingInformation>("SELECT * FROM [BuildingInformation] WHERE [Building] = '" + building +"'");
		}

        public Task<ShopInforSQLDb> GetShopAsync(string building, string floor, string location)
        {
            return database.Table<ShopInforSQLDb>().Where(i => i.Location == location && i.Building == building && i.Floor == floor).FirstOrDefaultAsync();
        }

        public Task<BuildingInformation> GetBuildingAsync(string building)
        {
            return database.Table<BuildingInformation>().Where(i => i.Building == building).FirstOrDefaultAsync();
        }

        public Task<int> SaveShopAsync(ShopInforSQLDb item)
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


		public Task<int> DeleteShopAsync(ShopInforSQLDb item)
        {
            return database.DeleteAsync(item);
        }
    }
}
