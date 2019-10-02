// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace Bynder.Sdk.Query.Decoder
{
    /// <summary>
    /// Decodes query object to a dictionary of parameters.
    /// </summary>
    internal class QueryDecoder
    {
        /// <summary>
        /// Given a query object it gets the parameters. The parameters are basically the properties
        /// of the object query that have <see cref="ApiField"/> attributes.
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>collection with name and value to send to the API</returns>
        public IDictionary<string, string> GetParameters(object query)
        {
            var parameters = new Dictionary<string, string>();

            if (query != null)
            {
                foreach (var propertyInfo in query.GetType().GetTypeInfo().GetProperties())
                {
                    ConvertProperty(propertyInfo, query, parameters);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Function called for each property in a query object. It extracts the different properties
        /// with <see cref="ApiField"/> attribute and, if needed, calls appropiate converter to convert property value to string.
        /// </summary>
        /// <param name="propertyInfo">property type information</param>
        /// <param name="query">query object</param>
        /// <param name="collection">collection to add the converted values</param>
        private void ConvertProperty(PropertyInfo propertyInfo, object query, IDictionary<string, string> collection)
        {
            var attributes = propertyInfo.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                ApiField nameAttr = attribute as ApiField;
                if (nameAttr != null)
                {
                    object value = propertyInfo.GetValue(query);
                    if (value != null)
                    {
                        var convertedValue = ConvertPropertyValue(nameAttr, propertyInfo.PropertyType, value);
                        if (!string.IsNullOrEmpty(convertedValue))
                        {
                            collection.Add(nameAttr.ApiName, convertedValue);
                        }
                    }

                    // No need to continue. Only one ApiField attribute per property
                    return;
                }
            }
        }

        /// <summary>
        /// Function called to convert property values to string. If no converter is
        /// specified, then .ToString is called.
        /// </summary>
        /// <param name="apiField">API field attribute</param>
        /// <param name="propertyType">property type information</param>
        /// <param name="value">current value</param>
        /// <returns>converted value</returns>
        private string ConvertPropertyValue(ApiField apiField, Type propertyType, object value)
        {
            string convertedValue = null;
            bool isConverted = false;
            if (apiField.Converter != null)
            {
                IParameterDecoder converter = Activator.CreateInstance(apiField.Converter) as IParameterDecoder;

                if (converter != null
                    && converter.CanConvert(propertyType))
                {
                    convertedValue = converter.Convert(value);
                    isConverted = true;
                }
            }

            if (!isConverted)
            {
                convertedValue = value.ToString();
            }

            return convertedValue;
        }
    }
}
