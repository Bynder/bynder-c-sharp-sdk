// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.IO;
using Bynder.Sdk.Api.Converters;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class BooleanJsonConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsBoolean()
        {
            BooleanJsonConverter converter = new BooleanJsonConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.True(converter.CanConvert(typeof(bool)));
        }
    }
}