// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Models;

namespace Bynder.Api.Impl
{
    /// <summary>
    /// Implementation of <see cref="IBynderApi"/>
    /// </summary>
    internal class BynderApi : IBynderApi
    {
        /// <summary>
        /// Instance to communicate with the Bynder API
        /// </summary>
        private IOauthRequestSender _requestSender;

        /// <summary>
        /// Credentials to use to login and to generate the AuthorizeUrl
        /// </summary>
        private Credentials _credentials;

        /// <summary>
        /// Base Url needed to generate AuthorizeUrl
        /// </summary>
        private string _baseUrl;

        /// <summary>
        /// Instance of the asset bank manager
        /// </summary>
        private IAssetBankManager _assetBankManager;

        /// <summary>
        /// Instance of the collections manager
        /// </summary>
        private ICollectionsManager _mediaCollectionsManager;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="credentials">Credentials to use to call the API</param>
        /// <param name="baseUrl">Base Url where we want to point API calls</param>
        /// <param name="requestSender">Instance to communicate with Bynder API</param>
        public BynderApi(Credentials credentials, string baseUrl, IOauthRequestSender requestSender)
        {
            _credentials = credentials;
            _baseUrl = baseUrl;
            _requestSender = requestSender;
        }

        /// <summary>
        /// Creates an instance of <see cref="IBynderApi"/> using settins parameter
        /// </summary>
        /// <param name="settings">settings to correctly configure <see cref="IBynderApi"/> instance</param>
        /// <returns>Bynder API instance to communicate with Bynder</returns>
        public static IBynderApi Create(Settings settings)
        {
            var credentials = new Credentials(
                settings.CONSUMER_KEY,
                settings.CONSUMER_SECRET,
                settings.TOKEN,
                settings.TOKEN_SECRET);

            return new BynderApi(credentials, settings.URL, new OauthRequestSender(credentials, settings.URL));
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        public async Task GetRequestTokenAsync()
        {
            var request = new Request<string>
            {
                Uri = "/api/v4/oauth/request_token",
                HTTPMethod = HttpMethod.Post,
                DeserializeResponse = false
            };

            var response = await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
            SetCredentialsFromResponse(response);
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        public async Task GetAccessTokenAsync()
        {
            var request = new Request<string>
            {
                Uri = "/api/v4/oauth/access_token",
                HTTPMethod = HttpMethod.Post,
                DeserializeResponse = false
            };

            var response = await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
            SetCredentialsFromResponse(response);
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        public IAssetBankManager GetAssetBankManager()
        {
            if (_assetBankManager == null)
            {
                _assetBankManager = new AssetBankManager(_requestSender);
            }

            return _assetBankManager;
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        public ICollectionsManager GetCollectionsManager()
        {
            if (_mediaCollectionsManager == null)
            {
                _mediaCollectionsManager = new CollectionsManager(_requestSender);
            }

            return _mediaCollectionsManager;
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <param name="callbackUrl">Check <see cref="IBynderApi"/> for more information</param>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        public string GetAuthorizeUrl(string callbackUrl)
        {
            UriBuilder builder = new UriBuilder(_baseUrl);
            NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add("oauth_token", _credentials.ACCESS_TOKEN);

            if (!string.IsNullOrEmpty(callbackUrl))
            {
                parameters.Add("callback", callbackUrl);
            }

            builder.Path += "api/v4/oauth/authorise/";
            builder.Query = parameters.ToString();
            return builder.ToString();
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        /// <param name="email">Check <see cref="IBynderApi"/> for more information</param>
        /// <param name="password">Check <see cref="IBynderApi"/> for more information</param>
        /// <returns>Check <see cref="IBynderApi"/> for more information</returns>
        [Obsolete("This method of login is deprecated. We should login through the browser")]
        public Task<User> LoginAsync(string email, string password)
        {
            return LoginAsync(new LoginQuery
            {
                Username = email,
                Password = password,
                ConsumerId = _credentials.CONSUMER_KEY
            });
        }

        /// <summary>
        /// Check <see cref="IBynderApi"/> for more information
        /// </summary>
        public void Logout()
        {
            _credentials.Reset();
        }

        /// <summary>
        /// Disposes the request sender
        /// </summary>
        public void Dispose()
        {
            _requestSender.Dispose();
        }

        /// <summary>
        /// Helper method to get string response and update the credentials
        /// </summary>
        /// <param name="response">response string</param>
        private void SetCredentialsFromResponse(string response)
        {
            string token = HttpUtility.ParseQueryString(response).Get("oauth_token");
            string tokenSecret = HttpUtility.ParseQueryString(response).Get("oauth_token_secret");
            if (token != null
                && tokenSecret != null)
            {
                _credentials.Set(token, tokenSecret);
            }
        }

        /// <summary>
        /// Returns Task containing user information. Sends the request to <see cref="_requestSender"/>
        /// </summary>
        /// <param name="query">information to call login API</param>
        /// <returns>Task with User information</returns>
        private async Task<User> LoginAsync(LoginQuery query)
        {
            var request = new Request<User>
            {
                Uri = "/api/v4/users/login/",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            var user = await _requestSender.SendRequestAsync(request);
            _credentials.Set(user.TokenKey, user.TokenSecret);

            return user;
        }
    }
}
