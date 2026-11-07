// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to retrieve metaproperty options by their ids
    /// </summary>
    public class MetapropertyOptionByIdsQuery
    {
        /// <summary>
        /// List of metaproperty option ids
        /// </summary>
        [ApiField("ids", Converter = typeof(ListConverter))]
        public IEnumerable<string> Ids { get; set; }
    }
}
