// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class ListConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsEnumerableOfStrings()
        {
            ListConverter converter = new ListConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(IEnumerable<int>)));
            Assert.True(converter.CanConvert(typeof(IEnumerable<string>)));
        }

        [Fact]
        public void ConvertReturnsJoinedList()
        {
            ListConverter converter = new ListConverter();
            string convertedList = converter.Convert(new List<string> { "item1" });
            Assert.Equal("item1", convertedList);

            convertedList = converter.Convert(new List<string> { "item1" , "item2" });
            Assert.Equal("item1,item2", convertedList);
        }
    }
}
