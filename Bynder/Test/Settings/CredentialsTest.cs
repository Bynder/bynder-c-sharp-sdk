// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Model;
using Bynder.Sdk.Settings;
using Moq;
using Xunit;

namespace Bynder.Test.Settings
{
    public class CredentialsTest
    {
        [Fact]
        public void ValidReturnsTrueIfValidToken()
        {
            var token = new Token() {
                AccessToken = string.Empty,
                ExpiresIn = 45
            };
            token.SetAccessTokenExpiration();

            Credentials credentials = new Credentials(token);
            Assert.True(credentials.AreValid());
        }

        [Fact]
        public void ValidReturnsFalseIfItExpiresInLessThan15Seconds()
        {
            var token = new Token() {
                AccessToken = string.Empty,
                ExpiresIn = 10
            };
            token.SetAccessTokenExpiration();

            Credentials credentials = new Credentials(token);
            Assert.False(credentials.AreValid());
        }


        [Fact]
        public void ValidReturnsFalseIfTokenAlreadyExpired()
        {
             var token = new Token() {
                AccessToken = string.Empty,
                ExpiresIn = -5
            };
            token.SetAccessTokenExpiration();

            Credentials credentials = new Credentials(token);
            Assert.False(credentials.AreValid());
        }
    }
}
