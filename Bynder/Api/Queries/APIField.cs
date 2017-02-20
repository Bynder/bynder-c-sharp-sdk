// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Class to be used as attributes for properties in queries to specify
    /// if property needs to be send as parameter to the API
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class APIField : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="name">Name of the field in the API documentation</param>
        public APIField(string name)
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
    }
}
