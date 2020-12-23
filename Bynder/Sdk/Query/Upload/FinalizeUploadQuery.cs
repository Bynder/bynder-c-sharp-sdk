// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    internal class FinalizeUploadQuery
    {
        [ApiField("chunksCount")]
        internal int ChunksCount { get; set; }

        [ApiField("fileName")]
        internal string Filename { get; set; }

        [ApiField("fileSize")]
        internal long FileSize { get; set; }

        [ApiField("sha256")]
        internal string SHA256 { get; set; }
    }
}
