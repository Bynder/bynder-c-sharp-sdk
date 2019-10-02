// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Bynder.Sdk.Settings;

namespace Bynder.Sdk.Service
{
    /// <summary>
    /// Factory to create Bynder Client.
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        /// Creates the client to be used to communicate with Bynder.
        /// </summary>
        /// <returns>Bynder Client.</returns>
        /// <param name="configuration">Configuration used to create the client.</param>
        public static IBynderClient Create(Configuration configuration)
        {
            return new BynderClient(configuration);
        }
    }
}