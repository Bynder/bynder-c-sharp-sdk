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
        public long Total => GetTotalCount();

        private long GetTotalCount()
        {
            if (!TryGetValue("total", out var total))
                return 0;

            if (total is long totalLong)
                return totalLong;

            return 0;
        }

        public IEnumerable<CountValue> GetCountValues(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!TryGetValue(propertyName, out var count))
                return null;

            if (!(count is JObject countJObject))
                return null;

            return countJObject.Children()
                .Where(child => child is JProperty)
                .Select(child => child as JProperty)
                .Select(jProperty =>
                    new CountValue()
                    {
                        PropertyOptionKey = jProperty.Name,
                        Count = jProperty.Value.Value<int>()
                    }
                );
        }
    }
}
