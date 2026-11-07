// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to filter media results including the individual counts
    /// </summary>
    public class MediaWithCountQuery : MediaQuery
    {
        /// <summary>
        /// Indicating whether or not the response should include the total count of results.
        /// </summary>
        [ApiField("total")]
        public bool? IncludeTotal { get; set; }

        /// <summary>
        /// Indicating whether or not the response should include the individual count of results.
        /// </summary>
        [ApiField("count")]
        public bool? IncludeCount { get; set; }

        /// <summary>
        /// Indicating whether or not the response should include media items.
        /// </summary>
        [ApiField("includeMediaItems")]
        public bool? IncludeMediaItems { get; set; }

        /// <summary>
        /// Indicating whether or not the response should include the version number.
        /// </summary>
        [ApiField("includeVersionNumber")]
        public bool? IncludeVersionNumber { get; set; }

        /// <summary>
        /// <para>Desired order of returned assets.</para>
        /// <para>See <see cref="Model.AssetOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }
    }
}
