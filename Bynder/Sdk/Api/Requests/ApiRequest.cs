// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Sdk.Api.Requests
{
    /// <summary>
    /// API request where the response body can be deserialized into an object with type T.
    /// </summary>
    /// <typeparam name="T">Type to which the response will be deserialized</typeparam>
    internal class ApiRequest<T> : Request<T>
    {
    }

    /// <summary>
    /// API request where the response has an empty body, or a body with an unknown structure.
    /// </summary>
    internal class ApiRequest : Request<object>
    {
    }
}
