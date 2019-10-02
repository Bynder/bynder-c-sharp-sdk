// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Test.Utils;
using Xunit;

namespace Bynder.Test.Api.RequestSender
{
    public class HttpRequestSenderTest
    {
        [Fact]
        public void WhenErrorReceivedAnExceptionIsThown()
        {
            using (var testHttpListener = new TestHttpListener(HttpStatusCode.Forbidden, null))
            {
                using (HttpRequestSender apiRequestSender = new HttpRequestSender())
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, testHttpListener.ListeningUrl);

                    Assert.ThrowsAsync<HttpRequestException>(async () => await apiRequestSender.SendHttpRequest(requestMessage));
                }
            }
        }
    }
}
