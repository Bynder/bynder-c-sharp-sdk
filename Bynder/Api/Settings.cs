// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api
{
    /// <summary>
    /// Settings needed to configure <see cref="IBynderApi"/>
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Oauth Consumer key
        /// </summary>
        public string CONSUMER_KEY { get; set; }

        /// <summary>
        /// Oauth Consumer secret
        /// </summary>
        public string CONSUMER_SECRET { get; set; }

        /// <summary>
        /// Oauth token 
        /// </summary>
        public string TOKEN { get; set; }

        /// <summary>
        /// Oauth secret 
        /// </summary>
        public string TOKEN_SECRET { get; set; }

        /// <summary>
        /// Bynder domain Url we want to communicate with
        /// </summary>
        public string URL { get; set; }
    }
}
