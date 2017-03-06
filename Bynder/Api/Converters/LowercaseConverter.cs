using System;
using Bynder.Models;

namespace Bynder.Api.Converters
{
    public class LowerCaseConverter : ITypeToStringConverter
    {
        public bool CanConvert(Type typeToConvert)
        {
            return typeof(AssetType?).IsAssignableFrom(typeToConvert);
        }

        public string Convert(object value)
        {
            var assetType = value as AssetType?;

            return assetType?.ToString().ToLowerInvariant() ?? string.Empty;
        }
    }
}