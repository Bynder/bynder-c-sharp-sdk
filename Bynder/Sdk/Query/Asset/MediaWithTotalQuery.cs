// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to filter media results including the total count
    /// </summary>
    public class MediaWithTotalQuery : MediaQuery
    {
        /// <summary>
        /// Indicating whether or not the response should include the total count of results.
        /// </summary>
        [ApiField("total")]
        public bool? IncludeTotal { get; set; }

        /// <summary>
        /// <para>Desired order of returned assets.</para>
        /// <para>See <see cref="Model.AssetOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }
    }
}
