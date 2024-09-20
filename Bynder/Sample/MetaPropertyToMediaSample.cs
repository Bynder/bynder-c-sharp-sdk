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
using System.Text;

namespace Bynder.Sample
{
    public class MetaPropertyToMediaSample
    {
        private IBynderClient _bynderClient;

        public static async Task MetaPropertyToMediaSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            Console.WriteLine($"BaseUrl: {configuration.BaseUrl}");
            var apiSample = new MetaPropertyToMediaSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunMetaPropertyToMediaSampleAsync();
        }

        private MetaPropertyToMediaSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunMetaPropertyToMediaSampleAsync()
        {
            var assetService = _bynderClient.GetAssetService();
            // Get a list of media with limit 10
            Console.WriteLine("Available metaproperties: ");
            var metaProperties  = await assetService.GetMetapropertiesAsync();
            int i = 0;
            foreach(var metaProperty in metaProperties.Values) {
                Console.WriteLine($"({++i}) MetaProperty {metaProperty.Name} ({metaProperty.Id})");
            }
            Console.WriteLine("Enter number of the metaProperty to search by");
            var metaPropertyNr = Convert.ToInt32(Console.ReadLine());
            var metaPropertySelected = metaProperties.Skip(metaPropertyNr - 1).FirstOrDefault().Value;
            i = 0;
            foreach (var option in metaPropertySelected.Options)
            {
                Console.WriteLine($"({++i}) Option {option.Name} ({option.Id})");
            }
            Console.WriteLine("Enter number of the option to search by, or a text value");
            var optionSearchText = Console.ReadLine();
            if (Int32.TryParse(optionSearchText, out int optionNr))
            {
                var optionSelected = metaPropertySelected.Options.Skip(optionNr - 1).FirstOrDefault();
                optionSearchText = optionSelected.Name;
            }

            // Get matching media (assets)
            var mediaQuery = new MediaQuery()
            {
                MetaProperties = new Dictionary<string, IList<string>> { { metaPropertySelected.Name, [optionSearchText] } },
                Limit = 10
            };
            var mediaList = await _bynderClient.GetAssetService().GetMediaListAsync(mediaQuery);
            foreach (var media in mediaList)
            {
                Console.WriteLine($"ID: {media.Id}");
                Console.WriteLine($"Name: {media.Name}");
                Console.WriteLine($"Meta properties: {ShowMetaProperties(media.PropertyOptionsDictionary)}");
                Console.WriteLine("-----------------");
            }
        }

        private string ShowMetaProperties(Dictionary<string, JToken> propertyOptionsDictionary)
        {
            if (propertyOptionsDictionary == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var key in propertyOptionsDictionary.Keys)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append("|");
                }
                sb.Append($"{key.Replace("property_","")}:{string.Join(',', propertyOptionsDictionary[key].Select(a => a.ToString()))}");
            }
            return sb.ToString();
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
