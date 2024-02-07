// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;

namespace Bynder.Sample
{
    public class BrandsSample
    {
        private IBynderClient _bynderClient;

        public static async Task BrandsSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new BrandsSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunBrandsSampleAsync();
        }

        private BrandsSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunBrandsSampleAsync()
        {
            var brands = await _bynderClient.GetAssetService().GetBrandsAsync();
            Console.WriteLine($"Brands: [{string.Join(", ", brands.Select(m => m.Name))}]");
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
