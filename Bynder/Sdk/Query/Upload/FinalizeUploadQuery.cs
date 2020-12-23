// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    /// <summary>
    /// Finalize upload information. To be used for the finalise_api request
    /// in <see cref="FileUploader"/>.
    /// </summary>
    internal class FinalizeUploadQuery
    {
        /// <summary>
        /// The amount of chunks that were uploaded.
        /// </summary>
        [ApiField("chunksCount")]
        public int ChunksCount { get; set; }

        /// <summary>
        /// The filename the file will get on the server.
        /// </summary>
        [ApiField("fileName")]
        public string Filename { get; set; }

        /// <summary>
        /// The size of the uploaded file.
        /// </summary>
        [ApiField("fileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// SHA-256 hash of the uploaded file.
        /// </summary>
        [ApiField("sha256")]
        public string SHA256 { get; set; }
    }
}
