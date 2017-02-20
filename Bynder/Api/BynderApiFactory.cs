// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Api.Impl;

namespace Bynder.Api
{
    /// <summary>
    /// Static class to create a <see cref="IBynderApi"/> instance
    /// </summary>
    public static class BynderApiFactory
    {
        /// <summary>
        /// Create a <see cref="IBynderApi"/> using the given settings
        /// </summary>
        /// <param name="settings">Settings to configure Bynder API</param>
        /// <returns><see cref="IBynderApi"/> instance</returns>
        public static IBynderApi Create(Settings settings)
        {
            return BynderApi.Create(settings);
        }
    }
}
