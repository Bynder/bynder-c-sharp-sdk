// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Model;
using Bynder.Sdk.Settings;
using Xunit;

namespace Bynder.Test.Settings
{
    public class CredentialsTest
    {
        [Fact]
        public void ValidReturnsTrueIfValidToken()
        {
            CheckValid(45, true);
        }

        [Fact]
        public void ValidReturnsFalseIfItExpiresInLessThan15Seconds()
        {
            CheckValid(10, false);
        }


        [Fact]
        public void ValidReturnsFalseIfTokenAlreadyExpired()
        {
            CheckValid(-5, false);
        }

        private void CheckValid(int expiresIn, bool expectedValid)
        {
            var token = new Token
            {
                AccessToken = string.Empty,
                ExpiresIn = expiresIn,
            };
            Credentials credentials = new Credentials();
            credentials.Update(token);
            Assert.Equal(credentials.AreValid(), expectedValid);
        }

    }
}
