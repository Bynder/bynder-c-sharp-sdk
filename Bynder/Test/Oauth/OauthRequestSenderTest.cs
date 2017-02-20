// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Models;
using Bynder.Test.Utils;
using NUnit.Framework;

namespace Bynder.Test.Oauth
{
    /// <summary>
    /// Class to test <see cref="OauthRequestSender"/>
    /// </summary>
    [TestFixture]
    public class OauthRequestSenderTest
    {
        /// <summary>
        /// Tests that an exception is thrown when server returns Forbidden
        /// </summary>
        [Test]
        public void WhenSentRequestAndUnauthorizedThenExceptionIsThrown()
        {
            var settings = TestConfiguration.GetSettings();
            var credentials = new Credentials(
                settings.CONSUMER_KEY,
                settings.CONSUMER_SECRET,
                settings.TOKEN,
                settings.TOKEN_SECRET);

            using (var testHttpListener = new TestHttpListener(settings.URL, HttpStatusCode.Forbidden, null))
            {
                OauthRequestSender api = new OauthRequestSender(credentials, settings.URL);
                var apiRequest = new Request<bool>
                {
                    Uri = "/fake/api",
                    HTTPMethod = HttpMethod.Get
                };
                Assert.ThrowsAsync<HttpRequestException>(async () => await api.SendRequestAsync(apiRequest));
            }
        }

        /// <summary>
        /// Tests that response is deserialized to specified Object.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenRequestSentThenResponseIsDeserializedToObject()
        {
            var settings = TestConfiguration.GetSettings();
            var credentials = TestConfiguration.GetCredentials();

            using (var testHttpListener = new TestHttpListener(settings.URL, HttpStatusCode.OK, "HTTPResponses/User.txt"))
            {
                OauthRequestSender oauthSender = new OauthRequestSender(credentials, settings.URL);
                var apiRequest = new Request<User>
                {
                    Uri = "/fake/api",
                    HTTPMethod = HttpMethod.Get
                };
                var user = await oauthSender.SendRequestAsync(apiRequest);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.TokenKey);
                Assert.IsNotNull(user.TokenSecret);
                Assert.IsNotNull(user.UserId);
            }
        }

        /// <summary>
        /// Tests that oauth header is added for a request.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenSendRequestThenAuthHeaderIsAdded()
        {
            var settings = TestConfiguration.GetSettings();
            var credentials = TestConfiguration.GetCredentials();

            using (var testHttpListener = new TestHttpListener(settings.URL, HttpStatusCode.OK, "HTTPResponses/User.txt"))
            {
                bool containsAuthHeader = false;
                testHttpListener.MessageReceived += (sender, requestEventArgs) =>
                {
                    containsAuthHeader = requestEventArgs.Request.Headers["Authorization"] != null;
                };

                OauthRequestSender oauthSender = new OauthRequestSender(credentials, settings.URL);
                var apiRequest = new Request<User>
                {
                    Uri = "/fake/api",
                    HTTPMethod = HttpMethod.Get
                };
                var user = await oauthSender.SendRequestAsync(apiRequest);
                Assert.IsTrue(containsAuthHeader);
            }
        }

        /// <summary>
        /// Tests that when Request is POST then parameters are added to the body
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenRequestIsPostThenParametersAreAddedToBody()
        {
            var settings = TestConfiguration.GetSettings();
            var credentials = TestConfiguration.GetCredentials();

            var query = new StubQuery
            {
                Item1 = "Value"
            };

            using (var testHttpListener = new TestHttpListener(settings.URL, HttpStatusCode.OK, null))
            {
                bool containsItemInBody = false;
                testHttpListener.MessageReceived += (sender, requestEventArgs) =>
                {
                    var request = requestEventArgs.Request;
                    using (var streamReader = new StreamReader(request.InputStream))
                    {
                        var str = streamReader.ReadToEnd();
                        containsItemInBody = str.Contains("Item1") && str.Contains(query.Item1);
                    }
                };

                OauthRequestSender oauthSender = new OauthRequestSender(credentials, settings.URL);
                var apiRequest = new Request<bool>
                {
                    Uri = "/fake/api",
                    HTTPMethod = HttpMethod.Post,
                    Query = query
                };
                var user = await oauthSender.SendRequestAsync(apiRequest);
                Assert.IsTrue(containsItemInBody);
            }
        }

        /// <summary>
        /// Tests that when request is GET, parameters are added to the Url.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenRequestIsGetThenParametersAreAddedToUrl()
        {
            var settings = TestConfiguration.GetSettings();
            var credentials = TestConfiguration.GetCredentials();

            var query = new StubQuery
            {
                Item1 = "Value"
            };

            using (var testHttpListener = new TestHttpListener(settings.URL, HttpStatusCode.OK, null))
            {
                bool containsItemInUrl = false;
                testHttpListener.MessageReceived += (sender, requestEventArgs) =>
                {
                    var request = requestEventArgs.Request;
                    containsItemInUrl = request.Url.Query.Contains("Item1")
                    && request.Url.Query.Contains("Value");
                };

                OauthRequestSender oauthSender = new OauthRequestSender(credentials, settings.URL);
                var apiRequest = new Request<bool>
                {
                    Uri = "/fake/api",
                    HTTPMethod = HttpMethod.Get,
                    Query = query
                };
                var user = await oauthSender.SendRequestAsync(apiRequest);
                Assert.IsTrue(containsItemInUrl);
            }
        }

        /// <summary>
        /// Stub query for testing purposes.
        /// </summary>
        private class StubQuery
        {
            /// <summary>
            /// Stub property
            /// </summary>
            [APIField("Item1")]
            public string Item1 { get; set; }
        }
    }
}
