using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class MetapropertyOptionsConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsDateTimeOffset()
        {
            var converter = new MetapropertyOptionsConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(bool)));
            Assert.True(converter.CanConvert(typeof(IDictionary<string, IList<string>>)));
        }

        [Fact]
        public void ConvertReturnsStringWithDate()
        {
            const string metaprop1 = "metaprop1";
            const string metaprop1option1 = "metaprop1option1";
            const string metaprop1option2 = "metaprop1option2";
            const string metaprop1option3 = "metaprop1option3";

            const string metaprop2 = "metaprop2";
            const string metaprop2option1 = "metaprop2option1";
            const string metaprop2option2 = "metaprop2option2";
            const string metaprop2option3 = "metaprop2option3";

            const string metaprop3 = "metaprop3";
            const string metaprop3option1 = "metaprop3option1";
            const string metaprop3option2 = "metaprop3option2";
            const string metaprop3option3 = "metaprop3option3";

            var converter = new MetapropertyOptionsConverter();
            var converted = converter.Convert(new Dictionary<string, IList<string>>
            {
                { metaprop1, new List<string> { metaprop1option1, metaprop1option2, metaprop1option3 } },
                { metaprop2, new List<string> { metaprop2option1, metaprop2option2, metaprop2option3 } },
                { metaprop3, new List<string> { metaprop3option1, metaprop3option2, metaprop3option3 } }
            });
            var expected = new Dictionary<string, string>
            {
                { metaprop1, $"{metaprop1option1},{metaprop1option2},{metaprop1option3}" },
                { metaprop2, $"{metaprop2option1},{metaprop2option2},{metaprop2option3}" },
                { metaprop3, $"{metaprop3option1},{metaprop3option2},{metaprop3option3}" }
            };
            Assert.Equal(expected, converted);
        }
    }
}
