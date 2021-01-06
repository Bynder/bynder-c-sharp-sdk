// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    internal class FinalizeUploadQuery
    {
        [ApiField("chunksCount")]
        public int ChunksCount { get; set; }

        [ApiField("fileName")]
        public string Filename { get; set; }

        [ApiField("fileSize")]
        public long FileSize { get; set; }

        [ApiField("sha256")]
        public string SHA256 { get; set; }
    }
}
