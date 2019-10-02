// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Service.OAuth;
using Bynder.Sdk.Settings;
using Moq;
using Xunit;

namespace Bynder.Test.Service.OAuth
{
    public class OAuthServiceTest
    {
        [Fact]
        public void GetAuthorisationUrlReturnsCorrectUrl()
        {
            
            OAuthService oauth = new OAuthService(
                new Configuration {
                    BaseUrl = new Uri("https://example.bynder.com"),
                    ClientId = "clientId",
                    ClientSecret = "clientSecret",
                    RedirectUri = "https://redirect.bynder.com"
                }, null, null);

            var authorisationUrl = oauth.GetAuthorisationUrl("state example", "openid offline");

            Assert.Equal("https://example.bynder.com:443/v6/authentication/oauth2/auth?client_id=clientId&redirect_uri=https%3A%2F%2Fredirect.bynder.com&response_type=code&scope=openid%20offline&scope=openid%20offline&state=state%20example",
                authorisationUrl);
        }

        [Fact]
        public async Task GetAccessTokenCallsRequestAsyncAndUpdates()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = new Token();
            var credentials = new Mock<ICredentials>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<OAuthRequest<Token>>()))
                            .Returns(Task.FromResult(result));
            var oauthService = new OAuthService(
                new Configuration {
                    BaseUrl = new Uri("https://example.bynder.com"),
                    ClientId = "clientId",
                    ClientSecret = "clientSecret",
                    RedirectUri = "https://redirect.bynder.com"
                },
                credentials.Object,
                apiRequestSender.Object);
            
            await oauthService.GetAccessTokenAsync("code", "openid offline").ConfigureAwait(false);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<OAuthRequest<Token>>(
                    req => req.Path == "/v6/authentication/oauth2/token"
                    && req.HTTPMethod == HttpMethod.Post
                    && !req.Authenticated
                    && req.Query != null
                )));

            credentials.Verify(cred => cred.Update(
                It.Is<Token>(
                    token => token == result
                )));
        }
    }
}
