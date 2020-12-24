// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;

namespace Bynder.Sample
{
    /// <summary>
    /// Class to show a little example on how to use the Bynder C# SDK. Before running this example is important that you fill
    /// API_BASE_URL, CONSUMER_KEY and CONSUMER_SECRET in App.config
    /// </summary>
    public class ApiSample
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">arguments to main</param>
        public static async Task Main(string[] args)
        {
            Configuration configuration = Configuration.FromJson("Config.json");
            using var client = ClientFactory.Create(configuration);
            if (configuration.PermanentToken != null)
            {
                await QueryBynder(client);
            }
            else
            {
                await QueryBynderUsingOAuth(client);
            }
        }

        private static async Task QueryBynder(IBynderClient client)
        {
            var mediaList = await client.GetAssetService().GetMediaListAsync(new MediaQuery());
            foreach (var media in mediaList)
            {
                Console.WriteLine(media.Name);
            }

            var collectionList = await client.GetCollectionService().GetCollectionsAsync(new GetCollectionsQuery());
            foreach (var collection in collectionList)
            {
                Console.WriteLine(collection.Name);
            }
        }

        private static async Task QueryBynderUsingOAuth(IBynderClient client)
        {
            using var waitForToken = new WaitForToken();
            using var listener = new UrlHttpListener("http://localhost:8888/", waitForToken);

            Browser.Launch(client.GetOAuthService().GetAuthorisationUrl("state example", "offline asset:read collection:read"));
            waitForToken.WaitHandle.WaitOne(50000);

            if (waitForToken.Success)
            {
                client.GetOAuthService().GetAccessTokenAsync(waitForToken.Token, "offline asset:read collection:read").Wait();
                await QueryBynder(client);
            }
        }

    }
}
