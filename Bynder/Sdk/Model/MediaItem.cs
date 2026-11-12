// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Bynder.Sdk.Api.Converters;
using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Media Item specific information received from <see cref="RequestMediaInfoAsync"/> of IAssetBankManager
    /// </summary>
    public class MediaItem
    {
        /// <summary>
        /// Id of the item
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("fileName")]
        public string Name { get; set; }

        /// <summary>
        /// Date created
        /// </summary>
        [JsonProperty("dateCreated")]
        public string DateCreated { get; set; }

        /// <summary>
        /// File size of the item
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; set; }

        /// <summary>
        /// true if it is corresponds to the current version of the asset
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("active", DefaultValueHandling = DefaultValueHandling.Populate, ItemConverterType = typeof(BooleanJsonConverter))]
        public bool Active { get; set; }

        /// <summary>
        /// Media item type. (original, additional, web...)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
