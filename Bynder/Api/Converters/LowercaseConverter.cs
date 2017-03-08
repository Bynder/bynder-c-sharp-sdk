using System;

namespace Bynder.Api.Converters
{
    public class LowerCaseEnumConverter : ITypeToStringConverter
    {
        public bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum || Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;
        }

        public string Convert(object value)
        {
            return value?.ToString().ToLowerInvariant() ?? string.Empty;
        }
    }
}