// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Api.Converters;
using NUnit.Framework;

namespace Bynder.Test.Converters
{
    /// <summary>
    /// Tests for <seealso cref="ListConverter"/>
    /// </summary>
    [TestFixture]
    public class ListConverterTest
    {
        /// <summary>
        /// Tests that <seealso cref="ListConverter.Convert(object)"/> converts a list of strings
        /// in a comma separated string
        /// </summary>
        [Test]
        public void WhenListOfStringConvertedThenReturnsCommaSeparatedValues()
        {
            ListConverter converter = new ListConverter();
            var convertedValue = converter.Convert(new List<string>
            {
                "item1",
                "item2",
                "item3"
            });

            Assert.AreEqual("item1,item2,item3", convertedValue);
        }

        /// <summary>
        /// Tests that <seealso cref="ListConverter.CanConvert(System.Type)"/> ony returns true for
        /// IEnumerable types
        /// </summary>
        [Test]
        public void WhenCanConvertCalledWithTypeThenOnlyReturnsToForIEnumerabletypes()
        {
            var converter = new ListConverter();
            Assert.IsTrue(converter.CanConvert(typeof(List<string>)));
            Assert.IsTrue(converter.CanConvert(typeof(IEnumerable<string>)));
            Assert.IsFalse(converter.CanConvert(typeof(Dictionary<string, string>)));
        }
    }
}
