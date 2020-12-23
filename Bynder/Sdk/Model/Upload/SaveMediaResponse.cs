// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Sdk.Model.Upload
{
    /// <summary>
    /// Represents the response body of file upload calls
    /// <see cref="IAssetService.UploadFileToNewAssetAsync"/> and
    /// <see cref="IAssetService.UploadFileToExistingAssetAsync"/>.
    /// </summary>
    public class SaveMediaResponse
    {
        [JsonProperty("accessRequestId")]
        public string AccessRequestId { get; set; }

        [JsonProperty("mediaid")]
        public string MediaId { get; set; }

        [JsonProperty("batchId")]
        public string BatchId { get; set; }

        [JsonProperty("success")]
        public bool IsSuccessful { get; set; }
    }
}
