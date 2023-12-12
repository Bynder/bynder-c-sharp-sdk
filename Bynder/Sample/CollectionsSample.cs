// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Model;

namespace Bynder.Sample
{
    public class CollectionsSample
    {
        private IBynderClient _bynderClient;

        public static async Task CollectionsSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new CollectionsSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunCollectionsSampleAsync();
        }

        private CollectionsSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunCollectionsSampleAsync()
        {
            // Get collections with a limit of 10
            Console.WriteLine("Getting collections with a limit of 10: ");
            var collections = await _bynderClient.GetCollectionService().GetCollectionsAsync(new GetCollectionsQuery{
                Limit = 10
            });
            foreach(Collection collection in collections) {
                Console.WriteLine($"ID: {collection.Id}");
                Console.WriteLine($"Name: {collection.Name}");
                Console.WriteLine($"Media Count: {collection.MediaCount}");
            }

            // Get one collection by ID
            Console.WriteLine("Enter the collection ID to get the collection info for: ");
            var getCollectionId = Console.ReadLine();
            var collectionInfo = await _bynderClient.GetCollectionService().GetCollectionAsync(getCollectionId.Trim());
            Console.WriteLine($"ID: {collectionInfo.Id}");
            Console.WriteLine($"Name: {collectionInfo.Name}");
            Console.WriteLine($"Media Count: {collectionInfo.MediaCount}");

            // Create a collection
            Console.WriteLine("Enter the name for the collection to be created: ");
            var createCollectionName = Console.ReadLine();
            await _bynderClient.GetCollectionService().CreateCollectionAsync(new CreateCollectionQuery(createCollectionName.Trim()));
            collections = await _bynderClient.GetCollectionService().GetCollectionsAsync(new GetCollectionsQuery{
                Limit = 10
            });
            foreach(Collection collection in collections) {
                Console.WriteLine($"ID: {collection.Id}");
                Console.WriteLine($"Name: {collection.Name}");
                Console.WriteLine($"Media Count: {collection.MediaCount}");
            }

            // Share a collection
            Console.WriteLine("Enter the collection ID to share: ");
            var shareCollectionId = Console.ReadLine();
            Console.WriteLine("Enter the email recipient to share the collection to: ");
            var recipient = Console.ReadLine();
            List<string> recipients = new List<string>
            {
                recipient
            };
            var shareQuery = new ShareQuery(shareCollectionId.Trim(),recipients, SharingPermission.View){
                LoginRequired = false,
                Message = "test",
                SendMail = true
            };
            await _bynderClient.GetCollectionService().ShareCollectionAsync(shareQuery);

            // Delete a collection
            Console.WriteLine("Enter the ID for the collection to be deleted: ");
            var deleteCollectionId = Console.ReadLine();
            await _bynderClient.GetCollectionService().DeleteCollectionAsync(deleteCollectionId.Trim());
            
            // Add media to collection
            Console.WriteLine("Enter the collection ID to add a media to: ");
            var collectionIdAddMedia = Console.ReadLine();
            Console.WriteLine("Enter the media ID to add to the collection: ");
            var collectionIdAddMediaId = Console.ReadLine();

            List<string> mediaAdd = new List<string>
            {
                collectionIdAddMediaId.Trim()
            };
            await _bynderClient.GetCollectionService().AddMediaAsync(new AddMediaQuery(collectionIdAddMedia.Trim(), mediaAdd));

            // Get media IDs from collection ID
            Console.WriteLine("Enter the collection ID to get a medias from: ");
            var collectionIdGetMedia = Console.ReadLine();
            var collectionMedia = await _bynderClient.GetCollectionService().GetMediaAsync(new GetMediaQuery(collectionIdGetMedia.Trim()));
            foreach(string media in collectionMedia) {
                Console.WriteLine($"Collection Media: {media}");
            }

            // Remove media from collection
            Console.WriteLine("Enter the collection ID to remove a media from: ");
            var collectionIdRemoveMedia = Console.ReadLine();
            Console.WriteLine("Enter the media ID to remove from the collection: ");
            var collectionIdRemoveMediaId = Console.ReadLine();

            List<string> mediaRemove = new List<string>
            {
                collectionIdRemoveMediaId.Trim()
            };
            await _bynderClient.GetCollectionService().RemoveMediaAsync(new RemoveMediaQuery(collectionIdRemoveMedia.Trim(), mediaRemove));
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
