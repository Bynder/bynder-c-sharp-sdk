// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bynder.Models
{
    /// <summary>
    /// Model to represent the thumbnails of a media asset
    /// </summary>
    public class Thumbnails
    {
        /// <summary>
        /// Mini thumbnail Url
        /// </summary>
        [JsonProperty("mini")]
        public string Mini { get; set; }

        /// <summary>
        /// Thul thumbnail Url
        /// </summary>
        [JsonProperty("thul")]
        public string Thul { get; set; }

        /// <summary>
        /// Web-image Url
        /// </summary>
        [JsonProperty("webimage")]
        public string WebImage { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> All { get; set; }
    }
}
