// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Bynder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Bynder.Api.Converters
{
    /// <summary>
    /// Class to convert objects to their Json string representation
    /// </summary>
    public class MediaConverter : CustomCreationConverter<Media>
    {
        private const string PROPERTY_PREFX = "property_";
        /// <summary>
        /// Creates an object which will then be populated by the serializer.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// The created object.
        /// </returns>
        public override Media Create(Type objectType)
        {
            return new Media();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The Media with the <see cref="T:PropertyOptionsDictionary" /> populated.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var mediaObject = base.ReadJson(jsonObject.CreateReader(), objectType, existingValue, serializer);
            var mediaProperties = new Dictionary<string, List<string>>();
            foreach (var property in jsonObject.Properties().Where(p => p.Name.StartsWith(PROPERTY_PREFX)).ToList())
            {
                if (property.HasValues)
                {
                    var propertyName = property.Name.Replace(PROPERTY_PREFX, string.Empty);
                    if (!mediaProperties.Keys.Contains(propertyName))
                    {
                        mediaProperties[propertyName] = new List<string>();
                    }
                    property.Value.Values().ToList().ForEach(item => mediaProperties[propertyName].Add(item.Value<string>()));
                }
            }
            if (mediaObject is Media)
            {
                (mediaObject as Media).PropertyOptionsDictionary = mediaProperties;
            }
            return mediaObject;
        }
    }
}