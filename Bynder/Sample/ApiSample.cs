// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Upload;

namespace Bynder.Sample
{
    /// <summary>
    /// Class to show a little example on how to use the Bynder C# SDK. Before running this example is important that you fill
    /// API_BASE_URL, CONSUMER_KEY and CONSUMER_SECRET in App.config
    /// </summary>
    public class ApiSample
    {
        private IBynderClient _bynderClient;

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">arguments to main</param>
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the sample Class to run.");
                Console.WriteLine("Usage: dotnet run -- BrandsSample");
                return;
            }

            Console.WriteLine(args[0]);
            // Run samples related to brands
            if (args[0].Equals("BrandsSample")) {
                Console.WriteLine("Running samples for brands...");
                await BrandsSample.BrandsSampleAsync();
                return;
            }
            // Run samples related to metaproperties
            if (args[0].Equals("MetapropertiesSample")) {
                Console.WriteLine("Running samples for metaproperties...");
                await MetapropertiesSample.MetapropertiesSampleAsync();
                return;
            }
            // Run samples related to media
            if (args[0].Equals("MediaSample")) {
                Console.WriteLine("Running samples for media...");
                await MediaSample.MediaSampleAsync();
                return;
            }

            // Run samples related to modifying media
            if (args[0].Equals("ModifyMediaSample"))
            {
                Console.WriteLine("Running samples for the modification of media...");
                await ModifyMediaSample.ModifyMediaSampleAsync();
                return;
            }

            // Run samples related to collections
            if (args[0].Equals("CollectionsSample")) {
                Console.WriteLine("Running samples for collections...");
                await CollectionsSample.CollectionsSampleAsync();
                return;
            }
            // Run samples related to tags
            if (args[0].Equals("TagsSample")) {
                Console.WriteLine("Running samples for tags...");
                await TagsSample.TagsSampleAsync();
                return;
            }
            // Run samples related to upload
            if (args[0].Equals("UploadSample")) {
                Console.WriteLine("Running samples for upload...");
                await UploadSample.UploadSampleAsync();
                return;
            }
            // Run samples related to asset usage
            if (args[0].Equals("AssetUsageSample")) {
                Console.WriteLine("Running samples for asset usage...");
                await AssetUsageSample.AssetUsageSampleAsync();
                return;
            }
        }
    }
}
