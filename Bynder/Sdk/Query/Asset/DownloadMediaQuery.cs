// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to specify the media from which we want the Url
    /// </summary>
    public class DownloadMediaQuery
    {
        /// <summary>
        /// Media id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Media item id. Can be null and Url returned will be the Url for the 
        /// original item
        /// </summary>
        public string MediaItemId { get; set; }
    }
}
