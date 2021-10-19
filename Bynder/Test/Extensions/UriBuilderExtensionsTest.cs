using System;
using Bynder.Sdk.Extensions;
using Xunit;

namespace Bynder.Test.Extensions
{
    public class UriBuilderExtensionsTest
    {
        [Fact]
        public void PathsAreCorrectlyAppended()
        {
            var expectedUrl = "https://example.com:443/base/path/to/something";
            Assert.Equal(expectedUrl, new UriBuilder("https://example.com/base").AppendPath("path/to/something").ToString());
            Assert.Equal(expectedUrl, new UriBuilder("https://example.com/base").AppendPath("/path/to/something").ToString());
            Assert.Equal(expectedUrl, new UriBuilder("https://example.com/base/").AppendPath("path/to/something").ToString());
            Assert.Equal(expectedUrl, new UriBuilder("https://example.com/base/").AppendPath("/path/to/something").ToString());
        }
    }

}
