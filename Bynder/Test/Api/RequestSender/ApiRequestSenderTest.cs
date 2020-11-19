// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Query.Decoder;
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

        private Mock<IHttpRequestSender> _httpSenderMock;
        private StubQuery _query;
        private IList<string> _expectedResponseBody;

        public ApiRequestSenderTest()
        {
            _httpSenderMock = new Mock<IHttpRequestSender>();
            _query = new StubQuery
            {
                Item1 = _queryValue
            };
            _expectedResponseBody = new List<string> { "foo", "bar" };
        }

        [Fact]
        public async Task WhenErrorResponseThenReturnDefaultObject()
        {
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(
                    JsonConvert.SerializeObject(_expectedResponseBody),
                    Encoding.UTF8,
                    "application/json"
                )
            };
            var doRequest = SendRequestAsync<IList<string>>(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Post,
                expectedResponse
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
                System.Net.HttpStatusCode.OK
            );

            Assert.Equal(default(IList<string>), responseBody);

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
            var responseBody = await SendRequestAsync(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Get,
                System.Net.HttpStatusCode.OK,
                _expectedResponseBody
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
            var responseBody = await SendRequestAsync(
                hasValidCredentials: true,
                httpMethod: HttpMethod.Post,
                System.Net.HttpStatusCode.OK,
                _expectedResponseBody
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
            var responseBody = await SendRequestAsync(
                hasValidCredentials: false,
                httpMethod: HttpMethod.Get,
                System.Net.HttpStatusCode.OK,
                _expectedResponseBody
            );

            Assert.Equal(_expectedResponseBody, responseBody);

            // Check Refresh token request
            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery == ApiRequestSender.TokenPath
                    && req.Method == HttpMethod.Post
                )
            ), Times.Once);

            // Check API request
            _httpSenderMock.Verify(sender => sender.SendHttpRequest(
                It.Is<HttpRequestMessage>(
                    req => req.RequestUri.PathAndQuery.Contains(_path)
                    && req.Method == HttpMethod.Get
                    && req.Headers.Authorization.ToString() == _authHeader
                    && req.RequestUri.Query.Contains(_queryString)
                )
            ), Times.Once);
        }

        private async Task<T> SendRequestAsync<T>(
            bool hasValidCredentials,
            HttpMethod httpMethod,
            System.Net.HttpStatusCode responseStatus)
        {
            return await SendRequestAsync<T>(
                hasValidCredentials,
                httpMethod,
                new HttpResponseMessage
                {
                    StatusCode = responseStatus
                }
            );
        }

        private async Task<T> SendRequestAsync<T>(
            bool hasValidCredentials,
            HttpMethod httpMethod,
            System.Net.HttpStatusCode responseStatus,
            T responseBody)
        {
            return await SendRequestAsync<T>(
                hasValidCredentials,
                httpMethod,
                new HttpResponseMessage
                {
                    StatusCode = responseStatus,
                    Content = new StringContent(
                        JsonConvert.SerializeObject(responseBody),
                        Encoding.UTF8,
                        "application/json"
                    )
                }
            );
        }

        private async Task<T> SendRequestAsync<T>(
            bool hasValidCredentials,
            HttpMethod httpMethod,
            HttpResponseMessage response)
        {
            var httpSenderMockSetup = _httpSenderMock
                .Setup(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(req => req.RequestUri.PathAndQuery != ApiRequestSender.TokenPath)
                ));
            if (response.IsSuccessStatusCode)
            {
                httpSenderMockSetup.Returns(Task.FromResult(response));
            }
            else
            {
                httpSenderMockSetup.ThrowsAsync(new HttpRequestException(""));
            }

            _httpSenderMock
                .Setup(sender => sender.SendHttpRequest(
                    It.Is<HttpRequestMessage>(req => req.RequestUri.PathAndQuery == ApiRequestSender.TokenPath)
                ))
                .Returns(Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    }
                ));

            return await CreateApiRequestSender(hasValidCredentials).SendRequestAsync(
                new ApiRequest<T>
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
