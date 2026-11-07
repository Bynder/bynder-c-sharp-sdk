// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Model;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class LowerCaseEnumListConverterTest
    {
        [Theory]
        [InlineData(typeof(IList<AssetType>))]
        [InlineData(typeof(List<AssetType>))]
        [InlineData(typeof(IEnumerable<AssetType>))]
        [InlineData(typeof(AssetType[]))]
        public void CanConvertWhenTypeIsEnumerableOfEnum(Type typeToConvert)
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            Assert.True(converter.CanConvert(typeToConvert));
        }

        [Theory]
        [InlineData(typeof(AssetType))]
        [InlineData(typeof(AssetType?))]
        [InlineData(typeof(string))]
        [InlineData(typeof(IList<string>))]
        [InlineData(typeof(IList<int>))]
        public void CannotConvertWhenTypeIsNotEnumerableOfEnum(Type typeToConvert)
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            Assert.False(converter.CanConvert(typeToConvert));
        }

        [Fact]
        public void ConvertReturnsLowerCaseStringForOneValue()
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            string convertedValue = converter.Convert(new List<AssetType> { AssetType.Image });
            Assert.Equal("image", convertedValue);
        }

        [Fact]
        public void ConvertReturnsCommaSeparatedLowerCaseStringInOrder()
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            string convertedValue = converter.Convert(new List<AssetType> { AssetType.Video, AssetType.Document, AssetType.Audio });
            Assert.Equal("video,document,audio", convertedValue);
        }

        [Fact]
        public void ConvertReturnsEmptyStringForEmptyList()
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            string convertedValue = converter.Convert(new List<AssetType>());
            Assert.Equal(string.Empty, convertedValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void ConvertReturnsEmptyStringWhenValueIsNotEnumerable(object value)
        {
            LowerCaseEnumListConverter converter = new LowerCaseEnumListConverter();
            Assert.Equal(string.Empty, converter.Convert(value));
        }
    }
}
