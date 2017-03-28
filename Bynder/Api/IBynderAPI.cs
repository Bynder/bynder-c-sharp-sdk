// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Bynder.Models;

namespace Bynder.Api
{
    /// <summary>
    /// Interface to login to Bynder and then get instance of <see cref="IAssetBankManager"/>
    /// </summary>
    public interface IBynderApi : IDisposable
    {
        /// <summary>
        /// Log in using API. To be able to use this method we need to provide an access token/secret with login permissions in <see cref="Settings"/>
        /// </summary>
        /// <param name="email">email/username</param>
        /// <param name="password">password</param>
        /// <returns>Task with the user information</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<User> LoginAsync(string email, string password);

        /*
         * To Login using the browser so only consumer key/secret are needed. Client has to do the following:
         * 1. Request temporary Tokens -> GetRequestTokenAsync
         * 2. Get Authorized Url to open the browser -> GetAuthorizeUrl
         * 3. Wait until user enters credentials and browser is redirected to callback Url.
         * 4. Request final access tokens -> GetAccessTokenAsync
         * 
         * Example can be found in Bynder.Sample project
         */

        /// <summary>
        /// Gets access token once the user has already logged in through the browser,
        /// </summary>
        /// <returns>Task to represent the access token process</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task GetAccessTokenAsync();

        /// <summary>
        /// Gets temporary tokens to use when login through the browser
        /// </summary>
        /// <returns>Task to represent the request of temp tokens</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task GetRequestTokenAsync();

        /// <summary>
        /// Gets the Url we need to open with the browser so the user can login and
        /// we can exchange temp tokes for final access tokens
        /// </summary>
        /// <param name="callbackUrl">Callback Url to be redirected when login is successful</param>
        /// <returns>string with the Url we need to open the browser with</returns>
        string GetAuthorizeUrl(string callbackUrl);

        /// <summary>
        /// Logout resets your credential. If the access token/secret provided in the <see cref="Settings"/> have full permission,
        /// even after this call, calls to all API functions will still work
        /// </summary>
        void Logout();

        /// <summary>
        /// Get asset bank manager to perform asset bank operations
        /// </summary>
        /// <returns>instance of asset manager.</returns>
        IAssetBankManager GetAssetBankManager();

        /// <summary>
        /// Get collection manager to perform collections operations
        /// </summary>
        /// <returns>instance of collections manager</returns>
        ICollectionsManager GetCollectionsManager();
    }
}
