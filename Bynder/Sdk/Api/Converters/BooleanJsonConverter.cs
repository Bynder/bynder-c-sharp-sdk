// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace Bynder.Sdk.Api.Converters
{
    /// <summary>
    /// Class used during Json deserialization to convert 0/1 to boolean members. We need it because the default bool converter only works if
    /// value to convert is "true"/"false".
    /// </summary>
    internal class BooleanJsonConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// Checks if this converter can convert to a specific type.
        /// </summary>
        /// <param name="objectType">Type we want to convert to</param>
        /// <returns>true if can be converted</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        /// <summary>
        /// Converts json value to boolean.
        /// </summary>
        /// <param name="reader">reader to get the value</param>
        /// <param name="objectType">type we want to convert to</param>
        /// <param name="existingValue">existing value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns>true if string value of the object is "1"</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLower().Trim();
            return value == "1";
        }

        /// <summary>
        /// Converts value to json
        /// </summary>
        /// <param name="writer">json writer</param>
        /// <param name="value">value to serialize</param>
        /// <param name="serializer">json serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Not implemented. Just need to serialize to json
        }
    }
}
