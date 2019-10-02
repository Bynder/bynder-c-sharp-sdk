// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Exceptions;
using Bynder.Sdk.Settings;

namespace Bynder.Sdk.Settings.Validators
{
    /// <summary>
    /// OAuth2 service configuration validator.
    /// </summary>
    internal class OAuth2ServiceConfigurationValidator
    {
        /// <summary>
        /// Validates the specified configuration is valid to be used by <see cref="OAuthService"/>.
        /// Throws if configuration is not valid.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        internal void Validate(Configuration configuration)
        {
            if (configuration.RedirectUri == null)
            {
                throw new InvalidConfigurationException("Missing Client Secret");
            }
        }
    }
}
