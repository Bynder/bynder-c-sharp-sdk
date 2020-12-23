// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Sdk.Model.Upload
{
    internal class PrepareUploadResponse
    {
        [JsonProperty("file_id")]
        internal string FileId { get; set; }
    }
}
