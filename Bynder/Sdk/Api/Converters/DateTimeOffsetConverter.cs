// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class to convert DateTime to a string formated as followed yyyy-MM-ddTHH:mm:ssZ
    /// </summary>
    public class DateTimeOffsetConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Checks if the converter can convert a specific type
        /// </summary>
        /// <param name="typeToConvert">Type to convert from</param>
        /// <returns>true if the type is <see cref="DateTimeOffset"/></returns>
        public bool CanConvert(Type typeToConvert)
        {       
            return typeof(DateTimeOffset) == typeToConvert || typeof(DateTimeOffset) == Nullable.GetUnderlyingType(typeToConvert);
        }

        /// <summary>
        /// Converts the date to string
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted string value</returns>
        public string Convert(object value)
        {
            if (value is DateTimeOffset)
            {
                return $"{((DateTimeOffset)value).ToString("s")}Z";
            }

            return string.Empty;
        }
    }
}
