// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;
using Xunit;

namespace Bynder.Test.Api
{
    public class QueryDecoderTest
    {
        private const string _stringItem1ApiField = "string1ApiField";
        private const string _stringItem2ApiField = "string2ApiField";
        private const string _dictItemApiField = "dictApiField";

        private const string _item = "item";
        private const string _stringItem1 = "string1";
        private const string _stringItem2 = "string2";

        private const string _converted = "converted";

        private const string _dictKey1 = "dictKey1";
        private const string _dictKey2 = "dictKey2";
        private const string _dictKey3 = "dictKey3";
        private const string _dictValue1 = "dictValue1";
        private const string _dictValue2 = "dictValue2";
        private const string _dictValue3 = "dictValue3";

        /// <summary>
        /// Tests that <see cref="QueryDecoder.GetParameters(object)"/> returns
        /// only the parameters properties that have the <see cref="ApiField"/> attribute.
        /// </summary>
        [Fact]
        public void WhenQueryPassedThenOnlyAPIFieldAttributesAreReturned()
        {
            var queryDecoder = new QueryDecoder();
            var parameters = queryDecoder.GetParameters(new StubQuery
            {
                Item = _item,
                StringItem1 = _stringItem1,
                StringItem2 = _stringItem2
            });

            // Unannotated property should not appear as it does not have APIField attribute
            Assert.Equal(2, parameters.Count);

            Assert.Equal(_stringItem1, parameters[_stringItem1ApiField]);
            Assert.Equal(_stringItem2, parameters[_stringItem2ApiField]);
        }

        /// <summary>
        /// Tests that <see cref="QueryDecoder.GetParameters(object)"/> calls
        /// converter for properties that specify converter.
        /// </summary>
        [Fact]
        public void WhenQueryAttributeHasConverterThenParameterValueIsConverted()
        {
            var queryDecoder = new QueryDecoder();
            var parameters = queryDecoder.GetParameters(new StubConverterQuery
            {
                StringItem = _stringItem1,
                DictItem = 42
            });

            Assert.Equal(_converted, parameters[_stringItem1ApiField]);
            Assert.Equal(_dictValue1, parameters[$"{_dictItemApiField}.{_dictKey1}"]);
            Assert.Equal(_dictValue2, parameters[$"{_dictItemApiField}.{_dictKey2}"]);
            Assert.Equal(_dictValue3, parameters[$"{_dictItemApiField}.{_dictKey3}"]);
        }

        /// <summary>
        /// Stub class only for testing purposes.
        /// </summary>
        private class StubQuery
        {
            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField(_stringItem1ApiField)]
            public string StringItem1 { get; set; }

            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField(_stringItem2ApiField)]
            public string StringItem2 { get; set; }

            /// <summary>
            /// Stub property.
            /// </summary>
            public string Item { get; set; }
        }

        /// <summary>
        /// Stub converter only used for testing purposes.
        /// </summary>
        private class StubStringConverter : ITypeToStringConverter
        {
            /// <summary>
            /// Check <see cref="IParameterDecoder.CanConvert(Type)"/>.
            /// </summary>
            /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></returns>
            public bool CanConvert(Type typeToConvert)
            {
                return true;
            }

            /// <summary>
            /// Check <see cref="IParameterDecoder.Convert(object)"/>.
            /// </summary>
            /// <param name="value">Check <see cref="ITypeToStringConverter.Convert(object)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.Convert(object)"/></returns>
            public string Convert(object value)
            {
                return _converted;
            }
        }

        /// <summary>
        /// Stub converter only used for testing purposes.
        /// </summary>
        private class StubDictionaryConverter : ITypeToDictionaryConverter
        {
            /// <summary>
            /// Check <see cref="IParameterDecoder.CanConvert(Type)"/>.
            /// </summary>
            /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></returns>
            public bool CanConvert(Type typeToConvert)
            {
                return true;
            }

            /// <summary>
            /// Check <see cref="IParameterDecoder.Convert(object)"/>.
            /// </summary>
            /// <param name="value">Check <see cref="ITypeToStringConverter.Convert(object)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.Convert(object)"/></returns>
            public IDictionary<string, string> Convert(object value)
            {
                return new Dictionary<string, string>
                {
                    { _dictKey1, _dictValue1 },
                    { _dictKey2, _dictValue2 },
                    { _dictKey3, _dictValue3 }
                };
            }
        }

        /// <summary>
        /// Stub converter query only used for testing purposes.
        /// </summary>
        private class StubConverterQuery
        {
            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField(_stringItem1ApiField, Converter = typeof(StubStringConverter))]
            public string StringItem { get; set; }

            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField(_dictItemApiField, Converter = typeof(StubDictionaryConverter))]
            public int DictItem { get; set; }
        }
    }
}
