// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bynder.Sdk.Settings
{
    /// <summary>
    /// Settings needed to configure <see cref="IBynderClient"/>
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Bynder domain Url we want to communicate with
        /// </summary>
        [JsonProperty("base_url")]
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// OAuth Client id
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth Client secret
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI. Optional: It is
        /// only needed if trying to login through OAuth
        /// </summary>
        /// <value>The redirect URI.</value>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// OAuth token
        /// </summary>
        public Token Token { get; set; }

        /// <summary>
        /// Create a <see cref="Configuration"/> using the given filepath
        /// </summary>
        /// <param name="filepath">JSON file path</param>
        /// <returns><see cref="Configuration"/> instance</returns>
        public static Configuration FromJson(string filepath)
        {
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filepath));
        }
    }
}
