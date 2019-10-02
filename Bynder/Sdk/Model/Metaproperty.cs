// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Model to represent a metaproperty
    /// </summary>
    public class Metaproperty
    {
        /// <summary>
        /// Id of metaproperty
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of metaproperty
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Label of metaproperty
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Child metaproperty options
        /// </summary>
        [JsonProperty("options")]
        public List<MetapropertyOption> Options { get; set; }

        /// <summary>
        /// Returns true if Multiselect is selected for the metaproperty
        /// </summary>
        [JsonProperty("isMultiSelect", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Returns true if Required is selected for the metaproperty
        /// </summary>
        [JsonProperty("isRequired", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Returns true if Filterable is selected for the metaproperty
        /// </summary>
        [JsonProperty("isFilterable", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsFilterable { get; set; }

        /// <summary>
        /// Returns true if Main filter is selected for the metaproperty
        /// </summary>
        [JsonProperty("isMainfilter", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsMainfilter { get; set; }

        /// <summary>
        /// Returns true if Editable is selected for the metaproperty
        /// </summary>
        [JsonProperty("isEditable", ItemConverterType = typeof(BooleanJsonConverter))]
        public bool IsEditable { get; set; }

        /// <summary>
        /// Order in which metaproperty should appear
        /// </summary>
        [JsonProperty("zindex")]
        public int ZIndex { get; set; }
    }
}
