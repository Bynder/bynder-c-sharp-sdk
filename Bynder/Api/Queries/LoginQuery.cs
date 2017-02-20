// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Object used internally to login
    /// </summary>
    internal class LoginQuery
    {
        /// <summary>
        /// Username
        /// </summary>
        [APIField("username")]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [APIField("password")]
        public string Password { get; set; }

        /// <summary>
        /// Consumer id that corresponds to consumer key.
        /// </summary>
        [APIField("consumerId")]
        public string ConsumerId { get; set; }
    }
}
