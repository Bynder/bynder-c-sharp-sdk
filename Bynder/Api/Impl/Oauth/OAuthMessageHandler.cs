// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Bynder.Api.Impl.Oauth
{
    /// <summary>
    /// Class to sign requests.
    /// Implementation from https://code.msdn.microsoft.com/Extending-HttpClient-with-8739f267/view/SourceCode
    /// </summary>
    internal class OAuthMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Credentials to sign request
        /// </summary>
        private Credentials _credentials;

        /// <summary>
        /// Oauth logic to generate the oauth header.
        /// </summary>
        private OAuthBase _oauthBase = new OAuthBase();

        /// <summary>
        /// Creates a new handler to sign requests using OAuth 1.0
        /// </summary>
        /// <param name="credentials">credentials to sign requests</param>
        /// <param name="innerHandler">next handler</param>
        public OAuthMessageHandler(Credentials credentials, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _credentials = credentials;
        }

        /// <summary>
        /// Adds Authorization header to the request
        /// </summary>
        /// <param name="request">HTTP request to sign</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with <see cref="HttpResponseMessage"/></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Compute OAuth header
            string normalizedUri;
            string normalizedParameters;
            string authHeader;

            string signature = _oauthBase.GenerateSignature(
                request.RequestUri,
                _credentials.CONSUMER_KEY,
                _credentials.CONSUMER_SECRET,
                _credentials.ACCESS_TOKEN,
                _credentials.ACCESS_TOKEN_SECRET,
                request.Method.Method,
                _oauthBase.GenerateTimeStamp(),
                _oauthBase.GenerateNonce(),
                out normalizedUri,
                out normalizedParameters,
                out authHeader);

            if (request.Method == HttpMethod.Post)
            {
                request.RequestUri = new Uri(request.RequestUri.OriginalString.Split('?')[0]);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
