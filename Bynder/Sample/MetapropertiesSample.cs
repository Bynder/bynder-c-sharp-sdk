// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Model;
using System.Collections.Generic;
using Bynder.Sdk.Query.Asset;

namespace Bynder.Sample
{
    public class MetapropertiesSample
    {
        private IBynderClient _bynderClient;

        public static async Task MetapropertiesSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new MetapropertiesSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunMetapropertiesSampleAsync();
        }

        private MetapropertiesSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunMetapropertiesSampleAsync()
        {
            // Sample to list metaproperties
            var metapropertiesList = await _bynderClient.GetAssetService().GetMetapropertiesAsync();
            foreach (KeyValuePair<string,Metaproperty> metaproperty in metapropertiesList) {
                Console.WriteLine($"ID: {metaproperty.Value.Id}");
                Console.WriteLine($"Name: {metaproperty.Value.Name}");
                Console.WriteLine($"Label: {metaproperty.Value.Label}");
                if(metaproperty.Value.Options.Count > 0) {
                    Console.WriteLine("Metaproperty Options:");
                    foreach (MetapropertyOption option in metaproperty.Value.Options)
                    {
                        Console.WriteLine($"   ID: {option.Id}");
                        Console.WriteLine($"   Name: {option.Name}");
                        Console.WriteLine($"   Label: {option.Label}");
                        Console.WriteLine(" ");
                    }
                }
                Console.WriteLine(" ");
            }

            // Get metaproperty by ID
            Console.WriteLine("Enter the metaproperty ID to get info for: ");
            var metapropertyIdGetInfo = Console.ReadLine();
            var metapropertyInfo = await _bynderClient.GetAssetService().GetMetapropertyAsync(new MetapropertyQuery(metapropertyIdGetInfo.Trim()));
            Console.WriteLine($"ID: {metapropertyInfo.Id}");
            Console.WriteLine($"Name: {metapropertyInfo.Name}");
            Console.WriteLine($"Label: {metapropertyInfo.Label}");

            // Get metaproperty dependencies by metaproperty ID
            Console.WriteLine("Enter the metaproperty ID to get dependencies for: ");
            var metapropertyIdGetDependencies = Console.ReadLine();
            var metapropertyDependencies = await _bynderClient.GetAssetService().GetMetapropertyDependenciesAsync(new MetapropertyQuery(metapropertyIdGetDependencies.Trim()));
            Console.WriteLine($"Metaproperty Dependencies: [{string.Join(", ", metapropertyDependencies)}]");
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
