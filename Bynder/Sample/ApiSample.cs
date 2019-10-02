// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Configuration;
using Bynder.Sdk;
using Bynder.Sdk.Service;
using Bynder.Sample;
using Bynder.Sample.Utils;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Settings;

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
        public static void Main(string[] args)
        {
            using(var waitForToken = new WaitForToken())
            {
                using (var listener = new UrlHttpListener("http://localhost:8888/", waitForToken))
                {
                    using (var client = ClientFactory.Create(Configuration.FromJson("Config.json")))
                    {
                        Browser.Launch(client.GetOAuthService().GetAuthorisationUrl("state example", "openid offline"));
                        waitForToken.WaitHandle.WaitOne(50000);

                        if (waitForToken.Success)
                        {
                            client.GetOAuthService().GetAccessTokenAsync(waitForToken.Token, "openid offline").Wait();

                            var mediaList = client.GetAssetService().GetMediaListAsync(new MediaQuery()).Result;

                            foreach (var media in mediaList)
                            {
                                Console.WriteLine(media.Name);
                            }

                            var collectionList = client.GetCollectionService().GetCollectionsAsync(new GetCollectionsQuery()).Result;

                            foreach (var collection in collectionList)
                            {
                                Console.WriteLine(collection.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
