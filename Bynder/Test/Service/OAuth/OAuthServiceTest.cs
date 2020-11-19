// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query;
using Bynder.Sdk.Service.OAuth;
using Bynder.Sdk.Settings;
using Moq;
using Xunit;

namespace Bynder.Test.Service.OAuth
{
    public class OAuthServiceTest
    {
        private const string _clientId = "clientId";
        private const string _clientSecret = "clientSecret";
        private const string _baseUrl = "https://example.bynder.com";
        private const string _redirectUrl = "https://redirect.bynder.com";
        private const string _redirectUrlEncoded = "https%3A%2F%2Fredirect.bynder.com";
        private const string _refreshToken = "refreshToken";
        private const string _state = "state";
        private const string _scope = "offline";
        private const string _code = "code";

        private Token _token;
        private Mock<ICredentials> _credentialsMock;
        private Mock<IApiRequestSender> _apiRequestSenderMock;
        private OAuthService _oauthService;

        public OAuthServiceTest() {
            _token = new Token();

            _credentialsMock = new Mock<ICredentials>();
            _credentialsMock
                .SetupGet(credentials => credentials.RefreshToken)
                .Returns(_refreshToken);

            _apiRequestSenderMock = new Mock<IApiRequestSender>();
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<OAuthRequest<Token>>()))
                .Returns(Task.FromResult(_token));

            _oauthService = new OAuthService(
                new Configuration
                {
                    BaseUrl = new Uri(_baseUrl),
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    RedirectUri = _redirectUrl,
                },
                _credentialsMock.Object,
                _apiRequestSenderMock.Object
            );

        }

        [Fact]
        public void GetAuthorisationUrlReturnsCorrectUrl()
        {
            var authorisationUrl = _oauthService.GetAuthorisationUrl(_state, _scope);

            Assert.Equal(
                $"{_baseUrl}:443{OAuthService.AuthPath}?client_id={_clientId}&redirect_uri={_redirectUrlEncoded}&response_type={_code}&scope={_scope}&state={_state}",
                authorisationUrl
            );
        }

        [Fact]
        public async Task GetAccessTokenCallsRequestAsyncAndUpdates()
        {
            await _oauthService.GetAccessTokenAsync(_code, _scope).ConfigureAwait(false);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<OAuthRequest<Token>>(
                    req => req.Path == OAuthService.TokenPath
                    && req.HTTPMethod == HttpMethod.Post
                    && !req.Authenticated
                    && (req.Query as TokenQuery).ClientId == _clientId
                    && (req.Query as TokenQuery).ClientSecret == _clientSecret
                    && (req.Query as TokenQuery).RedirectUri == _redirectUrl
                    && (req.Query as TokenQuery).GrantType == "authorization_code"
                    && (req.Query as TokenQuery).Code == _code
                    && (req.Query as TokenQuery).Scopes == _scope
                )
             ), Times.Once);

            _credentialsMock.Verify(cred => cred.Update(
                It.Is<Token>(
                    token => token == _token
                )
            ), Times.Once);
        }

        [Fact]
        public async Task GetRefreshTokenCallsRequestAsyncAndUpdates()
        {
            await _oauthService.GetRefreshTokenAsync().ConfigureAwait(false);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<OAuthRequest<Token>>(
                    req => req.Path == OAuthService.TokenPath
                    && req.HTTPMethod == HttpMethod.Post
                    && !req.Authenticated
                    && (req.Query as TokenQuery).ClientId == _clientId
                    && (req.Query as TokenQuery).ClientSecret == _clientSecret
                    && (req.Query as TokenQuery).RefreshToken == _refreshToken
                    && (req.Query as TokenQuery).GrantType == "refresh_token"
                )
            ), Times.Once);

            _credentialsMock.Verify(cred => cred.Update(
                It.Is<Token>(
                    token => token == _token
                )
            ), Times.Once);
        }
    }
}
