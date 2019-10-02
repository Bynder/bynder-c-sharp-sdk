// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Query.Decoder
{
    /// <summary>
    /// Decoder interface to use in queries to convert from one specific type to string.
    /// </summary>
    public interface IParameterDecoder
    {
        /// <summary>
        /// Checks if the converter can convert a specific type.
        /// </summary>
        /// <param name="typeToConvert">Type to convert from</param>
        /// <returns>true if it can convert the type</returns>
        bool CanConvert(Type typeToConvert);

        /// <summary>
        /// Converts the value to string.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>converted string value</returns>
        string Convert(object value);
    }
}