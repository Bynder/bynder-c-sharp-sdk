// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Query.Decoder;
using Bynder.Sdk.Service;
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
        private readonly IBynderClient _bynderClient;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private IHttpRequestSender _httpSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sdk.Api.ApiRequestSender"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="credentials">Credentials to use in authorized requests and to refresh tokens</param>
        /// <param name="oauthService">OAuthService.</param>
        /// <param name="httpSender">HTTP instance to send API requests</param>
        internal ApiRequestSender(Configuration configuration, ICredentials credentials, IBynderClient bynderClient, IHttpRequestSender httpSender)
        {
            _configuration = configuration;
            _credentials = credentials;
            _bynderClient = bynderClient;
            _httpSender = httpSender;
        }

        /// <summary>
        /// Create an instance of <see cref="IApiRequestSender"/> given the specified configuration and credentials.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="configuration">Configuration.</param>
        /// <param name="credentials">Credentials.</param>
        /// <param name="oauthService">OAuthService.</param>
        public static IApiRequestSender Create(Configuration configuration, ICredentials credentials, IBynderClient bynderClient)
        {
            return new ApiRequestSender(configuration, credentials, bynderClient, new HttpRequestSender());
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
        /// <param name="request">Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</param>
        /// <typeparam name="T">Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</typeparam>
        /// <returns>Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</returns>
        /// <exception cref="T:System.Net.Http.HttpRequestException">Check <see cref="t:Sdk.Api.IApiRequestSender"/>.</exception>
        public async Task<T> SendRequestAsync<T>(Request<T> request)
        {
            var response = await CreateHttpRequestAsync(request).ConfigureAwait(false);

            var responseContent = response.Content;
            if (response.Content == null)
            {
                return default;
            }

            var responseString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
            if (responseString == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private async Task<HttpResponseMessage> CreateHttpRequestAsync<T>(Request<T> request)
        {
            var httpRequestMessage = CreateRequest(request);

            if (request.Authenticated)
            {
                if (!_credentials.AreValid())
                {
                    // Get a refesh token when the credentials are no longer valid
                    await _semaphore.WaitAsync().ConfigureAwait(false);
                    try
                    {
                        await _bynderClient.GetOAuthService().GetRefreshTokenAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    _credentials.TokenType,
                    _credentials.AccessToken
                );
            }

            return await _httpSender.SendHttpRequest(httpRequestMessage).ConfigureAwait(false);
        }

        private HttpRequestMessage CreateRequest<T>(Request<T> request)
        {
            var uriBuilder = new UriBuilder(_configuration.BaseUrl.ToString())
            {
                Path = request.Path
            };
            if (request.HTTPMethod == HttpMethod.Get)
            {
                uriBuilder.Query = Utils.Url.ConvertToQuery(_queryDecoder.GetParameters(request.Query));
            }
            HttpRequestMessage httpRequest = new HttpRequestMessage(request.HTTPMethod, uriBuilder.ToString());

            if (request.Headers != null)
            {
                foreach (var kv in request.Headers)
                {
                    httpRequest.Headers.Add(kv.Key, kv.Value);
                }
            }

            if (request.HTTPMethod == HttpMethod.Post)
            {
                httpRequest.Content = request.BinaryContent != null
                    ? new ByteArrayContent(request.BinaryContent)
                    : new FormUrlEncodedContent(_queryDecoder.GetParameters(request.Query));
            }

            return httpRequest;
        }

    }
}
