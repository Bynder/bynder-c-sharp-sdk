// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Query.Asset;

namespace Bynder.Sample
{
    public class AssetUsageSample
    {
        private IBynderClient _bynderClient;

        public static async Task AssetUsageSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new AssetUsageSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunAssetUsageSampleAsync();
        }

        private AssetUsageSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunAssetUsageSampleAsync()
        {
            Console.WriteLine("Enter the media ID to create asset usage for: ");
            var createAssetUsageMediaId = Console.ReadLine();
            Console.WriteLine("Enter the integration ID to create the asset usage for: ");
            var createAssetUsageIntegrationId = Console.ReadLine();
            await _bynderClient.GetAssetService().CreateAssetUsage(new AssetUsageQuery(createAssetUsageIntegrationId, createAssetUsageMediaId));

            
            Console.WriteLine("Enter the media ID to create delete usage from: ");
            var deleteAssetUsageMediaId = Console.ReadLine();
            Console.WriteLine("Enter the integration ID to delete the asset usage from: ");
            var deleteAssetUsageIntegrationId = Console.ReadLine();
             await _bynderClient.GetAssetService().DeleteAssetUsage(new AssetUsageQuery(deleteAssetUsageIntegrationId, deleteAssetUsageMediaId));
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
