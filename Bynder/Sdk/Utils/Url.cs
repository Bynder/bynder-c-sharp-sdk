// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bynder.Sdk.Utils
{
    /// <summary>
    /// URL Utilities Class.
    /// </summary>
    public static class Url
    {
        /// <summary>
        /// Converts dictionary to query parameters.
        /// </summary>
        /// <returns>Escaped query.</returns>
        /// <param name="parameters">dictionary with parameters.</param>
        public static string ConvertToQuery(IDictionary<string, string> parameters)
        {
            var encodedValues = parameters.Keys
                .OrderBy(key => key)
                .Select(key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(parameters[key])}");
            var queryUri = string.Join("&", encodedValues);

            return queryUri;
        }
    }
}