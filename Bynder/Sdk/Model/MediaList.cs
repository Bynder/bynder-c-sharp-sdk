// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Media list model returned by API /media
    /// </summary>
    public class MediaList
    {
        /// <summary>
        /// Media items
        /// </summary>
        [JsonProperty("media")]
        public IList<Media> Items { get; set; }

        /// <summary>
        /// Total of results
        /// </summary>
        [JsonProperty("total")]
        public TotalCountResponse Total { get; set; }
    }
}
