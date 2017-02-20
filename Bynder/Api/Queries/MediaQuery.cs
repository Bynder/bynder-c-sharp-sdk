// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Api.Converters;

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Query to filter media results
    /// </summary>
    public class MediaQuery
    {
        /// <summary>
        /// Brand id. Specify this property if you want only media for specific brand.
        /// </summary>
        [APIField("brandId")]
        public string BrandId { get; set; }

        /// <summary>
        /// SubBrand id. Specify this property if you want only media for specific subBrand.
        /// </summary>
        [APIField("subBrandId")]
        public string SubBrandId { get; set; }

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

        /// <summary>
        /// Keyword that the asset has to have to appear in the results
        /// </summary>
        [APIField("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// Metaproperty option ids that the asset has to have
        /// </summary>
        [APIField("propertyOptionId", Converter = typeof(ListConverter))]
        public IList<string> PropertyOptionId { get; set; } = new List<string>();
    }
}
