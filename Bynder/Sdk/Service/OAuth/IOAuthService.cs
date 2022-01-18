// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Bynder.Sdk.Service.OAuth
{
    /// <summary>
    /// Oauth service interface. Service that handles OAuth logic.
    /// </summary>
    public interface IOAuthService
    {
        /// <summary>
        /// Gets the authorisation URL.
        /// </summary>
        /// <returns>The authorisation URL.</returns>
        /// <param name="state">State string to be checked to avoid CSRF. https://auth0.com/docs/protocols/oauth2/oauth-state</param>
        string GetAuthorisationUrl(string state);

        /// <summary>
        /// Gets an access token using client credentials.
        /// </summary>
        /// <returns>The task to get the access token and update the credentials with it.</returns>
        Task GetAccessTokenAsync();

        /// <summary>
        /// Gets an access token using the code authorization grant.
        /// </summary>
        /// <returns>The task to get the access token and update the credentials with it.</returns>
        /// <param name="code">Code received after the redirect</param>
        Task GetAccessTokenAsync(string code);

        /// <summary>
        /// Gets a refresh token.
        /// </summary>
        /// <returns>The task to get the refresh token</returns>
        Task GetRefreshTokenAsync();
    }
}
