// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Bynder.Models
{
    /// <summary>
    /// Model returned when success login through API /login
    /// </summary>
    public class User
    {
        /// <summary>
        /// True if access was given to the username/password pair
        /// </summary>
        [JsonProperty("access")]
        public bool Access { get; set; }

        /// <summary>
        /// User id logged in
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Token key returned by API
        /// </summary>
        [JsonProperty("tokenKey")]
        public string TokenKey { get; set; }

        /// <summary>
        /// Token secret returned by API
        /// </summary>
        [JsonProperty("tokenSecret")]
        public string TokenSecret { get; set; }
    }
}
