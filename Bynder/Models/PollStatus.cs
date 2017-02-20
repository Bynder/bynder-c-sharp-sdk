// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bynder.Models
{
    /// <summary>
    /// Model for the/poll response of the API. This model is only and should
    /// be only used when uploading a file.
    /// </summary>
    public class PollStatus
    {
        /// <summary>
        /// Returns the items for which the conversion failed
        /// </summary>
        [JsonProperty("itemsFailed")]
        public HashSet<string> ItemsFailed { get; set; }

        /// <summary>
        /// Returns the items for which the conversion succeeded.
        /// </summary>
        [JsonProperty("itemsDone")]
        public HashSet<string> ItemsDone { get; set; }
    }
}
