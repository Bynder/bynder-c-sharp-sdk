// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Collection
{
    using System.Collections.Generic;

    using Bynder.Sdk.Api.Converters;

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

        /// <summary>
        /// <para>Desired order of returned collection set.</para>
        /// <para>See <see cref="Model.CollectionsOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }

        /// <summary>
        /// List of collection ids. Will return the collection for each existing collection.
        /// </summary>
        [ApiField("ids", Converter = typeof(ListConverter))]
        public IEnumerable<string> Ids { get; set; }

        /// <summary>
        /// Indicates whether or not the response should include count results. 
        /// </summary>
        [ApiField("count")]
        public bool? Count { get; set; }

        /// <summary>
        /// Search on matching names.
        /// </summary>
        [ApiField("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// Indicates whether or not the return should only contain collections marked as public.
        /// </summary>
        [ApiField("isPublic")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// Minimum collectionCount that the returned collections should have.
        /// </summary>
        [ApiField("minCount")]
        public int? MinCount { get; set; }
    }
}
