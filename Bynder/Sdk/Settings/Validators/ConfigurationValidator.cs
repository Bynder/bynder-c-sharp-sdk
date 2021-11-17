// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Exceptions;

namespace Bynder.Sdk.Settings.Validators
{
    /// <summary>
    /// Configuration validator.
    /// </summary>
    internal class ConfigurationValidator
    {
        /// <summary>
        /// Validate the specified configuration has all the required information for the
        /// SDK to work.
        /// Throws if configuration is not valid.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public void Validate(Configuration configuration)
        {
            if (configuration.BaseUrl == null)
            {
                throw new InvalidConfigurationException("Missing Base URL");
            }

            if (configuration.ClientId == null)
            {
                throw new InvalidConfigurationException("Missing Client Id");
            }

            if (configuration.ClientSecret == null)
            {
                throw new InvalidConfigurationException("Missing Client Secret");
            }
        }
    }
}
