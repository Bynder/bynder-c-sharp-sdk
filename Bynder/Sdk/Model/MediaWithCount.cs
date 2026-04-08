// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Media items returned by API /media including the individual counts
    /// </summary>
    public class MediaWithCount
    {
        /// <summary>
        /// Media items
        /// </summary>
        [JsonProperty("media")]
        public IList<Media> Items { get; set; }

        /// <summary>
        /// Counts
        /// </summary>
        [JsonProperty("count")]
        public Counts Counts { get; set; }
    }
}
