// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Query with the information to init an upload. This
    /// class should only be used to UploadFile
    /// </summary>
    internal class RequestUploadQuery
    {
        /// <summary>
        /// Filename of the file we want to initialize the upload
        /// </summary>
        [APIField("filename")]
        public string Filename { get; set; }
    }
}
