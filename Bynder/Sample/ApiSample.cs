// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Model;
using System.Net.Http;

namespace Bynder.Sample
{
    /// <summary>
    /// Class to show a little example on how to use the Bynder C# SDK. Before running this example is important that you fill
    /// API_BASE_URL, CONSUMER_KEY and CONSUMER_SECRET in App.config
    /// </summary>
    public class ApiSample
    {
        private IBynderClient _bynderClient;

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">arguments to main</param>
        public static async Task Main(string[] args)
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new ApiSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.ListItemsAsync();
            await apiSample.UploadFileAsync("/path/to/file.ext");
        }

        private ApiSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task ListItemsAsync()
        {
            var brands = await _bynderClient.GetAssetService().GetBrandsAsync();
            Console.WriteLine($"Brands: [{string.Join(", ", brands.Select(m => m.Name))}]");

            var mediaList = await _bynderClient.GetAssetService().GetMediaListAsync(
                new MediaQuery
                {
                    Type = AssetType.Image,
                    Limit = 10,
                    Page = 1,
                }
            );
            Console.WriteLine($"Assets: [{string.Join(", ", mediaList.Select(m => m.Name))}]");

            var collectionList = await _bynderClient.GetCollectionService().GetCollectionsAsync(
                new GetCollectionsQuery
                {
                    Limit = 10,
                    Page = 1,
                }
            );
            Console.WriteLine($"Collections: [{string.Join(", ", mediaList.Select(m => m.Name))}]");
        }

        private async Task UploadFileAsync(string uploadPath)
        {
            const int maxRetry = 10;

            var assetService = _bynderClient.GetAssetService();

            var brands = await assetService.GetBrandsAsync();
            if (!brands.Any())
            {
                Console.Error.WriteLine("No brands found in this account.");
                return;
            }

            var saveMediaResponse = await assetService.UploadFileToNewAssetAsync(uploadPath, brands.First().Id);
            Console.WriteLine($"Asset uploaded: {saveMediaResponse.MediaId}");

            Media media = null;
            for (int iterations = maxRetry; iterations > 0; --iterations)
            {
                try
                {
                    media = await assetService.GetMediaInfoAsync(
                        new MediaInformationQuery
                        {
                            MediaId = saveMediaResponse.MediaId,
                        }
                    );

                }
                catch (HttpRequestException)
                {
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
            if (media == null)
            {
                Console.Error.WriteLine("The asset could not be retrieved");
                return;
            }

            var saveMediaVersionResponse = await assetService.UploadFileToExistingAssetAsync(uploadPath, media.Id);
            Console.WriteLine($"New asset version uploaded: {saveMediaVersionResponse.MediaId}");
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
