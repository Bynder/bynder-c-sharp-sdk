using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query;
using Bynder.Sdk.Settings;

namespace Bynder.Sdk.Service.OAuth
{
    internal class OAuthService : IOAuthService
    {
        private readonly Configuration _configuration;
        private readonly ICredentials _credentials;
        /// <summary>
        /// Request sender to communicate with the Bynder API
        /// </summary>
        private readonly IApiRequestSender _requestSender;

        public const string AuthPath = "/v6/authentication/oauth2/auth";
        public const string TokenPath = "/v6/authentication/oauth2/token";

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="requestSender">instance to communicate with the Bynder API</param>
        public OAuthService(Configuration configuration, ICredentials credentials, IApiRequestSender requestSender)
        {
            _configuration = configuration;
            _credentials = credentials;
            _requestSender = requestSender;
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        /// <param name="state">Check <see cref="IOAuthService"/>.</param>
        /// <param name="scopes">Check <see cref="IOAuthService"/>.</param>
        public string GetAuthorisationUrl(string state, string scopes)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(state);
            }

            return new UriBuilder(_configuration.BaseUrl)
            {
                Path = AuthPath,
                Query = Utils.Url.ConvertToQuery(
                    new Dictionary<string, string>
                    {
                        { "client_id", _configuration.ClientId },
                        { "redirect_uri", _configuration.RedirectUri },
                        { "scope", scopes },
                        { "response_type", "code" },
                        { "state", state }
                    }
                )
            }.ToString();
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        /// <param name="code">Check <see cref="IOAuthService"/>.</param>
        /// <param name="scopes">Check <see cref="IOAuthService"/>.</param>
        public async Task GetAccessTokenAsync(string code, string scopes)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(code);
            }

            var token = await _requestSender.SendRequestAsync(
                new OAuthRequest<Token>
                {
                    Path = TokenPath,
                    HTTPMethod = HttpMethod.Post,
                    Query = new TokenQuery
                    {
                        ClientId = _configuration.ClientId,
                        ClientSecret = _configuration.ClientSecret,
                        RedirectUri = _configuration.RedirectUri,
                        GrantType = "authorization_code",
                        Code = code,
                        Scopes = scopes
                    },
                }
            ).ConfigureAwait(false);
            token.SetAccessTokenExpiration();
            _credentials.Update(token);
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        public async Task GetRefreshTokenAsync()
        {
            var token = await _requestSender.SendRequestAsync(
                new OAuthRequest<Token>
                {
                    Path = TokenPath,
                    HTTPMethod = HttpMethod.Post,
                    Query = new TokenQuery
                    {
                        ClientId = _configuration.ClientId,
                        ClientSecret = _configuration.ClientSecret,
                        RefreshToken = _credentials.RefreshToken,
                        GrantType = "refresh_token"
                    },
                }
            ).ConfigureAwait(false);
            _credentials.Update(token);
        }

    }
}
