// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Net.Http;

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Class that contains all information to do an API request to Bynder
    /// through <see cref="Impl.Oauth.IOauthRequestSender"/>
    /// </summary>
    /// <typeparam name="T">type we want the response to be deserialized to</typeparam>
    internal class Request<T>
    {
        /// <summary>
        /// Object with information about the API parameters to send.
        /// </summary>
        public object Query { get; set; }

        /// <summary>
        /// Uri of the endpoint to call
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// HttpMethod to use
        /// </summary>
        public HttpMethod HTTPMethod { get; set; }

        /// <summary>
        /// True if we want to deserialize response to <see cref="T"/>. 
        /// However if <see cref="T"/> is a string and this property has value false, we might want to get the raw string response without
        /// deserializing, so in that case deserialization will not occur.
        /// </summary>
        public bool DeserializeResponse { get; set; } = true;
    }
}
