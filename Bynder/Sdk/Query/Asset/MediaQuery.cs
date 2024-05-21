// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Model;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to filter media results
    /// </summary>
    public class MediaQuery
    {
        /// <summary>
        /// Brand id. Specify this property if you want only media for specific brand.
        /// </summary>
        [ApiField("brandId")]
        public string BrandId { get; set; }

        /// <summary>
        /// SubBrand id. Specify this property if you want only media for specific subBrand.
        /// </summary>
        [ApiField("subBrandId")]
        public string SubBrandId { get; set; }

        /// <summary>
        /// Category id. Specify this property if you want only media for specific category.
        /// </summary>
        [ApiField("categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// Collection id. Specify this property if you want only media for specific collection.
        /// </summary>
        [ApiField("collectionId")]
        public string CollectionId { get; set; }

        /// <summary>
        /// List of asset ids. Will return an asset for each existing id.
        /// </summary>
        [ApiField("ids", Converter = typeof(ListConverter))]
        public IEnumerable<string> Ids { get; set; }

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
        /// Keyword that the asset has to have to appear in the results
        /// </summary>
        [ApiField("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// The type of the asset
        /// </summary>
        [ApiField("type")]
        public string Types { get; set; }

        /// <summary>
        /// The tags of the asset
        /// </summary>
        [ApiField("tags")]
        public string Tags { get; set; }

        /// <summary>
        /// The tags of the asset
        /// </summary>
        [ApiField("orientation")]
        public string Orientations { get; set; }

        /// <summary>
        /// Metaproperty option ids that the asset has to have
        /// </summary>
        [ApiField("propertyOptionId", Converter = typeof(ListConverter))]
        public IList<string> PropertyOptionId { get; set; } = new List<string>();

        /// <summary>
        /// Properties values by key
        /// Examples
        /// key: property_NAME value: Amsterdam
        /// or
        /// key: property_NAME value: 00000000-0000-0000-0000000000000000
        /// key property_SomethingElse value: 00000000-0000-0000-0000000000000001
        /// </summary>
        [ApiField("property_", Converter = typeof(MetapropertyOptionsConverter))]
        public IDictionary<string, IList<string>> PropertiesByKey { get; set; }
    }
}
