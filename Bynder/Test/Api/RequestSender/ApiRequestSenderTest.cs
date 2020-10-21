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
        private const string AccessToken = "access_token";

        private Mock<IHttpRequestSender> httpSenderMock;
        private StubQuery query;

        public ApiRequestSenderTest()
        {
            httpSenderMock = new Mock<IHttpRequestSender>();
            httpSenderMock
                .Setup(sender => sender.SendHttpRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            query = new StubQuery
            {
                Item1 = "Value"
            };
        }

        [Fact]
        public async Task WhenRequestIsPostThenParametersAreAddedToContent()
        {
            using (var apiRequestSender = CreateApiRequestSender(true))
            {
                var apiRequest = new ApiRequest<bool>()
                {
                    Path = "/fake/api",
                    HTTPMethod = HttpMethod.Post,
                    Query = query
                };

                await apiRequestSender.SendRequestAsync(apiRequest);

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(req =>
                        req.RequestUri.PathAndQuery.Contains("/fake/api")
                        && req.Method == HttpMethod.Post
                        && req.Headers.Authorization.ToString() == $"Bearer {AccessToken}"
                        && req.Content.ReadAsStringAsync().Result.Contains("Item1=Value")
                    )
                ));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                ), Times.Once);
            }
        }


        [Fact]
        public async Task WhenCredentialInvalidTwoRequestsSent()
        {
            using (var apiRequestSender = CreateApiRequestSender(false))
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
                    )
                ));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.PathAndQuery.Contains("/fake/api")
                        && req.Method == HttpMethod.Get
                        && req.Headers.Authorization.ToString() == $"Bearer {AccessToken}"
                        && req.RequestUri.Query.Contains("Item1=Value")
                    )
                ));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                ), Times.Exactly(2));
            }
        }

        [Fact]
        public async Task WhenRequestIsGetThenParametersAreAddedToUrl()
        {
            using (var apiRequestSender = CreateApiRequestSender(true))
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
                        && req.Headers.Authorization.ToString() == $"Bearer {AccessToken}"
                        && req.RequestUri.Query.Contains("Item1=Value")
                    )
                ));

                httpSenderMock.Verify(sender => sender.SendHttpRequest(
                    It.IsAny<HttpRequestMessage>()
                ), Times.Once);
            }
        }

        private IApiRequestSender CreateApiRequestSender(bool hasValidCredentials)
        {
            return new ApiRequestSender(
                new Configuration
                {
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(hasValidCredentials),
                httpSenderMock.Object
            );
        }

        private ICredentials GetCredentials(bool valid)
        {
            var credentialsMock = new Mock<ICredentials>();

            credentialsMock
                .Setup(mock => mock.AreValid())
                .Returns(valid);

            credentialsMock
                .SetupGet(mock => mock.AccessToken)
                .Returns(AccessToken);

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
