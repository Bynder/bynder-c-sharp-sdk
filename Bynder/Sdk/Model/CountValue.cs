// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// The count values as delivered by the API
    /// </summary>
    public class CountValue
    {
        /// <summary>
        /// The property option key
        /// </summary>
        public string PropertyOptionKey { get; set; }

        /// <summary>
        /// The count
        /// </summary>
        public int Count { get; set; }
    }
}
