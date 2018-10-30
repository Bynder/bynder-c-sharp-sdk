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
        private const string THUMBNAIL_PREFX = "thumbnails";
        private readonly Type[] _types;


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

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert is Media;
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="NotImplementedException">Unnecessary because CanWrite is false. The type will skip the converter.</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
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
            var media = serializer.Deserialize<Media>(jsonObject.CreateReader());
            media = ConvertMediaProperties(jsonObject, media);
            media = ConvertMediaThumbnails(jsonObject, media);
            return media;
        }

        /// <summary>
        /// Converts the media properties into a dictionary.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="mediaObject">The media object.</param>
        /// <returns></returns>
        private Media ConvertMediaProperties(JObject jsonObject, Media mediaObject)
        {
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
            mediaObject.PropertyOptionsDictionary = mediaProperties;
            return mediaObject;
        }

        /// <summary>
        /// Converts the thumbnails inside the media.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="mediaObject">The media object.</param>
        /// <returns></returns>
        private Media ConvertMediaThumbnails(JObject jsonObject, Media mediaObject)
        {
            var thumbnailProperties = new Dictionary<string, string>();

            var thumbnailsProperty = jsonObject.Properties().FirstOrDefault(p => p.Name.StartsWith(THUMBNAIL_PREFX));
            if (thumbnailsProperty != null)
            {
                foreach (var jToken in thumbnailsProperty.Values())
                {
                    if (jToken is JProperty item)
                    {
                        thumbnailProperties[item.Name] = item.Value.Value<string>();
                    }
                }
            }
            mediaObject.Thumbnails.All = thumbnailProperties;
            return mediaObject;
        }
    }
}