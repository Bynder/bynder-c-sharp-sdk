// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class LowerCaseEnumConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsEnum()
        {
            LowerCaseEnumConverter converter = new LowerCaseEnumConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(bool)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.True(converter.CanConvert(typeof(Example)));
        }

        [Fact]
        public void ConvertReturnsLowerCaseString()
        {
            LowerCaseEnumConverter converter = new LowerCaseEnumConverter();
            string convertedValue = converter.Convert(Example.Example);
            Assert.Equal("example", convertedValue);
        }
    }

    public enum Example { Example };
}
