// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Reflection;
using Bynder.Api.Converters;

namespace Bynder.Api.Queries
{
    /// <summary>
    /// Class that gets a query and extracts all <see cref="APIField"/> properties with the 
    /// values and returns them in a <see cref="NameValueCollection"/>.
    /// </summary>
    internal class QueryDecoder
    {
        /// <summary>
        /// Given a query object it gets the parameters. The parameters are basically the properties
        /// of the object query that have <see cref="APIField"/> attributes.
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>collection with name and value to send to the API</returns>
        public NameValueCollection GetParameters(object query)
        {
            NameValueCollection parameters = new NameValueCollection();

            if (query != null)
            {
                foreach (var propertyInfo in query.GetType().GetProperties())
                {
                    ConvertProperty(propertyInfo, query, parameters);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Function called for each property in a query object. It extracts the different properties 
        /// with <see cref="APIField"/> attribute and, if needed, calls appropiate converter to convert property value to string
        /// </summary>
        /// <param name="propertyInfo">property type information</param>
        /// <param name="query">query object</param>
        /// <param name="collection">collection to add the converted values</param>
        private void ConvertProperty(PropertyInfo propertyInfo, object query, NameValueCollection collection)
        {
            var attributes = propertyInfo.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                APIField nameAttr = attribute as APIField;
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

                    // No need to continue. Only one APIField attribute per property
                    return;
                }
            }
        }

        /// <summary>
        /// Function called to convert property values to string. If no converter is
        /// specified, then .ToString is called
        /// </summary>
        /// <param name="apiField">API field attribute</param>
        /// <param name="propertyType">property type information</param>
        /// <param name="value">current value</param>
        /// <returns>converted value</returns>
        private string ConvertPropertyValue(APIField apiField, Type propertyType, object value)
        {
            string convertedValue = null;
            bool isConverted = false;
            if (apiField.Converter != null)
            {
                ITypeToStringConverter converter = Activator.CreateInstance(apiField.Converter) as ITypeToStringConverter;

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
