// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Parameters needed to upload a part to Amazon. This model is only and should
    /// be only used when uploading a file.
    /// </summary>
    public class MultipartParameters
    {
        /// <summary>
        /// Amz credentials
        /// </summary>
        [JsonProperty("x-amz-credential")]
        public string AWSAccessKeyid { get; set; }

        /// <summary>
        /// Policy
        /// </summary>
        [JsonProperty("Policy")]
        public string Policy { get; set; }

        /// <summary>
        /// Success status
        /// </summary>
        [JsonProperty("success_action_status")]
        public string SuccessActionStatus { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Amz signature
        /// </summary>
        [JsonProperty("X-Amz-Signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Content type
        /// </summary>
        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Acl
        /// </summary>
        [JsonProperty("acl")]
        public string Acl { get; set; }

        /// <summary>
        /// Amz algorithm
        /// </summary>
        [JsonProperty("x-amz-algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Amz date
        /// </summary>
        [JsonProperty("x-amz-date")]
        public string Date { get; set; }
    }
}
