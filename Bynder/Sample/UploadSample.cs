// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Query.Upload;
using System.Collections.Generic;
using System.IO;
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

            Console.WriteLine("Name of the media item after upload: ");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name)) 
            {
                name = null; 
            }

            Console.WriteLine("Override original filename (leave empty to use the actual filename): ");
            var filename = Console.ReadLine();

            Console.WriteLine("Description (leave empty to use default): ");
            var description = Console.ReadLine();

            var customParameters = GetCustomParameters();

            Console.WriteLine("Do you want to pass a file stream to the SDK?: (y/n)");
            var passAsStream = Console.ReadLine().ToLower().StartsWith("y");


            var query = new UploadQuery
            {
                Filepath = uploadPath,
                BrandId = brands.First().Id,
                Name = name,
                CustomParameters = customParameters
            };
            if (!string.IsNullOrEmpty(filename))
            {
                query.OriginalFileName = filename;
            }
            if (!string.IsNullOrEmpty(description))
            {
                query.Description = description;
            }

           
            query.MetapropertyOptions = new Dictionary<string,IList<string>>()
            {
                { "a", [ "b" ]}
            };

            FileStream fileStream = null;
            if (passAsStream)
            {
                fileStream = File.Open(query.Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            }
            var before = DateTime.Now;
            var response = passAsStream ? await assetService.UploadFileAsync(fileStream, query) : await assetService.UploadFileAsync(query);
            var ms = Math.Round((DateTime.Now - before).TotalMilliseconds);
            Console.WriteLine($"Uploaded file as media with id {response.MediaId} (time elapsed: {ms})");

        }

        private Dictionary<string,string> GetCustomParameters()
        {
            Console.WriteLine("Do you want to add custom parameters during the upload? (y/n)");
            var input = Console.ReadLine();

            if (!input.ToString().ToLower().StartsWith("y"))
            {
                return null;
            }

            Dictionary<string,string> parameters = new Dictionary<string,string>();
            while (input.ToString().ToLower().StartsWith("y"))
            {
                Console.WriteLine("Parameter name: ");
                var paramName = Console.ReadLine();
                Console.WriteLine("Parameter value: ");
                var paramValue = Console.ReadLine();
                parameters.Add(paramName, paramValue);
                Console.WriteLine("Do you want to add another custom parameter? (y/n)");
                input = Console.ReadLine();
            }
            return parameters;
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
