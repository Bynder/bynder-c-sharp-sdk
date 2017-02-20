// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Api.Converters;
using Bynder.Api.Queries;
using NUnit.Framework;

namespace Bynder.Test.Converters
{
    /// <summary>
    /// Class to test <see cref="QueryDecoder"/> 
    /// </summary>
    [TestFixture]
    public class QueryDecoderTest
    {
        /// <summary>
        /// Tests that <see cref="QueryDecoder.GetParameters(object)"/> returns 
        /// only the parameters properties that have the <see cref="APIField"/> attribute.
        /// </summary>
        [Test]
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
            Assert.AreEqual(2, parameters.Count);

            Assert.AreEqual("1", parameters["Item1"]);
            Assert.AreEqual("2", parameters["Item2"]);
        }

        /// <summary>
        /// Tests that <see cref="QueryDecoder.GetParameters(object)"/> calls 
        /// Converter for properties that specify converter
        /// </summary>
        [Test]
        public void WhenQueryAttributeHasConverterThenParameterValueIsConverted()
        {
            var queryDecoder = new QueryDecoder();
            var parameters = queryDecoder.GetParameters(new StubConverterQuery
            {
                Item1 = "1"
            });

            Assert.AreEqual("Converted", parameters["Item1"]);
        }

        /// <summary>
        /// Stub class only for testing purposes
        /// </summary>
        private class StubQuery
        {
            /// <summary>
            /// Stub property
            /// </summary>
            [APIField("Item1")]
            public string Item1 { get; set; }

            /// <summary>
            /// Stub property
            /// </summary>
            [APIField("Item2")]
            public string Item2 { get; set; }

            /// <summary>
            /// Stub property
            /// </summary>
            public string Item3 { get; set; }
        }

        /// <summary>
        /// Stub converter only used for testing purposes
        /// </summary>
        private class StubConverter : ITypeToStringConverter
        {
            /// <summary>
            /// Check <see cref="ITypeToStringConverter.CanConvert(Type)"/>
            /// </summary>
            /// <param name="typeToConvert">Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.CanConvert(Type)"/></returns>
            public bool CanConvert(Type typeToConvert)
            {
                return true;
            }

            /// <summary>
            /// Check <see cref="ITypeToStringConverter.Convert(object)"/>
            /// </summary>
            /// <param name="value">Check <see cref="ITypeToStringConverter.Convert(object)"/></param>
            /// <returns>Check <see cref="ITypeToStringConverter.Convert(object)"/></returns>
            public string Convert(object value)
            {
                return "Converted";
            }
        }

        /// <summary>
        /// Stub converter query only used for testing purposes
        /// </summary>
        private class StubConverterQuery
        {
            /// <summary>
            /// Stub property
            /// </summary>
            [APIField("Item1", Converter = typeof(StubConverter))]
            public string Item1 { get; set; }
        }
    }
}
