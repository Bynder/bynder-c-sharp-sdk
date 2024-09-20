// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Query.Decoder
{
    /// <summary>
    /// Class to be used as attributes for properties in queries to specify
    /// if property needs to be send as parameter to the API
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ApiField : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="name">Name of the field in the API documentation</param>
        public ApiField(string name)
        {
            ApiName = name;
        }

        /// <summary>
        /// Converter to be used to convert the property
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// Name of the property in the API documentation.
        /// </summary>
        public string ApiName { get; private set; }

        /// <summary>
        /// Indicates whether or not the separator (usually a dot) must be omitted when converting
        /// </summary>
        public bool OmitSeparator { get; set; }
    }
}
