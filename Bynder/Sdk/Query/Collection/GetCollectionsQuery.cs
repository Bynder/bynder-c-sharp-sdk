// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Collection
{
    /// <summary>
    /// Query to retrieve a list of collections
    /// </summary>
    public class GetCollectionsQuery
    {
        /// <summary>
        /// Limit of results per request. Max 1000. Default 50.
        /// </summary>
        [ApiField("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Page to be retrieved.
        /// </summary>
        [ApiField("page")]
        public int? Page { get; set; }
    }
}
