// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Model;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to filter media results, including the option to see the total number of results
    /// </summary>
    public class MediaQueryFull : MediaQuery
    {
        [ApiField("total")]
        public bool Total { get; set; }
    }
}
