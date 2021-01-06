// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    internal class SaveMediaQuery
    {
        [ApiField("brandId")]
        public string BrandId { get; set; }

        [ApiField("name")]
        public string Filename { get; set; }

        [ApiField("tags", Converter = typeof(ListConverter))]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Metaproperty options to set on the asset.
        /// </summary>
        [ApiField("metaproperty", Converter = typeof(MetapropertyOptionsConverter))]
        public IDictionary<string, IList<string>> MetapropertyOptions { get; set; } = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Add a set of options to a metaproperty
        /// </summary>
        /// <param name="metapropertyId">metaproperty ID</param>
        /// <param name="optionIds">set of options</param>
        public void AddMetapropertyOptions(string metapropertyId, IList<string> optionIds)
        {
            MetapropertyOptions.Add(metapropertyId, optionIds);
        }

    }
}
