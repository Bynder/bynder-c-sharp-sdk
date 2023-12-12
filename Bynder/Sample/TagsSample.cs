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
            Console.WriteLine("Getting tags with a limit of 10: ");
            var tags = await _bynderClient.GetAssetService().GetTagsAsync(new GetTagsQuery{Limit = 10});
            foreach(Tag tag in tags){
                Console.WriteLine($"Tag Id: {tag.ID}");
                Console.WriteLine($"Tag Name: {tag.TagName}");
                Console.WriteLine($"Tag MediaCount: {tag.MediaCount}");
            }

            Console.WriteLine("Enter the media ID to add a tag to: ");
            var mediaIdAddTag = Console.ReadLine();
            Console.WriteLine("Enter the tag ID to add to the media: ");
            var tagIdAddToMedia = Console.ReadLine();
            List<string> mediasAddTag = new List<string>
            {
                mediaIdAddTag
            };
            await _bynderClient.GetAssetService().AddTagToMediaAsync(new AddTagToMediaQuery(tagIdAddToMedia, mediasAddTag));
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
