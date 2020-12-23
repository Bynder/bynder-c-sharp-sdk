// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;

namespace Bynder.Sdk.Api.Requests
{
    /// <summary>
    /// API Request representation.
    /// </summary>
    /// <typeparam name="T">Type to which the response will be deserialized</typeparam>
    internal abstract class Request<T>
    {
        protected bool _authenticated = true;

        /// <summary>
        /// Indicates whether the request needs to be authenticated.
        /// </summary>
        /// <value><c>true</c> if authenticated; otherwise, <c>false</c>.</value>
        public bool Authenticated { get { return _authenticated; } }

        /// <summary>
        /// HttpMethod to use.
        /// </summary>
        public HttpMethod HTTPMethod { get; set; }

        /// <summary>
        /// Path of the endpoint to call.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Optional: Key/value pairs to add to the request header.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Optional: Object with information about the API parameters to send.
        /// </summary>
        public object Query { get; set; }

        /// <summary>
        /// Optional: Binary content to put in the request body.
        /// </summary>
        public byte[] BinaryContent { get; set; }
    }
}
