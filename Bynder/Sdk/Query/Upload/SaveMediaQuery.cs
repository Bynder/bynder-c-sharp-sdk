// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    internal class SaveMediaQuery
    {
        [ApiField("brandId")]
        public string BrandId { get; set; }

        [ApiField("name")]
        public string Filename { get; set; }
    }
}
