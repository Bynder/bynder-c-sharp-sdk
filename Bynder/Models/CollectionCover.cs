// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bynder.Models
{
    /// <summary>
    /// Model describing the cover of a collection
    /// </summary>
    public class CollectionCover
    {
        /// <summary>
        /// Thumbnail Url
        /// </summary>
        [JsonProperty("thumbnail")]
        public Uri Thumbnail { get; set; }

        /// <summary>
        /// Thumbnail Urls
        /// </summary>
        [JsonProperty("thumbnails")]
        public IEnumerable<Uri> Thumbnails { get; set; }

        /// <summary>
        /// Url to the large version of the cover
        /// </summary>
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }
}
