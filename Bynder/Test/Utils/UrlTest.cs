// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Sdk.Utils;
using Xunit;

namespace Bynder.Test.Utils
{
    public class UrlTest
    {
        [Fact]
        public void ParametersAreConvertedToValidQueryUrl()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "item1", "\"value'" },
                { "item2", "value2" }
            };

            var queryUri = Url.ConvertToQuery(dictionary);
            Assert.Equal("item1=%22value%27&item2=value2", queryUri);
        }

        [Fact]
        public void IfEmptyDictionaryPassedNothingIsReturned()
        {
            Assert.Empty(Url.ConvertToQuery(new Dictionary<string, string>()));
        }
    }
}
