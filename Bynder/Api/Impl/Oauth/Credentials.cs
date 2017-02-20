// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Impl.Oauth
{
    /// <summary>
    /// Class to hold token credentials to call the API.
    /// </summary>
    internal class Credentials
    {
        /// <summary>
        /// Initializes new instance with specified values
        /// </summary>
        /// <param name="consumerKey">consumer key</param>
        /// <param name="consumerSecret">consumer secret</param>
        /// <param name="token">token. This can be null if we are going to log in into Bynder through the browser</param>
        /// <param name="tokenSecret">token secret. This can be null if we are going to log in into Bynder through the browser</param>
        public Credentials(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            CONSUMER_KEY = consumerKey;
            CONSUMER_SECRET = consumerSecret;
            ACCESS_TOKEN = token;
            ACCESS_TOKEN_SECRET = tokenSecret;

            INITIAL_TOKEN = token;
            INITIAL_SECRET = tokenSecret;
        }

        /// <summary>
        /// Gets the consumer key
        /// </summary>
        public string CONSUMER_KEY { get; private set; }

        /// <summary>
        /// Gets the consumer secret
        /// </summary>
        public string CONSUMER_SECRET { get; private set; }

        /// <summary>
        /// Gets the access token
        /// </summary>
        public string ACCESS_TOKEN { get; private set; }

        /// <summary>
        /// Gets the access token secret
        /// </summary>
        public string ACCESS_TOKEN_SECRET { get; private set; }

        /// <summary>
        /// Initial token. Used when we want to reset credentials
        /// </summary>
        private string INITIAL_TOKEN { get; set; }

        /// <summary>
        /// Initial token secret. Used when we want to reset credentials
        /// </summary>
        private string INITIAL_SECRET { get; set; }

        /// <summary>
        /// Resets access token/secret to the initial ones.
        /// </summary>
        public void Reset()
        {
            ACCESS_TOKEN = INITIAL_TOKEN;
            ACCESS_TOKEN_SECRET = INITIAL_SECRET;
        }

        /// <summary>
        /// Sets new access token/secret
        /// </summary>
        /// <param name="token">new access token</param>
        /// <param name="secret">new access secret</param>
        public void Set(string token, string secret)
        {
            ACCESS_TOKEN = token;
            ACCESS_TOKEN_SECRET = secret;
        }
    }
}
