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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Bynder.Sample
{
    public class ModifyMediaSample
    {
        private IBynderClient _bynderClient;

        public static async Task ModifyMediaSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new ModifyMediaSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunModifyMediaSampleAsync();
        }

        private ModifyMediaSample(Configuration configuration)
        {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunModifyMediaSampleAsync()
        {
            var assetService = _bynderClient.GetAssetService();
            // Get a list of media with limit 10
            Console.WriteLine("Listing media with limit of 10: ");
            var mediaList = await assetService.GetMediaListAsync(new MediaQuery { Limit = 10 });
            foreach (Media media in mediaList)
            {
                Console.WriteLine($"Media ID: {media.Id}");
                Console.WriteLine($"Media Name: {media.Name}");
            }

            // Get the media info
            Console.WriteLine("Enter the media ID to modify: ");
            var mediaId = Console.ReadLine();
            var mediaInformationQuery = new MediaInformationQuery
            {
                MediaId = mediaId.Trim()
            };
            var mediaInfo = await assetService.GetMediaInfoAsync(mediaInformationQuery);
            Console.WriteLine($"ID: {mediaInfo.Id}");
            Console.WriteLine($"Name: {mediaInfo.Name}");


            // datePublished
            Console.WriteLine($"---\r\nTest with datePublished");
            Console.WriteLine($"datePublished is currently set to: {mediaInfo.DatePublished}");
            Console.WriteLine("New value (use ISO8601 format: yyyy-mm-ddThh:mm:ssZ, or n = now, or leave empty to erase): ");
            var cmd = Console.ReadLine();
            var query = new ModifyMediaQuery(mediaId);
            if (cmd.ToLower().StartsWith("n"))
            {
                query.PublishedDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else if (string.IsNullOrEmpty(cmd))
            {
                query.PublishedDate = "";
            } 
            else
            {
                query.PublishedDate = cmd;
            }
            await assetService.ModifyMediaAsync(query);
            Console.WriteLine("The asset has been modified. It takes a few seconds for changes come through. Hit enter when you want to retrieve the asset again.");
            Console.ReadLine();
            mediaInfo = await assetService.GetMediaInfoAsync(mediaInformationQuery);
            Console.WriteLine($"datePublished is now set to: {mediaInfo.DatePublished}");

            // isArchived (boolean)
            Console.WriteLine($"---\r\nTest with boolean value (isArchived)");
            Console.WriteLine($"isArchived is currently set to: {mediaInfo.IsArchived}");
            Console.WriteLine("New value (t=true, f=false, n=not set): ");
            cmd = Console.ReadLine();
            query = new ModifyMediaQuery(mediaId);
            if (cmd.ToLower().StartsWith("t"))
            {
                query.Archive = true;
            }
            else if (cmd.ToLower().StartsWith("f"))
            {
                query.Archive = false;
            }
            await assetService.ModifyMediaAsync(query);
            Console.WriteLine("The asset has been modified. It takes a few seconds for changes come through. Hit enter when you want to retrieve the asset again.");
            Console.ReadLine();
            mediaInfo = await assetService.GetMediaInfoAsync(mediaInformationQuery);
            Console.WriteLine($"isArchived is now set to: {mediaInfo.IsArchived}");

            // Copyright message
            Console.WriteLine($"---\r\nTest with string value (copyright)");
            Console.WriteLine($"Copyright message is currently set to {mediaInfo.Copyright}");
            Console.WriteLine($"Please supply a new value for the copyright message, or hit enter to erase it");

            cmd = Console.ReadLine();
            query = new ModifyMediaQuery(mediaId);
            query.Copyright = string.IsNullOrEmpty(cmd) ? "" : cmd;
            await assetService.ModifyMediaAsync(query);
            Console.WriteLine("The asset has been modified. It takes a few seconds for changes come through. Hit enter when you want to retrieve the asset again.");
            Console.ReadLine();
            mediaInfo = await assetService.GetMediaInfoAsync(mediaInformationQuery);
            Console.WriteLine($"Copyright message is now set to {mediaInfo.Copyright}");

            // Metaproperties
            if (!(mediaInfo.PropertyOptionsDictionary?.Keys?.Any() ?? false))
            {
                Console.WriteLine("This media item has no meta properties, please choose a different one the next time! Bye for now.");
                return;
            }
            var metaprop = mediaInfo.PropertyOptionsDictionary.FirstOrDefault();
            var metaPropertyName = metaprop.Key.Replace("property_", "");

            Console.WriteLine($"---\r\nTest with metaproperties");
            Console.WriteLine($"Meta property {metaprop.Key} is currently set to {metaprop.Value.ToString()}");
            Console.WriteLine($"Please supply a new value for the meta property, or hit enter to erase it");

            cmd = Console.ReadLine();

            // get ID of the meta property
            var metaProperties = await assetService.GetMetapropertiesAsync();
            var metaProperty = metaProperties.Values.FirstOrDefault(mp => mp.Name == metaPropertyName);
            if (metaProperty == null)
            {
                throw new Exception("Unable to find property with name " + metaprop.Key.Replace("property_", ""));
            }

            query = new ModifyMediaQuery(mediaId);
            query.MetapropertyOptions = new Dictionary<string,IList<string>>()
            {
                {
                    metaProperty.Id,
                    string.IsNullOrEmpty(cmd) ? [] : [cmd]
                }
            };
            await assetService.ModifyMediaAsync(query);
            Console.WriteLine("The asset has been modified. It takes a few seconds for changes come through. Hit enter when you want to retrieve the asset again.");
            Console.ReadLine();
            mediaInfo = await assetService.GetMediaInfoAsync(mediaInformationQuery);
            if (mediaInfo.PropertyOptionsDictionary?.TryGetValue("property_" + metaPropertyName, out JToken value) ?? false)
            {
                Console.WriteLine($"Meta property {metaPropertyName} is now set to {value.ToString()}");
            }
            else
            {
                Console.WriteLine($"Asset has no value for metaproperty {metaPropertyName}");
            }
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
