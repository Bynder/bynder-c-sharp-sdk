// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query;
using Bynder.Sdk.Query.Decoder;
using Bynder.Sdk.Settings;
using Newtonsoft.Json;

namespace Bynder.Sdk.Api.RequestSender
{
    /// <summary>
    /// Implementation of <see cref="IApiRequestSender"/> interface.
    /// </summary>
    internal class ApiRequestSender : IApiRequestSender
    {
        private readonly Configuration _configuration;
        private readonly QueryDecoder _queryDecoder = new QueryDecoder();
        private readonly ICredentials _credentials;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private IHttpRequestSender _httpSender;

        public const string TokenPath = "/v6/authentication/oauth2/token";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sdk.Api.ApiRequestSender"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="credentials">Credentials to use in authorized requests and to refresh tokens</param>
        /// <param name="httpSender">HTTP instance to send API requests</param>
        internal ApiRequestSender(Configuration configuration, ICredentials credentials, IHttpRequestSender httpSender)
        {
            _configuration = configuration;
            _credentials = credentials;
            _httpSender = httpSender;
        }

        /// <summary>
        /// Create an instance of <see cref="IApiRequestSender"/> given the specified configuration and credentials.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="configuration">Configuration.</param>
        /// <param name="credentials">Credentials.</param>
        public static IApiRequestSender Create(Configuration configuration, ICredentials credentials)
        {
            return new ApiRequestSender(configuration, credentials, new HttpRequestSender());
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:Sdk.Api.ApiRequestSender"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:Sdk.Api.ApiRequestSender"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:Sdk.Api.ApiRequestSender"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Sdk.Api.ApiRequestSender"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:Sdk.Api.ApiRequestSender"/> was occupying.</remarks>
        public void Dispose()
        {
            _httpSender.Dispose();
        }

        /// <summary>
        /// Check <see cref="t:Sdk.Api.IApiRequestSender"/>.
        /// </summary>
        /// <returns>Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</returns>
        /// <param name="request">Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</param>
        /// <typeparam name="T">Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</typeparam>
        public async Task<T> SendRequestAsync<T>(Request<T> request)
        {
            var response = await CreateHttpRequestAsync(request).ConfigureAwait(false);

            var responseContent = response.Content;
            if (response.Content == null)
            {
                return default(T);
            }

            var responseString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
            if (responseString == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private async Task<HttpResponseMessage> CreateHttpRequestAsync<T>(Request<T> request)
        {
            var parameters = _queryDecoder.GetParameters(request.Query);
            var httpRequestMessage = HttpRequestMessageFactory.Create(
                _configuration.BaseUrl.ToString(),
                request.HTTPMethod,
                parameters,
                request.Path
            );

            if (request.Authenticated)
            {
                if (!_credentials.AreValid())
                {
                    _credentials.Update(await RefreshToken().ConfigureAwait(false));
                }

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    _credentials.TokenType,
                    _credentials.AccessToken
                );
            }

            return await _httpSender.SendHttpRequest(httpRequestMessage).ConfigureAwait(false);
        }

        private async Task<Token> RefreshToken()
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                return await SendRequestAsync(
                    new OAuthRequest<Token>
                    {
                        Authenticated = false,
                        Query = new TokenQuery
                        {
                            ClientId = _configuration.ClientId,
                            ClientSecret = _configuration.ClientSecret,
                            RefreshToken = _credentials.RefreshToken,
                            GrantType = "refresh_token"
                        },
                        Path = TokenPath,
                        HTTPMethod = HttpMethod.Post
                    }
                ).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static class HttpRequestMessageFactory
        {
            internal static HttpRequestMessage Create(
                string baseUrl, HttpMethod method, IDictionary<string, string> requestParams, string urlPath)
            {
                var builder = new UriBuilder(baseUrl);
                builder.Path = urlPath;

                if (HttpMethod.Get == method)
                {
                    builder.Query = Utils.Url.ConvertToQuery(requestParams);
                }

                HttpRequestMessage requestMessage = new HttpRequestMessage(method, builder.ToString());
                if (HttpMethod.Post == method)
                {
                    requestMessage.Content = new FormUrlEncodedContent(requestParams);
                }

                return requestMessage;
            }
        }
    }
}
