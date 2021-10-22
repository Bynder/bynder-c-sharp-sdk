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
        public string Convert(object value)
        {
            switch (value)
            {
                case TagsOrderBy.TagAscending:
                    return "tag asc";
                case TagsOrderBy.TagDescending:
                    return "tag desc";
                case TagsOrderBy.MediaCountAscending:
                    return "mediaCount asc";
                case TagsOrderBy.MediaCountDescending:
                    return "mediaCount desc";
                default:
                    return null;
            }
        }
    }
}
