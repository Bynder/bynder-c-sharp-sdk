// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Api.Converters;
using NUnit.Framework;

namespace Bynder.Test.Converters
{
    /// <summary>
    /// Tests for <seealso cref="LowerCaseEnumConverter"/>
    /// </summary>
    [TestFixture]
    public class LowerCaseEnumConverterTest
    {
        /// <summary>
        /// Enum used for Testing
        /// </summary>
        public enum TestEnum
        {
            /// <summary>
            /// First option
            /// </summary>
            First,

            /// <summary>
            /// Second option
            /// </summary>
            Second
        }

        /// <summary>
        /// Checks <see cref="LowerCaseEnumConverter.CanConvert(Type)"/> when a correct type is provided
        /// </summary>
        /// <param name="type">Type to be converted</param>
        [TestCase(typeof(TestEnum))]
        public void WhenCanConvertCorrectTypeThenTrue(Type type)
        {
            Assert.IsTrue(new LowerCaseEnumConverter().CanConvert(type));
        }

        /// <summary>
        /// Checks <see cref="LowerCaseEnumConverter.CanConvert(Type)"/> when an erroneous type is provided
        /// </summary>
        /// <param name="type">Type to be converted</param>
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(object))]
        [TestCase(typeof(string))]
        public void WhenCanConvertWrongTypeThenFalse(Type type)
        {
            Assert.IsFalse(new LowerCaseEnumConverter().CanConvert(type));
        }

        /// <summary>
        /// Checks <see cref="LowerCaseEnumConverter.Convert(object)"/> when an correct value is provided
        /// </summary>
        /// <param name="testEnumOption">Value of the <see cref="TestEnum"/> to be converted</param>
        [TestCase("First")]
        [TestCase("Second")]
        public void WhenConvertCorrectEnumThenCorrectExpectedDateString(string testEnumOption)
        {
            var option = (TestEnum)Enum.Parse(typeof(TestEnum), testEnumOption, true);
            Assert.AreEqual(new LowerCaseEnumConverter().Convert(option), testEnumOption.ToLower());
        }
    }
}
