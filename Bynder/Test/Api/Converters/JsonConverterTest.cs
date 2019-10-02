// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class JsonConverterTest
    {
        [Fact]
        public void ConvertReturnsString()
        {
            JsonConverter converter = new JsonConverter();
            string convertedValue = converter.Convert(new TestClass {
                Name = "name"
            });
            Assert.Equal("{\"Name\":\"name\"}", convertedValue);
        }
    }

    public class TestClass {
        public string Name { get; set; }
    }
}
