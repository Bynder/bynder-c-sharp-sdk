// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to search the options of a metaproperty
    /// </summary>
    public class MetapropertyOptionSearchQuery
    {
        /// <summary>
        /// Id of the metaproperty
        /// </summary>
        public string MetapropertyId { get; set; }

        /// <summary>
        /// Name of the option
        /// </summary>
        [ApiField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Maximum number of results
        /// </summary>
        [ApiField("limit")]
        public int Limit { get; set; } = 50;

        /// <summary>
        /// Offset page for results: return the N-th set of limit-results
        /// </summary>
        [ApiField("page")]
        public int Page { get; set; } = 1;
    }
}
