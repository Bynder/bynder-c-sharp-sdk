// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bynder.Api;
using Bynder.Api.Impl;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Models;
using Bynder.Test.Utils;
using Moq;
using NUnit.Framework;

namespace Bynder.Test
{
    /// <summary>
    /// Tests the <seealso cref="BynderApi"/> login methods
    /// </summary>
    public class BynderApiTest
    {
        /// <summary>
        /// Tests that when login is done using <seealso cref="LoginQuery"/>, request has appropiate values in
        /// query, HTTPMethod and Url
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenLoginCalledWithLoginQueryThenRequestHasCorrectValues()
        {
            const string Username = "bynder_user";
            const string Password = "bynder_password";
            var oauthRequestSenderMock = new Mock<IOauthRequestSender>();
            var user = new User { Access = true };
            oauthRequestSenderMock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<User>>()))
                .Returns(Task.FromResult(user));

            var credentials = TestConfiguration.GetCredentials();

            var bynderApi = new BynderApi(credentials, null, oauthRequestSenderMock.Object);

            Assert.AreEqual(user, await bynderApi.LoginAsync(Username, Password));

            oauthRequestSenderMock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<User>>(
                    req => ValidateLoginRequest(req, credentials.CONSUMER_KEY, Username, Password))));
        }

        /// <summary>
        /// Tests that Url returned by authorize Url is correct
        /// </summary>
        [Test]
        public void WhenGetAuthorizeUrlCalledThenUrlReturnedHasAppropiateUrlAndToken()
        {
            Settings settings = TestConfiguration.GetSettings();
            IBynderApi api = BynderApiFactory.Create(settings);
            var noCallback = api.GetAuthorizeUrl(null);
            Assert.AreEqual(string.Format("{0}api/v4/oauth/authorise/?oauth_token={1}", settings.URL, settings.TOKEN), noCallback);
        }

        /// <summary>
        /// Tests that Url returned by authorize Url when callback Url is specified is correct
        /// </summary>
        [Test]
        public void WhenGetAuthorizeUrlCalledWithCallbackUrlThenUrlReturnedUrlAddsCallbackParameter()
        {
            Settings settings = TestConfiguration.GetSettings();
            IBynderApi api = BynderApiFactory.Create(settings);
            string callbackUrl = "http://localhost/";
            string encodedUrl = HttpUtility.UrlEncode(callbackUrl);
            var noCallback = api.GetAuthorizeUrl(callbackUrl);
            Assert.AreEqual(string.Format("{0}api/v4/oauth/authorise/?oauth_token={1}&callback={2}", settings.URL, settings.TOKEN, encodedUrl), noCallback);
        }

        /// <summary>
        /// Tests that when Request tokens is called, then credentials are updated and request has correct values
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenRequestTokensCalledThenRequestHasCorrectValues()
        {
            const string OauthToken = "FAKE_TOKEN";
            const string OauthTokenSecret = "FAKE_TOKEN_SECRET";

            var oauthRequestSenderMock = GetIOauthRequestSenderToRespondToTokenCalls(OauthToken, OauthTokenSecret);
            var credentials = TestConfiguration.GetCredentials();

            var bynderApi = new BynderApi(credentials, null, oauthRequestSenderMock.Object);
            await bynderApi.GetRequestTokenAsync();

            oauthRequestSenderMock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<string>>(
                    req => ValidateRequestForTokenCall(req, "/api/v4/oauth/request_token"))));

            Assert.AreEqual(credentials.ACCESS_TOKEN, OauthToken);
            Assert.AreEqual(credentials.ACCESS_TOKEN_SECRET, OauthTokenSecret);
        }

        /// <summary>
        /// Checks that when access token call is called, then credentials are updated and request has correct values
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenAccessTokensCalledThenRequestHasCorrectValues()
        {
            const string OauthToken = "FAKE_TOKEN";
            const string OauthTokenSecret = "FAKE_TOKEN_SECRET";

            var credentials = TestConfiguration.GetCredentials();
            var oauthRequestSenderMock = GetIOauthRequestSenderToRespondToTokenCalls(OauthToken, OauthTokenSecret);
            var bynderApi = new BynderApi(credentials, null, oauthRequestSenderMock.Object);
            await bynderApi.GetAccessTokenAsync();

            oauthRequestSenderMock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<string>>(
                    req => ValidateRequestForTokenCall(req, "/api/v4/oauth/access_token"))));

            Assert.AreEqual(credentials.ACCESS_TOKEN, OauthToken);
            Assert.AreEqual(credentials.ACCESS_TOKEN_SECRET, OauthTokenSecret);
        }

        /// <summary>
        /// Returns Mock <see cref="IOauthRequestSender"/> and sets it up to respond to Request for token calls
        /// </summary>
        /// <param name="oauthToken">oauth token to include in the response</param>
        /// <param name="oauthTokenSecret">oauth token secret to include in the response</param>
        /// <returns>Mock instance of <see cref="IOauthRequestSender"/></returns>
        private Mock<IOauthRequestSender> GetIOauthRequestSenderToRespondToTokenCalls(string oauthToken, string oauthTokenSecret)
        {
            var oauthRequestSenderMock = new Mock<IOauthRequestSender>();
            oauthRequestSenderMock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<string>>()))
                .Returns(Task.FromResult($"oauth_token={oauthToken}&oauth_token_secret={oauthTokenSecret}"));

            return oauthRequestSenderMock;
        }

        /// <summary>
        /// Validates that a request is valid for a token call.
        /// It is validated against Uri specified as parameter and checks that we do not deserialize response
        /// and that is a POST
        /// </summary>
        /// <param name="request">Request to validate</param>
        /// <param name="uri">Uri the request should point to (either /api/v4/oauth/access_token or /api/v4/oauth/request_token</param>
        /// <returns>true if request is valid</returns>
        private bool ValidateRequestForTokenCall(Request<string> request, string uri)
        {
            return request.Uri == uri
                    && request.HTTPMethod == HttpMethod.Post
                    && request.DeserializeResponse == false;
        }

        /// <summary>
        /// Helper function to validate a login request
        /// </summary>
        /// <param name="request">request to validate</param>
        /// <param name="consumerId">consumer id</param>
        /// <param name="username">login username</param>
        /// <param name="password">login password</param>
        /// <returns>if request is valid</returns>
        private bool ValidateLoginRequest(Request<User> request, string consumerId, string username, string password)
        {
            var internalLoginQuery = (LoginQuery)request.Query;

            return request.Uri == $"/api/v4/users/login/"
                && request.HTTPMethod == HttpMethod.Post
                && internalLoginQuery.ConsumerId == consumerId
                && internalLoginQuery.Password == password
                && internalLoginQuery.Username == username;
        }
    }
}
