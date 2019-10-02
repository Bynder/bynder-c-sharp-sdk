// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class DateTimeOffsetConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsDateTimeOffset()
        {
            DateTimeOffsetConverter converter = new DateTimeOffsetConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(bool)));
            Assert.True(converter.CanConvert(typeof(DateTimeOffset)));
        }

        [Fact]
        public void ConvertReturnsStringWithDate()
        {
            DateTimeOffsetConverter converter = new DateTimeOffsetConverter();
            var date = converter.Convert(new DateTimeOffset(new DateTime(1000, 1, 1)));
            Assert.Equal("1000-01-01T00:00:00Z", date);
        }
    }
}