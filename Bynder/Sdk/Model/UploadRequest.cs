// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Model to get the information to start an upload /upload/init. This model is only and should
    /// be only used when uploading a file.
    /// </summary>
    public class UploadRequest
    {
        /// <summary>
        /// S3 file name
        /// </summary>
        [JsonProperty("s3_filename")]
        public string S3Filename { get; set; }

        /// <summary>
        /// S3 file information. <see cref="S3File"/>
        /// </summary>
        [JsonProperty("s3file")]
        public S3File S3File { get; set; }

        /// <summary>
        /// Amazon parameters information <see cref="MultipartParameters"/>
        /// </summary>
        [JsonProperty("multipart_params")]
        public MultipartParameters MultipartParams { get; set; }
    }
}
