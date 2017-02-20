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
                URL = ConfigurationManager.AppSettings["API_BASE_URL"],
                TOKEN = ConfigurationManager.AppSettings["TOKEN"],
                TOKEN_SECRET = ConfigurationManager.AppSettings["TOKEN_SECRET"],
                CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"],
                CONSUMER_SECRET = ConfigurationManager.AppSettings["CONSUMER_SECRET"]
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
