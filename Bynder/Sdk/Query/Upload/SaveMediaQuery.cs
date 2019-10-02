// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

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
    }
}
