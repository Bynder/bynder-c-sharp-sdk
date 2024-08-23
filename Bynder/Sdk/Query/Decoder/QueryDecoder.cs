// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Bynder.Sdk.Api.Converters;

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
        private void ConvertProperty(PropertyInfo propertyInfo, object query, IDictionary<string, string> parameters)
        {
            foreach (var attribute in propertyInfo.GetCustomAttributes(true))
            {
                if (attribute is ApiField apiField)
                {
                    object value = propertyInfo.GetValue(query);
                    if (value == null)
                    {
                        return;
                    }

                    if (apiField.Converter == null)
                    {
                        AddParam(parameters, apiField.ApiName, value.ToString());
                    }
                    else if (Activator.CreateInstance(apiField.Converter) is ITypeToStringConverter stringConverter
                        && stringConverter.CanConvert(propertyInfo.PropertyType))
                    {
                        AddParam(parameters, apiField.ApiName, stringConverter.Convert(value));
                    }
                    else if (Activator.CreateInstance(apiField.Converter) is ITypeToDictionaryConverter dictConverter
                        && dictConverter.CanConvert(propertyInfo.PropertyType))
                    {
                        foreach (var item in dictConverter.Convert(value))
                        {
                            AddParam(parameters, $"{apiField.ApiName}{item.Key}", item.Value);
                        }
                    }

                    // No need to continue. Only one ApiField attribute per property
                    return;
                }
            }
        }


        private void AddParam(IDictionary<string, string> parameters, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                parameters.Add(key, value);
            }
        }

    }
}
