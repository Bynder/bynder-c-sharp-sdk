// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    /// <summary>
    /// Query that has the information to save media.
    /// This should only be used to upload a file
    /// </summary>
    internal class SaveMediaQuery
    {
        /// <summary>
        /// Brand id we want to save media to
        /// </summary>
        [ApiField("brandid")]
        public string BrandId { get; set; }

        /// <summary>
        /// Name of the asset
        /// </summary>
        [ApiField("name")]
        public string Filename { get; set; }

        /// <summary>
        /// Import id
        /// </summary>
        public string ImportId { get; set; }

        /// <summary>
        /// Media id. If specified it will add the asset as new version
        /// of the specified media. Otherwise a new media will be added to 
        /// the asset bank
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Tags that will be added on the image
        /// </summary>
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
