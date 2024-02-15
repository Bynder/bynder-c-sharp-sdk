// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class used to convert bool to string.
    /// True is represented as "1" and false as "0"
    /// </summary>
    public class BoolConverter : ITypeToStringConverter
    {
        private const string _trueString = "1";
        private const string _falseString = "0";

        /// <summary>
        /// Returns true if type is assignable from bool
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true if type is assignable from bool/></returns>
        public bool CanConvert(Type typeToConvert)
        {
            return typeof(bool).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Converts bool to string represented as "1" or "0"
        /// </summary>
        /// <param name="value">bool value</param>
        /// <returns>converted string</returns>
        public string Convert(object value)
        {
            if (value is bool boolValue)
            {
                return boolValue
                    ? _trueString
                    : _falseString;
            }

            return string.Empty;
        }
    }
}
