// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class to convert objects to their Json string representation
    /// </summary>
    public class JsonConverter : ITypeToStringConverter
    {
        /// <summary>
        /// Returns always true since it is possible to convert objects to Json strings
        /// </summary>
        /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
        /// <returns>true</returns>
        public bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        /// <summary>
        /// Converts the value to its Json representation using the Newtonsoft.Json.JsonConvert.SerializeObject function
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>converted string</returns>
        public string Convert(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
