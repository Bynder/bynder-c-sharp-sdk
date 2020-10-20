// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Test.Utils;
using Xunit;

namespace Bynder.Test.Api.RequestSender
{
    public class HttpRequestSenderTest
    {
        [Fact]
        public async Task WhenSuccessReceivedResponseIsReturned()
        {
            using (var httpListener = new TestHttpListener(HttpStatusCode.OK))
            using (var httpRequestSender = new HttpRequestSender())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, httpListener.ListeningUrl);
                var response = await httpRequestSender.SendHttpRequest(requestMessage);
                Assert.Equal(
                    httpRequestSender.UserAgent,
                    response.RequestMessage.Headers.GetValues("User-Agent").First()
                );

            }
        }
        [Fact]
        public async Task WhenErrorReceivedAnExceptionIsThown()
        {
            using (var httpListener = new TestHttpListener(HttpStatusCode.Forbidden))
            using (var httpRequestSender = new HttpRequestSender())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, httpListener.ListeningUrl);
                var doRequest = httpRequestSender.SendHttpRequest(requestMessage);

                await Assert.ThrowsAsync<HttpRequestException>(() => doRequest);
            }
        }
    }
}
