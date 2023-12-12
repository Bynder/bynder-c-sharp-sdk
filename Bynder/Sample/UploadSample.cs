// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Query.Upload;
namespace Bynder.Sample
{
    public class UploadSample
    {
        private IBynderClient _bynderClient;

        public static async Task UploadSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new UploadSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunUploadSampleAsync();
        }

        private UploadSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunUploadSampleAsync()
        {
            Console.WriteLine("Enter the path of the file to upload: ");
            var uploadPath = Console.ReadLine();
            var assetService = _bynderClient.GetAssetService();

            var brands = await assetService.GetBrandsAsync();
            if (!brands.Any())
            {
                Console.Error.WriteLine("No brands found in this account.");
                return;
            }

            await assetService.UploadFileAsync(new UploadQuery { Filepath = uploadPath, BrandId = brands.First().Id });
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
