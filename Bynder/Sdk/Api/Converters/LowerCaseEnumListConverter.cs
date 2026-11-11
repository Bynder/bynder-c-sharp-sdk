// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class used to convert IEnumerable of Enums to string.
    /// Concretely it joins the enumerators' names lowered case using ',' as separator
    /// </summary>
    public class LowerCaseEnumListConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Returns true if type is assignable from IEnumerable of an Enum
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true if type is assignable from IEnumerable of an Enum</returns>
        public bool CanConvert(Type typeToConvert)
        {
            var enumerableType = IsGenericEnumerable(typeToConvert) ? typeToConvert : typeToConvert.GetInterfaces().FirstOrDefault(IsGenericEnumerable);

            return enumerableType?.GetGenericArguments()[0].IsEnum == true;
        }

        /// <summary>
        /// Converts IEnumerable of Enums to comma separated string of their enumerators' names lowered case
        /// </summary>
        /// <param name="value">IEnumerable value</param>
        /// <returns>converted string</returns>
        public string Convert(object value)
        {
            if (value is IEnumerable enumerable)
            {
                return string.Join(",", enumerable.Cast<object>().Select(item => item.ToString().ToLowerInvariant()));
            }

            return string.Empty;
        }

        private static bool IsGenericEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}
