// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.IO;

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
        [JsonProperty("consumer_key")]
        public string CONSUMER_KEY { get; set; }

        /// <summary>
        /// Oauth Consumer secret
        /// </summary>
        [JsonProperty("consumer_secret")]
        public string CONSUMER_SECRET { get; set; }

        /// <summary>
        /// Oauth token
        /// </summary>
        [JsonProperty("token")]
        public string TOKEN { get; set; }

        /// <summary>
        /// Oauth secret
        /// </summary>
        [JsonProperty("token_secret")]
        public string TOKEN_SECRET { get; set; }

        /// <summary>
        /// Bynder domain Url we want to communicate with
        /// </summary>
        [JsonProperty("api_base_url")]
        public string URL { get; set; }

        /// <summary>
        /// Create a <see cref="Settings"/> using the given filepath
        /// </summary>
        /// <param name="filepath">JSON file path</param>
        /// <returns><see cref="Settings"/> instance</returns>
        public static Settings FromJson(string filepath)
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filepath));
        }
    }
}
