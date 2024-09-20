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

namespace Bynder.Sample
{
    public class FindMediaSample
    {
        private IBynderClient _bynderClient;

        public static async Task MediaSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new FindMediaSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunFindMediaSampleAsync();
        }

        private FindMediaSample(Configuration configuration)
        {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunFindMediaSampleAsync()
        {
            var metaProperties = await _bynderClient.GetAssetService().GetMetapropertiesAsync();
            bool c = true;
            while (c)
            {
                await PerformSearch(metaProperties);
                Console.WriteLine("Do you want to perform another search? (y/N)");
                var inp = Console.ReadLine();
                c = inp.ToLower().StartsWith("y");
            }
        }

        private async Task SearchCustom()
        {
            Console.WriteLine("Enter metaproperty name: ");
            var mpName = Console.ReadLine();
            Console.WriteLine("Enter option name: ");
            var optionName = Console.ReadLine();
            Console.WriteLine($"Searching via the meta property named {mpName} and option named {optionName}");
            var assets = await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery()
            {
                MetaProperties = new Dictionary<string, IList<string>>
                    {
                        {
                            mpName, [ optionName ]
                        }
                    }
            });

            Console.WriteLine($"Found {assets.Count()} assets");
            Console.WriteLine("Do you want to search again? (y/N)");
            var again = Console.ReadLine();
            if (again.ToLower().StartsWith("y"))
            {
                await SearchCustom();
            }
        }

        private async Task PerformSearch(IDictionary<string, Metaproperty> metaProperties)
        {
            await SearchCustom();


            Console.WriteLine("You have the following meta properties in your Bynder environment: ");
            var mpKeys = metaProperties.Keys.OrderBy(k => k);
            var counter = 1;
            foreach (var metaProperty in metaProperties.OrderBy(mp => mp.Key))
            {
                var extraInfo = metaProperty.Value.Options?.Any() ?? false ? $"[with {metaProperty.Value.Options.Count()} options]" : "[without options]";
                Console.WriteLine($"{counter++}) {metaProperty.Key} {extraInfo}");
            }
            Console.WriteLine("Type the number of the meta property to perform a search with: ");
            var mpNrInput = Console.ReadLine();
            if (!int.TryParse(mpNrInput, out int mpNr))
            {
                mpNr = 1;
            }
            var selectedMetaPropertyKey = mpKeys.Skip(mpNr - 1).FirstOrDefault();
            var selectedMetaProperty = metaProperties[selectedMetaPropertyKey];
            if (selectedMetaProperty == null)
            {
                Console.WriteLine("No meta property found, stopping execution");

                return;
            }

            string searchString = null;
            if (selectedMetaProperty.Options?.Any() ?? false)
            {
                counter = 1;
                var sortedOptions = selectedMetaProperty.Options.OrderBy(o => o.Label);
                foreach (var option in sortedOptions)
                {
                    Console.WriteLine($"{counter++}) {option.Label}");
                }
                Console.WriteLine("Type the number of the option to search for: ");
                mpNrInput = Console.ReadLine();
                if (!int.TryParse(mpNrInput, out mpNr))
                {
                    mpNr = 1;
                }
                var selectedOption = sortedOptions.Skip(mpNr - 1).FirstOrDefault();
                searchString = selectedOption.Name;

                Console.WriteLine($"Searching via the meta property named {selectedMetaProperty.Name} and option named {searchString}");
                var assets = await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery()
                {
                    MetaProperties = new Dictionary<string, IList<string>>
                    {
                        {
                            selectedMetaProperty.Name, [ searchString ]
                        }
                    }
                });

                if (assets?.Any() ?? false)
                {
                    Console.WriteLine($"Found {assets.Count} assets, showing first 5");
                    counter = 1;
                    foreach (var asset in assets)
                    {
                        Console.WriteLine($"{counter++}) {asset.Name}");
                        if (counter == 6)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No assets found by metaproperty name / option name");
                }

                Console.WriteLine($"Searching via the meta property option ID {selectedOption.Id}");
                assets = await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery()
                {
                    PropertyOptionId = [selectedOption.Id]
                });

                if (assets?.Any() ?? false)
                {
                    Console.WriteLine($"Found {assets.Count} assets, showing first 5");
                    counter = 1;
                    foreach (var asset in assets)
                    {
                        Console.WriteLine($"{counter++}) {asset.Name}");
                        if (counter == 6)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No assets found by metaproperty option id");
                }

            }
            else
            {
                Console.WriteLine("String to search for: ");
                searchString = Console.ReadLine();
            }
            Console.WriteLine("Searching by keyword");
            var assetsByKeyword = await _bynderClient.GetAssetService().GetMediaListAsync(new MediaQuery()
            {
                Keyword = searchString
            }
            );
            if (assetsByKeyword?.Any() ?? false)
            {
                Console.WriteLine($"Found {assetsByKeyword.Count} assets, showing first 5");
                counter = 1;
                foreach (var asset in assetsByKeyword)
                {
                    Console.WriteLine($"{counter++}) {asset.Name}");
                    if (counter == 6)
                    {
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("No assets found by keyword");
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
