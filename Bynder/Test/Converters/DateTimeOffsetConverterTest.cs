// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Api.Converters;
using NUnit.Framework;

namespace Bynder.Test.Converters
{
    /// <summary>
    /// Tests for <seealso cref="DateTimeOffsetConverter"/>
    /// </summary>
    [TestFixture]
    public class DateTimeOffsetConverterTest
    {
        /// <summary>
        /// Checks <see cref="DateTimeOffsetConverter.CanConvert(Type)"/> when a correct type is provided
        /// </summary>
        /// <param name="type">Type to be converted</param>
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(DateTimeOffset?))]
        public void WhenCanConvertCorrectTypeThenTrue(Type type)
        {
            Assert.IsTrue(new DateTimeOffsetConverter().CanConvert(type));
        }

        /// <summary>
        /// Checks <see cref="DateTimeOffsetConverter.CanConvert(Type)"/> when an erroneous type is provided
        /// </summary>
        /// <param name="type">Type to be converted</param>
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(object))]
        [TestCase(typeof(string))]
        public void WhenCanConvertWrongTypeThenFalse(Type type)
        {
            Assert.IsFalse(new DateTimeOffsetConverter().CanConvert(type));
        }

        /// <summary>
        /// Checks <see cref="DateTimeOffsetConverter.Convert(object)"/> when an correct value is provided
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <param name="expectedDateString">Expected string value</param>
        [TestCase("01/20/2018 22:12PM", "2018-01-20T22:12:00Z")]
        [TestCase("01/12/2017", "2017-01-12T00:00:00Z")]
        public void WhenConvertCorrectDateTimeOffsetThenCorrectExpectedDateString(DateTime date, string expectedDateString)
        {
            var dateOffset = new DateTimeOffset(date);
            Assert.AreEqual(new DateTimeOffsetConverter().Convert(dateOffset), expectedDateString);
        }

        /// <summary>
        /// Checks <see cref="DateTimeOffsetConverter.Convert(object)"/> when an erroneous type value is provided
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <param name="variable">Value to be converted</param>
        [TestCase(typeof(string), "")]
        [TestCase(typeof(int), 10)]
        [TestCase(typeof(DateTimeOffset?), null)]
        public void WhenConvertNotDateTimeOffsetOrNullThenEmptyString(Type type, object variable)
        {
            object objectCreated = variable;
            if (objectCreated != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type.GetConstructor(Type.EmptyTypes) != null && !type.IsAbstract)
                {
                    // this type is constructable with default constructor
                    objectCreated = Activator.CreateInstance(type);
                }
                else
                {
                    // no default constructor
                    objectCreated = Convert.ChangeType(objectCreated, type);
                }
            }

            Assert.AreEqual(new DateTimeOffsetConverter().Convert(objectCreated), string.Empty);
        }
    }
}
