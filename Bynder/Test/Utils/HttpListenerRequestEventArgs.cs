// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Bynder.Test.Utils
{
    /// <summary>
    /// Event arguments that have request information done to the HttpListener
    /// </summary>
    public class HttpListenerRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="request">HttpListenerRequest instance</param>
        public HttpListenerRequestEventArgs(HttpListenerRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Gets the Request information done to the HttpListener
        /// </summary>
        public HttpListenerRequest Request { get; private set; }
    }
}
