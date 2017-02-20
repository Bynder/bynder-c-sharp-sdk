// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Models
{
    /// <summary>
    /// Model for the /upload/init/ response of the API. This model is only and should
    /// be only used when uploading a file.
    /// </summary>
    public class S3File
    {
        /// <summary>
        /// Upload id
        /// </summary>
        [JsonProperty("uploadid")]
        public string UploadId { get; set; }

        /// <summary>
        /// Target it
        /// </summary>
        [JsonProperty("targetid")]
        public string TargetId { get; set; }
    }
}
