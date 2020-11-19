// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;
using Xunit;

namespace Bynder.Test.Api
{
    public class QueryDecoderTest
    {
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
                Item1 = "1",
                Item2 = "2",
                Item3 = "3"
            });

            // Property Item3 should not appear as it does not have APIField attribute
            Assert.Equal(2, parameters.Count);

            Assert.Equal("1", parameters["Item1"]);
            Assert.Equal("2", parameters["Item2"]);
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
                Item1 = "1"
            });

            Assert.Equal("Converted", parameters["Item1"]);
        }

        /// <summary>
        /// Stub class only for testing purposes.
        /// </summary>
        private class StubQuery
        {
            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField("Item1")]
            public string Item1 { get; set; }

            /// <summary>
            /// Stub property.
            /// </summary>
            [ApiField("Item2")]
            public string Item2 { get; set; }

            /// <summary>
            /// Stub property.
            /// </summary>
            public string Item3 { get; set; }
        }

        /// <summary>
        /// Stub converter only used for testing purposes.
        /// </summary>
        private class StubDecoder : ITypeToStringConverter
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
                return "Converted";
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
            [ApiField("Item1", Converter = typeof(StubDecoder))]
            public string Item1 { get; set; }
        }
    }
}
