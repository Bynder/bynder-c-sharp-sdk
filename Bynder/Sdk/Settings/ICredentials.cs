using System;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Settings
{
    /// <summary>
    /// Credentials interface. An instance of credentials is created
    /// when creating a <see cref="BynderApi"/> and it is updated when login
    /// or when refreshing tokens.
    /// </summary>
    internal interface ICredentials
    {
        /// <summary>
        /// Raised when login or when token is refreshed
        /// </summary>
        event EventHandler<Token> OnCredentialsChanged;

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        string AccessToken { get; }

        /// <summary>
        /// Gets the type of the token. In our case Bearer
        /// </summary>
        /// <value>The type of the token.</value>
        string TokenType { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        string RefreshToken { get; }

        /// <summary>
        /// Checks if credentials are valid or expired
        /// </summary>
        /// <returns><c>true</c>, if credentials are valid, <c>false</c> otherwise.</returns>
        bool AreValid();

        /// <summary>
        /// Update the credentials with the specified token.
        /// </summary>
        /// <param name="token">Token.</param>
        void Update(Token token);
    }
}
