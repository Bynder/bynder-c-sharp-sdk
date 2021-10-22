// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Exceptions;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Service.Asset;
using Bynder.Sdk.Service.Collection;
using Bynder.Sdk.Service.OAuth;
using Bynder.Sdk.Settings.Validators;
using Bynder.Sdk.Model;
using Bynder.Sdk.Settings;

namespace Bynder.Sdk.Service
{
    /// <summary>
    /// Client implementation of <see cref="IBynderClient"/>.
    /// </summary>
    internal class BynderClient : IBynderClient
    {
        private readonly Configuration _configuration;
        private readonly IApiRequestSender _requestSender;
        private ICredentials _credentials;
        private IOAuthService _oauthService;
        private IAssetService _assetService;
        private ICollectionService _collectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sdk.Service.Client"/> class.
        /// </summary>
        /// <param name="configuration">Client configuration.</param>
        public BynderClient(Configuration configuration)
        {
            new ConfigurationValidator().Validate(configuration);
            _configuration = configuration;
            if (configuration.PermanentToken != null)
            {
                _credentials = new Credentials(configuration.PermanentToken);
            }
            else
            {
                _credentials = new Credentials(configuration.Token);
            }
            _requestSender = ApiRequestSender.Create(_configuration, _credentials, _oauthService);
        }

        /// <summary>
        /// Check <see cref="t:Sdk.Service.IClient"/>
        /// </summary>
        public event EventHandler<Token> OnCredentialsChanged
        {
            add
            {
                _credentials.OnCredentialsChanged += value;
            }

            remove
            {
                _credentials.OnCredentialsChanged -= value;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Sdk.Service.Client"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:Sdk.Service.Client"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:Sdk.Service.Client"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="T:Sdk.Service.Client"/> so
        /// the garbage collector can reclaim the memory that the <see cref="T:Sdk.Service.Client"/> was occupying.</remarks>
        public void Dispose()
        {
            _requestSender.Dispose();
        }

        /// <summary>
        /// Check <see cref="t:Sdk.Service.IClient"/>
        /// </summary>
        /// <returns>Check <see cref="t:Sdk.Service.IClient"/></returns>
        public IAssetService GetAssetService()
        {
            if (!_credentials.AreValid() && !_credentials.CanRefresh)
            {
                throw new MissingTokenException("Access token expired and refresh token is missing. " +
                                                "Either pass a not expired access token through configuration or login through OAuth2");
            }

            return _assetService ?? (_assetService = new AssetService(_requestSender));
        }

        /// <summary>
        /// Check <see cref="t:Sdk.Service.IClient"/>
        /// </summary>
        /// <returns>Check <see cref="t:Sdk.Service.IClient"/></returns>
        public ICollectionService GetCollectionService()
        {
            if (!_credentials.AreValid() && !_credentials.CanRefresh)
            {
                throw new MissingTokenException("Access token expired and refresh token is missing. " +
                                                "Either pass a not expired access token through configuration or login through OAuth2");
            }

            return _collectionService ?? (_collectionService = new CollectionService(_requestSender));
        }

        /// <summary>
        /// Check <see cref="t:Sdk.Service.IClient"/>
        /// </summary>
        /// <returns>Check <see cref="t:Sdk.Service.IClient"/></returns>
        public IOAuthService GetOAuthService()
        {
            return _oauthService ?? (_oauthService = new OAuthService(_configuration, _credentials, _requestSender));
        }
    }
}
