// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query
{
    /// <summary>
    /// Query to retrieve and refresh tokens.
    /// </summary>
    public class TokenQuery
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        [ApiField("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>The client secret.</value>
        [ApiField("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>The redirect URI.</value>
        [ApiField("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the type of the grant.
        /// </summary>
        /// <value>The type of the grant.</value>
        [ApiField("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [ApiField("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>The scopes.</value>
        [ApiField("scope")]
        public string Scopes { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        [ApiField("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
