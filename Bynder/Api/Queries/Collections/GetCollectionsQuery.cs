// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Queries.Collections
{
    /// <summary>
    /// Query to retrieve a list of collections
    /// </summary>
    public class GetCollectionsQuery
    {
        /// <summary>
        /// Limit of results per request. Max 1000. Default 50.
        /// </summary>
        [APIField("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Page to be retrieved.
        /// </summary>
        [APIField("page")]
        public int? Page { get; set; }
    }
}
