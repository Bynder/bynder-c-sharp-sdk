// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Query.Decoder;
using Bynder.Sdk.Settings;
using Moq;
using Xunit;

namespace Bynder.Test.Api.RequestSender
{
    public class ApiRequestSenderTest
    {
        [Fact]
        public async Task WhenRequestIsPostThenParametersAreAddedToContent()
        {
            var httpSenderMock = new Mock<IHttpRequestSender>();
            var query = new StubQuery
            {
                Item1 = "Value"
            };
            var accessToken = "access_token";

            using (ApiRequestSender apiRequestSender = new ApiRequestSender(
                new Configuration{
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(true, accessToken),
                httpSenderMock.Object
            ))
            {
                var apiRequest = new ApiRequest<bool>()
                {
                    Path = "/fake/api",
                    HTTPMethod = HttpMethod.Post,
                    Query = query
                };

                await apiRequestSender.SendRequestAsync(apiRequest);

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.PathAndQuery.Contains("/fake/api")
                        && req.Method == HttpMethod.Post
                        && req.Headers.Authorization.ToString() == $"Bearer {accessToken}"
                        && req.Content.ReadAsStringAsync().Result.Contains("Item1=Value")
                    )));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                    ), Times.Once);
            }
        }


        [Fact]
        public async Task WhenCredentialInvalidTwoRequestsSent()
        {
            var httpSenderMock = new Mock<IHttpRequestSender>();

            var query = new StubQuery
            {
                Item1 = "Value"
            };
            var accessToken = "access_token";

            using (ApiRequestSender apiRequestSender = new ApiRequestSender(
                new Configuration{
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(false, accessToken),
                httpSenderMock.Object
            ))
            {
                var apiRequest = new ApiRequest<bool>()
                {
                    Path = "/fake/api",
                    HTTPMethod = HttpMethod.Get,
                    Query = query
                };

                await apiRequestSender.SendRequestAsync(apiRequest);

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.PathAndQuery.Contains("/oauth2/token")
                        && req.Method == HttpMethod.Post
                    )));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.PathAndQuery.Contains("/fake/api")
                        && req.Method == HttpMethod.Get
                        && req.Headers.Authorization.ToString() == $"Bearer {accessToken}"
                        && req.RequestUri.Query.Contains("Item1=Value")
                    )));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                ), Times.Exactly(2));
            }
        }

        [Fact]
        public async Task WhenRequestIsGetThenParametersAreAddedToUrl()
        {
            var httpSenderMock = new Mock<IHttpRequestSender>();
            var query = new StubQuery
            {
                Item1 = "Value"
            };
            var accessToken = "access_token";

            using (ApiRequestSender apiRequestSender = new ApiRequestSender(
                new Configuration{
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(true, accessToken),
                httpSenderMock.Object
            ))
            {
                var apiRequest = new ApiRequest<bool>
                {
                    Path = "/fake/api",
                    HTTPMethod = HttpMethod.Get,
                    Query = query
                };

                await apiRequestSender.SendRequestAsync(apiRequest);

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.PathAndQuery.Contains("/fake/api")
                        && req.Method == HttpMethod.Get
                        && req.Headers.Authorization.ToString() == $"Bearer {accessToken}"
                        && req.RequestUri.Query.Contains("Item1=Value")
                    )));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                ), Times.Once);
            }
        }

        private ICredentials GetCredentials(bool valid = true, string accessToken = null)
        {
            var credentialsMock = new Mock<ICredentials>();
            credentialsMock
                .Setup(mock => mock.AreValid())
                .Returns(valid);

            credentialsMock
                .SetupGet(mock => mock.AccessToken)
                .Returns(accessToken);

            credentialsMock
                .SetupGet(mock => mock.TokenType)
                .Returns("Bearer");

            return credentialsMock.Object;
        }

        /// <summary>
        /// Stub query for testing purposes.
        /// </summary>
        private class StubQuery
        {
            /// <summary>
            /// Stub property
            /// </summary>
            [ApiField("Item1")]
            public string Item1 { get; set; }
        }
    }
}
