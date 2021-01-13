using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Exceptions;
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
        public string GetAuthorisationUrl(string state)
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
                        { "scope", _configuration.Scopes },
                        { "response_type", "code" },
                        { "state", state },
                    }
                )
            }.ToString();
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        public async Task GetAccessTokenAsync()
        {
            _credentials.Update(await _requestSender.SendRequestAsync(
                new OAuthRequest<Token>
                {
                    Path = TokenPath,
                    HTTPMethod = HttpMethod.Post,
                    Query = new TokenQuery
                    {
                        ClientId = _configuration.ClientId,
                        ClientSecret = _configuration.ClientSecret,
                        GrantType = "client_credentials",
                        Scopes = _configuration.Scopes,
                    },
                }
            ).ConfigureAwait(false));
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        /// <param name="code">Check <see cref="IOAuthService"/>.</param>
        public async Task GetAccessTokenAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(code);
            }

            _credentials.Update(await _requestSender.SendRequestAsync(
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
                        Scopes = _configuration.Scopes,
                    },
                }
            ).ConfigureAwait(false));
        }

        /// <summary>
        /// Check <see cref="IOAuthService"/>.
        /// </summary>
        /// <returns>Check <see cref="IOAuthService"/>.</returns>
        public async Task GetRefreshTokenAsync()
        {
            if (_configuration.RedirectUri == null)
            {
                await GetAccessTokenAsync();
            }
            else
            {
                if (_credentials.RefreshToken == null)
                {
                    throw new MissingTokenException(
                        "Access token expired and refresh token is missing. " +
                        "Either pass a not expited access token through " +
                        "configuration or login through OAuth2"
                    );
                }
                _credentials.Update(await _requestSender.SendRequestAsync(
                    new OAuthRequest<Token>
                    {
                        Path = TokenPath,
                        HTTPMethod = HttpMethod.Post,
                        Query = new TokenQuery
                        {
                            ClientId = _configuration.ClientId,
                            ClientSecret = _configuration.ClientSecret,
                            RefreshToken = _credentials.RefreshToken,
                            GrantType = "refresh_token",
                        },
                    }
                ).ConfigureAwait(false));
            }
        }

    }
}
