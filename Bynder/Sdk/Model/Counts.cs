// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Counts returned by the API
    /// </summary>
    public class Counts : Dictionary<string, object>
    {
        /// <summary>
        /// Total count of results
        /// </summary>
        public long Total => GetTotalCount();

        private long GetTotalCount()
        {
            if (!TryGetValue("total", out var total))
            {
                return 0;
            }

            if (total is long totalLong)
            {
                return totalLong;
            }

            return 0;
        }

        /// <summary>
        /// Gets the counts per option of a property
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        /// <returns>counts per option, or null when the property has no counts</returns>
        public IEnumerable<CountValue> GetCountValues(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!TryGetValue(propertyName, out var count))
            {
                return null;
            }

            if (!(count is JObject countJObject))
            {
                return null;
            }

            return countJObject.Properties().Select(jProperty => new CountValue
            {
                PropertyOptionKey = jProperty.Name,
                Count = jProperty.Value.Value<int>()
            });
        }
    }
}
