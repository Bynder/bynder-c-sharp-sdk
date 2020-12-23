// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Sdk.Api.Requests
{
    /// <summary>
    /// OAuth request.
    /// </summary>
    /// <typeparam name="T">Type to which the response will be deserialized</typeparam>
    internal class OAuthRequest<T> : Request<T>
    {
        public OAuthRequest()
        {
            _authenticated = false;
        }
    }
}
