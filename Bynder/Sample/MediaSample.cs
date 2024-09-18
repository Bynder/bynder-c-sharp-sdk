// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Model;

namespace Bynder.Sample
{
    public class MediaSample
    {
        private IBynderClient _bynderClient;

        public static async Task MediaSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new MediaSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunMediaSampleAsync();
        }

        private MediaSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunMediaSampleAsync()
        {
            // Get a list of media with limit 10
            Console.WriteLine("Listing media with limit of 10: ");
            var mediaList = await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery{Limit=10});
            foreach(Media media in mediaList) {
                Console.WriteLine($"Media ID: {media.Id}");
                Console.WriteLine($"Media Name: {media.Name}");
            }

            // Get the media info
            Console.WriteLine("Enter the media ID to get the media info for: ");
            var mediaIdForInfo = Console.ReadLine();
            var mediaInformationQuery = new MediaInformationQuery{
                MediaId = mediaIdForInfo.Trim()
            };
            var mediaInfo = await _bynderClient.GetAssetService().GetMediaInfoAsync(mediaInformationQuery);
            Console.WriteLine($"ID: {mediaInfo.Id}");
            Console.WriteLine($"Name: {mediaInfo.Name}");
            Console.WriteLine($"Brand Id: {mediaInfo.BrandId}");
            Console.WriteLine($"Asset type: {string.Join(',', mediaInfo.PropertyAssetType)}");
            if (mediaInfo.PropertyOptionsDictionary != null)
            {
                foreach (var propertyKey in mediaInfo.PropertyOptionsDictionary.Keys)
                {
                    Console.Write($"Property option in dictionary: {propertyKey}: {mediaInfo.PropertyOptionsDictionary[propertyKey].ToString()}");
                }
            }


            // Get the media download URL
            Console.WriteLine("Enter the media ID to get the media download URL for: ");
            var mediaIdForDownloadUrl = Console.ReadLine();
            var downloadMediaQuery = new DownloadMediaQuery{
                MediaId = mediaIdForDownloadUrl.Trim()
            };
            var download = await _bynderClient.GetAssetService().GetDownloadFileUrlAsync(downloadMediaQuery);
            Console.WriteLine($"Media Download URL: {download}");


            // Modify a media with a new description
            Console.WriteLine("Enter the media ID to modify: ");
            var mediaIdForModify = Console.ReadLine();
            Console.WriteLine("Enter new description to modify for the media: ");
            var updatedDescription = Console.ReadLine();
            var modifyMediaQuery = new ModifyMediaQuery(mediaIdForModify){
                Description = updatedDescription
            };
            await _bynderClient.GetAssetService().ModifyMediaAsync(modifyMediaQuery);


            // Modify a media with a new description
            Console.WriteLine("Enter the media ID to delete: ");
            var mediaIdForDelete = Console.ReadLine();
            await _bynderClient.GetAssetService().DeleteAssetAsync(mediaIdForDelete);

        }
            
        private async Task AuthenticateWithOAuth2Async(bool useClientCredentials)
        {
            if (useClientCredentials)
            {
                await _bynderClient.GetOAuthService().GetAccessTokenAsync();
            }
            else
            {
                Browser.Launch(_bynderClient.GetOAuthService().GetAuthorisationUrl("state example"));
                Console.WriteLine("Insert the code: ");
                var code = Console.ReadLine();
                await _bynderClient.GetOAuthService().GetAccessTokenAsync(code);
            }
        }

    }
}
