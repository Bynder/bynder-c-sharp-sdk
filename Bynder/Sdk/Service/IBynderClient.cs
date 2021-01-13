// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Service.Asset;
using Bynder.Sdk.Service.Collection;
using Bynder.Sdk.Service.OAuth;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Service
{
    /// <summary>
    /// Bynder Client interface.
    /// </summary>
    public interface IBynderClient : IDisposable
    {
        /// <summary>
        /// Occurs when credentials changed, and that happens every time
        /// the access token is refreshed.
        /// </summary>
        event EventHandler<Token> OnCredentialsChanged;

        /// <summary>
        /// Gets the asset service to interact with assets in your Bynder portal.
        /// </summary>
        /// <returns>The asset service.</returns>
        IAssetService GetAssetService();

        /// <summary>
        /// Gets the collection service to interact with collections in your Bynder portal.
        /// </summary>
        /// <returns>The collection service.</returns>
        ICollectionService GetCollectionService();

        /// <summary>
        /// Gets the OAuth service.
        /// </summary>
        /// <returns>The OAuth service.</returns>
        IOAuthService GetOAuthService();
    }
}
