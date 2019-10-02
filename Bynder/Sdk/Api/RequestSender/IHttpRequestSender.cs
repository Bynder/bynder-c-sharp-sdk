// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bynder.Sdk.Api.RequestSender
{
    /// <summary>
    /// HTTP request sender. Used to send HTTP Requests.
    /// It eases unit testing of other components checking that correct requests are being sent.
    /// </summary>
    internal interface IHttpRequestSender : IDisposable
    {
        /// <summary>
        /// Sends the HTTP request and returns the content as string.
        /// </summary>
        /// <returns>The HTTP request response.</returns>
        /// <param name="httpRequest">HTTP request.</param>
        Task<string> SendHttpRequest(HttpRequestMessage httpRequest);
    }
}
