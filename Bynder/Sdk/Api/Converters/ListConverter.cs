// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class used to convert IEnumerable{string} to string.
    /// Concretely it joins all IEnumerable values using ',' as separator
    /// </summary>
    public class ListConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Returns true if type is assignable from IEnumerable of strings
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true if type is assignable from IEnumerable/></returns>
        public bool CanConvert(Type typeToConvert)
        {
            return typeof(IEnumerable<string>).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Converts IEnumerable of strings to comma separated string 
        /// </summary>
        /// <param name="value">IEnumerable value</param>
        /// <returns>converted string</returns>
        public string Convert(object value)
        {
            IEnumerable<string> list = value as IEnumerable<string>;
            if (list != null)
            {
                return string.Join(",", list);
            }

            return string.Empty;
        }
    }
}
