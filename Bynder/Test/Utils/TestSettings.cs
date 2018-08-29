// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Configuration;
using Bynder.Api;
using Bynder.Api.Impl.Oauth;

namespace Bynder.Test.Utils
{
    /// <summary>
    /// Mock data for test configuration used in different tests.
    /// </summary>
    internal static class TestConfiguration
    {
        /// <summary>
        /// Gets Test Settings 
        /// </summary>
        /// <returns>settings to use in tests</returns>
        public static Settings GetSettings()
        {
            return new Settings
            {
                URL = "http://localhost:8890/",
                TOKEN = "LOGIN_TOKEN",
                TOKEN_SECRET = "LOGIN_TOKEN_SECRET",
                CONSUMER_KEY = "CONSUMER_KEY",
                CONSUMER_SECRET = "CONSUMER_SECRET"
            };
        }

        /// <summary>
        /// Gets Test Credentials
        /// </summary>
        /// <returns>credentials to use in test</returns>
        public static Credentials GetCredentials()
        {
            var settings = GetSettings();
            return new Credentials(settings.CONSUMER_KEY, settings.CONSUMER_SECRET, settings.TOKEN, settings.TOKEN_SECRET);
        }
    }
}
