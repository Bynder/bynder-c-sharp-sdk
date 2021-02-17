using System;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Api.Converters
{
    public class TagsOrderByConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Returns true if type is assignable from IEnumerable of strings
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true if type is assignable from IEnumerable/></returns>
        public bool CanConvert(Type typeToConvert)
        {
            return typeof(TagsOrderBy).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Converts TagsOrderBy enum to string 
        /// </summary>
        /// <param name="value">TagsOrderBy value</param>
        /// <returns>converted string</returns>
        public string Convert(object value) => value switch
        {
            TagsOrderBy.TagAscending => "tag asc",
            TagsOrderBy.TagDescending => "tag desc",
            TagsOrderBy.MediaCountAscending => "mediaCount asc",
            TagsOrderBy.MediaCountDescending => "mediaCount desc",
            _ => null,
        };
    }
}
