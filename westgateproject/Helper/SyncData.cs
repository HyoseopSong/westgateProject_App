using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.Helper
{
    public class SyncData
    {
        public SyncData()
        {
        }

        static async public Task<bool> SaveAccountInfo(string ShopName, string PhoneNumber)
        {

			IDictionary<string, string> postDictionary = new Dictionary<string, string>
			{
				{ "id", App.userEmail},
				{ "shopName", ShopName },
				{ "phoneNumber", PhoneNumber}
			};
			await App.Client.InvokeApiAsync("userinformation", System.Net.Http.HttpMethod.Post, postDictionary);

			return true;
        }


        static async public Task<bool> DeleteContents(string blobName)
        {

	        Dictionary<string, string> getParam = new Dictionary<string, string>
	        {
	            { "id", App.userEmail},
	            { "blobName", blobName},
	        };
	        await App.Client.InvokeApiAsync<Dictionary<string, string>>("upload", System.Net.Http.HttpMethod.Delete, getParam);

			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.StorageConnectionString);

			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve reference to a previously created container.
			var containerName = App.userEmail.Split('@');
			CloudBlobContainer container = blobClient.GetContainerReference(containerName[0]);

			// Retrieve reference to a blob named "myblob.txt".
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			// Delete the blob.
			await blockBlob.DeleteIfExistsAsync();

            return true;
        }


		static async public Task<string> UploadContents(MediaFile img, string text)
		{
            var blobName = DateTime.Now.ToFileTime().ToString();
            if (img != null && text != null)
            {

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                var containerName = App.userEmail.Split('@');
                CloudBlobContainer container = blobClient.GetContainerReference(containerName[0]);
				await container.CreateIfNotExistsAsync();
				CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName + ".jpg");
                blockBlob.Properties.ContentType = "image/jpeg";
				//await blockBlob.UploadFromFileAsync(img.Path);
                await blockBlob.UploadFromStreamAsync(img.GetStream());



				IDictionary<string, string> postDictionary = new Dictionary<string, string>
				{
					{ "content", text },
					{ "id", App.userEmail},
					{ "blobName", blobName + ".jpg"}
				};
				await App.Client.InvokeApiAsync("upload", System.Net.Http.HttpMethod.Post, postDictionary);

				return blobName;

            }
            else
            {
                return null;
            }

		}

		static async public Task<bool> SyncShopInfo()
		{
			IDictionary<string, string> getResult = new Dictionary<string, string>();
			try
			{
				getResult = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getShopInformation", System.Net.Http.HttpMethod.Get, null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType());
                return false;
			}
			foreach (var shopInfo in getResult)
			{
				string[] building = shopInfo.Key.Split(':');
				string[] shop = shopInfo.Value.Split(':');
				ShopInformation result = new ShopInformation(building[0], building[1], building[2], shop[0], shop[1]);

				var res = await App.Database.GetShopAsync(building[0], building[1], building[2]);

				if (res != null)
				{
					result.ID = res.ID;
				}

				await App.Database.SaveShopAsync(result);
			}
            return true;
		}

		static async public Task<bool> SyncBuildingInfo()
		{
			IDictionary<string, string> result = new Dictionary<string, string>();
			try
			{
				result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getBuildingInformation", System.Net.Http.HttpMethod.Get, null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType());
                return false;
			}
            BuildingInformation info1 = new BuildingInformation()
            {
                Building = "1지구"
            };
            BuildingInformation info2 = new BuildingInformation()
            {
                Building = "2지구"
            };
            BuildingInformation info4 = new BuildingInformation()
            {
                Building = "4지구"
            };
            BuildingInformation info5 = new BuildingInformation()
            {
                Building = "5지구"
            };
            BuildingInformation infoSea = new BuildingInformation()
            {
                Building = "건해산물상가"
            };
            BuildingInformation infoDongsan = new BuildingInformation()
            {
                Building = "동산상가"
            };
            BuildingInformation infomp = new BuildingInformation()
            {
                Building = "명품프라자"
            };
            BuildingInformation infoUnion = new BuildingInformation()
            {
                Building = "상가연합회"
            };
            BuildingInformation infoAjin = new BuildingInformation()
            {
                Building = "아진상가"
            };
            foreach (var floorInfo in result)
			{
				string[] building = floorInfo.Key.Split(':');
				string resultString = "";
				resultString = floorInfo.Value;

				int retrievedID;
				var res = await App.Database.GetBuildingAsync(building[0]);

				if (res != null)
					retrievedID = res.ID;
				else
					retrievedID = 0;

				switch (building[0])
				{
					case "1지구":
						info1.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								info1.BaseFirst = resultString;
								break;
							case "지상1층":
								info1.First = resultString;
								break;
							case "지상2층":
								info1.Second = resultString;
								break;
							case "지상3층":
								info1.Third = resultString;
								break;
							case "지상4층":
								info1.Forth = resultString;
								break;
						}
						break;
					case "2지구":
						info2.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								info2.BaseFirst = resultString;
								break;
							case "지상1층":
								info2.First = resultString;
								break;
							case "지상2층":
								info2.Second = resultString;
								break;
							case "지상3층":
								info2.Third = resultString;
								break;
							case "지상4층":
								info2.Forth = resultString;
								break;
						}
						break;
					case "4지구":
						info4.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								info4.BaseFirst = resultString;
								break;
							case "지상1층":
								info4.First = resultString;
								break;
							case "지상2층":
								info4.Second = resultString;
								break;
							case "지상3층":
								info4.Third = resultString;
								break;
							case "지상4층":
								info4.Forth = resultString;
								break;
						}
						break;
					case "5지구":
						info5.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								info5.BaseFirst = resultString;
								break;
							case "지상1층":
								info5.First = resultString;
								break;
							case "지상2층":
								info5.Second = resultString;
								break;
							case "지상3층":
								info5.Third = resultString;
								break;
							case "지상4층":
								info5.Forth = resultString;
								break;
						}
						break;
					case "건해산물상가":
						infoSea.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								infoSea.BaseFirst = resultString;
								break;
							case "지상1층":
								infoSea.First = resultString;
								break;
							case "지상2층":
								infoSea.Second = resultString;
								break;
							case "지상3층":
								infoSea.Third = resultString;
								break;
							case "지상4층":
								infoSea.Forth = resultString;
								break;
						}
						break;
					case "동산상가":
						infoDongsan.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								infoDongsan.BaseFirst = resultString;
								break;
							case "지상1층":
								infoDongsan.First = resultString;
								break;
							case "지상2층":
								infoDongsan.Second = resultString;
								break;
							case "지상3층":
								infoDongsan.Third = resultString;
								break;
							case "지상4층":
								infoDongsan.Forth = resultString;
								break;
						}
						break;
					case "명품프라자":
						infomp.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								infomp.BaseFirst = resultString;
								break;
							case "지상1층":
								infomp.First = resultString;
								break;
							case "지상2층":
								infomp.Second = resultString;
								break;
							case "지상3층":
								infomp.Third = resultString;
								break;
							case "지상4층":
								infomp.Forth = resultString;
								break;
						}
						break;
					case "상가연합회":
						infoUnion.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								infoUnion.BaseFirst = resultString;
								break;
							case "지상1층":
								infoUnion.First = resultString;
								break;
							case "지상2층":
								infoUnion.Second = resultString;
								break;
							case "지상3층":
								infoUnion.Third = resultString;
								break;
							case "지상4층":
								infoUnion.Forth = resultString;
								break;
						}
						break;
					case "아진상가":
						infoAjin.ID = retrievedID;
						switch (building[1])
						{
							case "지하1층":
								infoAjin.BaseFirst = resultString;
								break;
							case "지상1층":
								infoAjin.First = resultString;
								break;
							case "지상2층":
								infoAjin.Second = resultString;
								break;
							case "지상3층":
								infoAjin.Third = resultString;
								break;
							case "지상4층":
								infoAjin.Forth = resultString;
								break;
						}
						break;
					default:
						break;
				}
			}
			await App.Database.SaveBuildingAsync(info1);
			await App.Database.SaveBuildingAsync(info2);
			await App.Database.SaveBuildingAsync(info4);
			await App.Database.SaveBuildingAsync(info5);
			await App.Database.SaveBuildingAsync(infoSea);
			await App.Database.SaveBuildingAsync(infoDongsan);
			await App.Database.SaveBuildingAsync(infomp);
			await App.Database.SaveBuildingAsync(infoUnion);
			await App.Database.SaveBuildingAsync(infoAjin);
			return true;
		}
    }
}
