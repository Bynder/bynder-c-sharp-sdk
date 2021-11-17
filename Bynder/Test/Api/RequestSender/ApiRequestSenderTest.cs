// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Query.Decoder;
using Bynder.Sdk.Service;
using Bynder.Sdk.Service.OAuth;
using Bynder.Sdk.Settings;
using Moq;
using Newtonsoft.Json;
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

        private Mock<IOAuthService> _oAuthServiceMock;
        private Mock<IBynderClient> _bynderClientMock;
        private Mock<IHttpRequestSender> _httpSenderMock;
        private StubQuery _query;
        private IList<string> _expectedResponseBody;

        public ApiRequestSenderTest()
        {
            _oAuthServiceMock = new Mock<IOAuthService>();
            _bynderClientMock = new Mock<IBynderClient>();
            _bynderClientMock
                .Setup(bynderClient => bynderClient.GetOAuthService())
                .Returns(_oAuthServiceMock.Object);
            _httpSenderMock = new Mock<IHttpRequestSender>();
            _query = new StubQuery
            {
                Item1 = _queryValue
            };
            _expectedResponseBody = new List<string> { "foo", "bar" };
        }

        [Fact]
        public async Task WhenErrorResponseThenThrowHttpRequestException()
        {
            var doRequest = SendRequestAsync<IList<string>>(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Post,
                response: CreateResponse(HttpStatusCode.InternalServerError)
            );
            await Assert.ThrowsAsync<HttpRequestException>(() => doRequest);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(req =>
                    req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Post
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.Content.ReadAsStringAsync().Result.Contains(_queryString)
                )
            ), Times.Once);
        }

        [Fact]
        public async Task WhenEmptyResponseThenReturnDefaultObject()
        {
            var responseBody = await SendRequestAsync<IList<string>>(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Post,
                response: CreateResponse(addContent: false)
            );

            Assert.Equal(default, responseBody);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(req =>
                    req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Post
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.Content.ReadAsStringAsync().Result.Contains(_queryString)
                )
            ), Times.Once);
        }

        [Fact]
        public async Task WhenRequestIsGetThenParametersAreAddedToUrl()
        {
            var responseBody = await SendRequestAsync<IList<string>>(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Get
            );

            Assert.Equal(_expectedResponseBody, responseBody);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Get
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.RequestUri.Query.Contains(_queryString)
                )
            ), Times.Once);
        }

        [Fact]
        public async Task WhenRequestIsPostThenParametersAreAddedToContent()
        {
            var responseBody = await SendRequestAsync<IList<string>>(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Post
            );

            Assert.Equal(_expectedResponseBody, responseBody);

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(req =>
                    req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Post
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.Content.ReadAsStringAsync().Result.Contains(_queryString)
                )
            ), Times.Once);
        }

        [Fact]
        public async Task WhenCredentialInvalidThenTokenIsRefreshed()
        {
            var responseBody = await SendRequestAsync<IList<string>>(
                hasValidCredentials: false,
                httpMethod: HttpMethod.Get
            );

            Assert.Equal(_expectedResponseBody, responseBody);

            _oAuthServiceMock.Verify(oAuthService => oAuthService.GetRefreshTokenAsync());

            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(req =>
                    req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Get
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.RequestUri.Query.Contains(_queryString)
                )
            ));
        }

        private async Task<T> SendRequestAsync<T>(
            bool hasValidCredentials,
            HttpMethod httpMethod,
            HttpResponseMessage response = null)
        {
            if (response == null)
            {
                response = CreateResponse();
            }

            var httpSenderMockSetup = _httpSenderMock
                .Setup(sender => sender.SendHttpRequest(It.IsAny<HttpRequestMessage>()));
            if (response.IsSuccessStatusCode)
            {
                httpSenderMockSetup.Returns(Task.FromResult(response));
            }
            else
            {
                httpSenderMockSetup.ThrowsAsync(new HttpRequestException(""));
            }

            return await CreateApiRequestSender(hasValidCredentials).SendRequestAsync(
                new ApiRequest<T>
                {
                    Path = _path,
                    HTTPMethod = httpMethod,
                    Query = _query
                }
            );
        }

        private HttpResponseMessage CreateResponse(
            HttpStatusCode statusCode = HttpStatusCode.OK,
            bool addContent = true)
        {
            var response = new HttpResponseMessage(statusCode);
            if (addContent)
            {
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(_expectedResponseBody),
                    Encoding.UTF8,
                    "application/json"
                );
            }
            return response;
        }

        private IApiRequestSender CreateApiRequestSender(bool hasValidCredentials)
        {
            return new ApiRequestSender(
                new Configuration
                {
                    BaseUrl = new Uri("https://example.bynder.com"),
                },
                GetCredentials(hasValidCredentials),
                _bynderClientMock.Object,
                _httpSenderMock.Object
            );
        }

        private Sdk.Settings.ICredentials GetCredentials(bool isValid)
        {
            var credentialsMock = new Mock<Sdk.Settings.ICredentials>();

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
