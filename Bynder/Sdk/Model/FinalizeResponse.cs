// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Model returned by /finalize for uploads
    /// </summary>
    public class FinalizeResponse
    {
        /// <summary>
        /// Import id for the upload. Needed to poll and save media.
        /// </summary>
        [JsonProperty("importId")]
        public string ImportId { get; set; }
    }
}
