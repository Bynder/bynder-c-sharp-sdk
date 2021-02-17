// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Bynder.Sdk.Query.Upload
{
    /// <summary>
    /// Query with the information to upload a file
    /// </summary>
    public class UploadQuery
    {
        /// <summary>
        /// File path of the file we want to update.
        /// </summary>
        public string Filepath { get; set; }

        /// <summary>
        /// Brand id where we want to store the file
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// Media id. If specified it will add the asset as new version
        /// of the specified media. Otherwise a new media will be added to 
        /// the asset bank
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Tags of the file that we want to update
        /// </summary>
        public IList<string> Tags { get; set; }
    }
}
