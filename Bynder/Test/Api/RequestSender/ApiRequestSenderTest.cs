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
        private const string _accessToken = "some_access_token";
        private const string _authHeader = "Bearer " + _accessToken;
        private const string _path = "/fake/api";
        private const string _queryValue = "some_query_value";
        private const string _queryString = "Item1=" + _queryValue;

        private Mock<IHttpRequestSender> _httpSenderMock;
        private StubQuery _query;

        public ApiRequestSenderTest()
        {
            _httpSenderMock = new Mock<IHttpRequestSender>();
            _httpSenderMock
                .Setup(sender => sender.SendHttpRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));
            _query = new StubQuery
            {
                Item1 = _queryValue
            };
        }

        [Fact]
        public async Task WhenRequestIsPostThenParametersAreAddedToContent()
        {
            await SendRequest(hasValidCredentials: true, httpMethod: HttpMethod.Post);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(req =>
                    req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Post
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.Content.ReadAsStringAsync().Result.Contains(_queryString)
                )
            ));

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.IsAny<HttpRequestMessage>()
            ), Times.Once);
        }


        [Fact]
        public async Task WhenCredentialInvalidTwoRequestsSent()
        {
            await SendRequest(hasValidCredentials: false, httpMethod: HttpMethod.Get);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery == ApiRequestSender.TokenPath
                    && req.Method == HttpMethod.Post
                )
            ));

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Get
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.RequestUri.Query.Contains(_queryString)
                )
            ));

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.IsAny<HttpRequestMessage>()
            ), Times.Exactly(2));
        }

        [Fact]
        public async Task WhenRequestIsGetThenParametersAreAddedToUrl()
        {
            await SendRequest(hasValidCredentials: true, httpMethod: HttpMethod.Get);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Get
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.RequestUri.Query.Contains(_queryString)
                )
            ));

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.IsAny<HttpRequestMessage>()
            ), Times.Once);
        }

        private async Task SendRequest(bool hasValidCredentials, HttpMethod httpMethod)
        {
            await CreateApiRequestSender(hasValidCredentials).SendRequestAsync(
                new ApiRequest<bool>()
                {
                    Path = _path,
                    HTTPMethod = httpMethod,
                    Query = _query
                }
            );
        }

        private IApiRequestSender CreateApiRequestSender(bool hasValidCredentials)
        {
            return new ApiRequestSender(
                new Configuration
                {
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(hasValidCredentials),
                _httpSenderMock.Object
            );
        }

        private ICredentials GetCredentials(bool isValid)
        {
            var credentialsMock = new Mock<ICredentials>();

            credentialsMock
                .Setup(mock => mock.AreValid())
                .Returns(isValid);

            credentialsMock
                .SetupGet(mock => mock.AccessToken)
                .Returns(_accessToken);

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
