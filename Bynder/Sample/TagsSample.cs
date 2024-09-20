// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service;
using Bynder.Sample.Utils;
using Bynder.Sdk.Settings;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Model;
using Bynder.Sdk.Service.Asset;

namespace Bynder.Sample
{
    public class TagsSample
    {
        private IBynderClient _bynderClient;

        public static async Task TagsSampleAsync()
        {
            var configuration = Configuration.FromJson("Config.json");
            var apiSample = new TagsSample(configuration);
            await apiSample.AuthenticateWithOAuth2Async(
                useClientCredentials: configuration.RedirectUri == null
            );
            await apiSample.RunTagsSampleAsync();
        }

        private TagsSample(Configuration configuration) {
            _bynderClient = ClientFactory.Create(configuration);
        }

        private async Task RunTagsSampleAsync()
        {
            var assetService = _bynderClient.GetAssetService();
            Console.WriteLine("Getting tags with a limit of 10: ");
            var tags = await assetService.GetTagsAsync(new GetTagsQuery{Limit = 10});
            foreach(Tag tag in tags){
                Console.WriteLine($"Tag Id: {tag.ID}");
                Console.WriteLine($"Tag Name: {tag.TagName}");
                Console.WriteLine($"Tag MediaCount: {tag.MediaCount}");
            }

            Console.WriteLine("Enter the media ID to add a tag to: ");
            var mediaId = Console.ReadLine();
            Console.WriteLine("Enter the tag ID to add to the media: ");
            var tagId = Console.ReadLine();
            await assetService.AddTagToMediaAsync(new AddTagToMediaQuery(tagId, [ mediaId ]));

            Console.WriteLine("Hit enter to view the asset (it may take a few seconds before the tag is registered)");
            Console.ReadKey();
            var asset = await assetService.GetMediaInfoAsync(new MediaInformationQuery() { MediaId = mediaId });
            ShowTags(asset);

            Console.WriteLine("Enter a new tag to add to the same media: ");
            var anotherTag = Console.ReadLine();
            if (asset.Tags == null)
            {
                asset.Tags = [anotherTag];
            }
            else
            {
                asset.Tags.Add(anotherTag);
            }

            await assetService.ModifyMediaAsync(new ModifyMediaQuery(mediaId) { Tags = asset.Tags } );

            Console.WriteLine("Hit enter to view the asset (it may take a few seconds before the tag is registered)");
            Console.ReadKey();
            asset = await assetService.GetMediaInfoAsync(new MediaInformationQuery() { MediaId = mediaId });
            ShowTags(asset);

            Console.WriteLine("Hit enter to remove the tags again");
            Console.ReadKey();

            foreach (var tag in asset.Tags)
            {
                var matchingTags = await assetService.GetTagsAsync(new GetTagsQuery() { Keyword = tag });
                if (matchingTags.Any())
                {
                    var tagToRemove = matchingTags.FirstOrDefault(t => t.TagName.Equals(tag, StringComparison.InvariantCultureIgnoreCase));
                    Console.WriteLine($"Removing tag {tagToRemove.TagName} with id {tagToRemove.ID}");
                    await assetService.RemoveTagFromMediaAsync(tagToRemove.ID, [mediaId]);
                }
                else
                {
                    Console.WriteLine($"Error: after adding tag with name '{tag}' to asset {mediaId}, tag cannot be found in Bynder");
                }
            }

            Console.WriteLine("Hit enter to view the asset (it may take a few seconds before the tags have been removed)");
            Console.ReadKey();

            asset = await assetService.GetMediaInfoAsync(new MediaInformationQuery() { MediaId = mediaId });
            ShowTags(asset);

        }

        private async void ShowTags(Media asset)
        {
            if (asset.Tags?.Any() ?? false)
            {
                Console.WriteLine($"Media with name {asset.Name} now has the following tags: {string.Join(',', asset.Tags)}");
            }
            else
            {
                Console.WriteLine($"Media with name {asset.Name} has no tags");
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
