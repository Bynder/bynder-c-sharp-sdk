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

        /// <summary>
        /// Merges BaseUrl with a RelativePath
        /// </summary>
        /// <param name="baseUrl">The baseUrl of the request</param>
        /// <param name="urlPath">The relative url of the request</param>
        /// <returns>A valid uri merge the base and the relative uri</returns>
        public static Uri MergeBaseUrlAndRelativePath(string baseUrl, string urlPath)
        {
            // for the relative url to be merged correctly the following must be true
            // base url should always end with /
            // relative url should never begin with a /
            return new Uri(new Uri(baseUrl.TrimEnd('/')+"/"), urlPath.TrimStart('/'));
        }
    }
}
