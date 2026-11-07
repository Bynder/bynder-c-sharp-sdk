// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Model;
using Newtonsoft.Json;
using Xunit;

namespace Bynder.Test.Model
{
    public class MediaWithTotalTest
    {
        [Fact]
        public void TotalCountAboveIntMaxValueIsRead()
        {
            var mediaWithTotal = JsonConvert.DeserializeObject<MediaWithTotal>("{\"media\":[{\"id\":\"mediaId\"}],\"total\":{\"count\":3000000000}}");

            Assert.Equal(3000000000L, mediaWithTotal.Total.Count);
            Assert.Equal("mediaId", Assert.Single(mediaWithTotal.Items).Id);
        }

        [Fact]
        public void TotalIsNullWhenNotReturned()
        {
            var mediaWithTotal = JsonConvert.DeserializeObject<MediaWithTotal>("{\"media\":[]}");

            Assert.Null(mediaWithTotal.Total);
            Assert.Empty(mediaWithTotal.Items);
        }
    }
}
