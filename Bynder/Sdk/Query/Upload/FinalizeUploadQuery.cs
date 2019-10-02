// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    /// <summary>
    /// Finalize upload information. This model should only be used by UploadFile
    /// </summary>
    internal class FinalizeUploadQuery
    {
        /// <summary>
        /// Target id
        /// </summary>
        [ApiField("targetid")]
        public string TargetId { get; set; }

        /// <summary>
        /// S3 filename
        /// </summary>
        [ApiField("s3_filename")]
        public string S3Filename { get; set; }

        /// <summary>
        /// Number of chunks
        /// </summary>
        [ApiField("chunks")]
        public string Chunks { get; set; }

        /// <summary>
        /// Upload id
        /// </summary>
        public string UploadId { get; set; }
    }
}
