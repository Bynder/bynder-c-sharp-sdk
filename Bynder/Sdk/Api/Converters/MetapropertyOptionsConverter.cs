using System;
using System.Collections.Generic;
using System.Linq;

namespace Bynder.Sdk.Api.Converters
{
    public class MetapropertyOptionsConverter : ITypeToDictionaryConverter
    {

        public bool CanConvert(Type typeToConvert)
        {
            return typeof(IDictionary<string, IList<string>>).IsAssignableFrom(typeToConvert);
        }

        public IDictionary<string, string> Convert(object value)
        {
            return ((IDictionary<string, IList<string>>)value).ToDictionary(
                item => item.Key,
                item => string.Join(",", item.Value)
            );
        }

    }
}
