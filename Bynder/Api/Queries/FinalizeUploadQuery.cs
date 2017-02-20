// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Finalize upload information. This model should only be used by UploadFile
    /// </summary>
    internal class FinalizeUploadQuery
    {
        /// <summary>
        /// Target id
        /// </summary>
        [APIField("targetid")]
        public string TargetId { get; set; }

        /// <summary>
        /// S3 filename
        /// </summary>
        [APIField("s3_filename")]
        public string S3Filename { get; set; }

        /// <summary>
        /// Number of chunks
        /// </summary>
        [APIField("chunks")]
        public string Chunks { get; set; }

        /// <summary>
        /// Upload id
        /// </summary>
        public string UploadId { get; set; }
    }
}
