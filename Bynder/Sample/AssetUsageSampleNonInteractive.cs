// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Service;
using Bynder.Sdk.Settings;

namespace Bynder.Sample
{
    /// <summary>
    /// Non-interactive variant of <see cref="AssetUsageSample"/>, using client credentials.
    /// Picks the first media item found, creates an asset usage with a URI, then deletes it.
    /// The integration ID is read from the ASSET_USAGE_INTEGRATION_ID environment variable.
    /// </summary>
    public class AssetUsageSampleNonInteractive
    {
        private IBynderClient _bynderClient;

        public static async Task AssetUsageSampleNonInteractiveAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new AssetUsageSampleNonInteractive(configuration);
            await apiSample._bynderClient.GetOAuthService().GetAccessTokenAsync();
            await apiSample.RunAssetUsageSampleAsync();
        }

        private AssetUsageSampleNonInteractive(Configuration configuration)
        {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunAssetUsageSampleAsync()
        {
            var integrationId = Environment.GetEnvironmentVariable("ASSET_USAGE_INTEGRATION_ID");
            if (string.IsNullOrEmpty(integrationId))
            {
                Console.WriteLine("Set the ASSET_USAGE_INTEGRATION_ID environment variable.");
                return;
            }

            var media = (await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery { Limit = 1 })).FirstOrDefault();
            if (media == null)
            {
                Console.WriteLine("No media found to create an asset usage for.");
                return;
            }

            var uri = "https://example.com/asset-usage-sample";
            Console.WriteLine($"Creating asset usage for media {media.Id} ({media.Name}), integration {integrationId}, uri {uri}");
            var created = await _bynderClient.GetAssetService().CreateAssetUsage(new AssetUsageQuery(integrationId, media.Id) { Uri = uri });
            Console.WriteLine($"Create: {created?.StatusCode} {created?.Message} (no exception = accepted)");

            Console.WriteLine("Deleting asset usage");
            var deleted = await _bynderClient.GetAssetService().DeleteAssetUsage(new AssetUsageQuery(integrationId, media.Id) { Uri = uri });
            Console.WriteLine($"Delete: {deleted?.StatusCode} {deleted?.Message} (no exception = accepted)");
        }
    }
}
