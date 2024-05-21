// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Model to represent metaproperty options
    /// </summary>
    public class MetapropertyOption
    {
        /// <summary>
        /// Id of the option
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the option
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Label of the option
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Order in which option should appear
        /// </summary>
        [JsonProperty("zindex")]
        public string ZIndex { get; set; }

        /// <summary>
        /// True if metaproperty option has selectable turned on
        /// </summary>
        [JsonProperty("isSelectable", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsSelectable { get; set; }

        /// <summary>
        /// Child metaproperty options
        /// </summary>
        [JsonProperty("options")]
        public List<MetapropertyOption> Options { get; set; }
        
        /// <summary>
        /// Id's of the linked metaproperty options
        /// </summary>
        [JsonProperty("linkedOptionIds")]
        public List<string> LinkedOptionIds { get; set; }

        /// <summary>
        /// Returns label translation by culture (e.g. 'en_US', 'de_DE' and etc)
        /// </summary>
        [JsonProperty("labels")]
        public IDictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
    }
}
