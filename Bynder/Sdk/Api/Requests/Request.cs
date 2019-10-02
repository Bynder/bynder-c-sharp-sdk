// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Net.Http;

namespace Bynder.Sdk.Api.Requests
{
    /// <summary>
    /// API Request representation.
    /// </summary>
    /// <typeparam name="T">Type to which the response will be deserialized</typeparam>
    internal abstract class Request<T>
    {
        /// <summary>
        /// Object with information about the API parameters to send.
        /// </summary>
        public object Query { get; set; }

        /// <summary>
        /// Path of the endpoint to call.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HttpMethod to use.
        /// </summary>
        public HttpMethod HTTPMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Request`1"/> is authenticated.
        /// </summary>
        /// <value><c>true</c> if authenticated; otherwise, <c>false</c>.</value>
        public bool Authenticated { get; set; } = true;

        /// <summary>
        /// True if we want to deserialize response to <see cref="T"/>. 
        /// However if <see cref="T"/> is a string and this property has value false, we might want to get the raw string response without
        /// deserializing, so in that case deserialization will not occur.
        /// </summary>
        public bool DeserializeResponse { get; set; } = true;
    }
}
