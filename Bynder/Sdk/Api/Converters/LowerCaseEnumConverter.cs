// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class used to convert Enums or nullable Enums to their enumerator's name lowered case string.
    /// </summary>
    public class LowerCaseEnumConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Returns true if type is assignable from Enum or a nullable Enum
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true if type is assignable from an Enum or a nullable Enum/></returns>
        public bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum || Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;
        }

        /// <summary>
        /// Converts an Enum or a nullable Enum to it's enumerator's name lowered case string
        /// </summary>
        /// <param name="value">Enum parameter to be converted</param>
        /// <returns>converted string</returns>
        public string Convert(object value)
        {
            return value?.ToString().ToLowerInvariant() ?? string.Empty;
        }
    }
}