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
        /// Sends the HTTP request and returns its response.
        /// </summary>
        /// <param name="httpRequest">HTTP request.</param>
        /// <returns>The HTTP request response.</returns>
        /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue
        /// such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        Task<HttpResponseMessage> SendHttpRequest(HttpRequestMessage httpRequest);
    }
}
