// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Model;
using Xunit;

namespace Bynder.Test.Model
{
    public class TokenTest
    {
        [Fact]
        public void SetAccessTokenExpirationAddsExpiresInToCurrentDate() {
            var token = new Token
            {
                AccessToken = string.Empty,
                ExpiresIn = 3600
            };
            token.SetAccessTokenExpiration();

            var currentDate = DateTimeOffset.Now;
            var expirationDate = token.AccessTokenExpiration.AddSeconds(-3600);
            Assert.Equal(currentDate.Date, expirationDate.Date);
        }
    }
}
