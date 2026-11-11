using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Decoder;
using Bynder.Sdk.Query.Upload;
using Bynder.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bynder.Test.Utils
{
    public class QueryParameterTest
    {
        [Fact]
        public void BooleanPropertiesSetWhenModified()
        {
            ModifyMediaQuery query = new ModifyMediaQuery("")
            {
                Archive = false,
            };

            var parametersAsString = GetQueryParameters(query);
            Assert.DoesNotContain("isPublic=", parametersAsString);
            Assert.Contains("archive=False", parametersAsString);
        }

        [Fact]
        public void MetaPropertyCanBeDeleted()
        {
            ModifyMediaQuery query = new ModifyMediaQuery("")
            {
                MetapropertyOptions = new Dictionary<string, IList<string>>()
                {
                    {
                        "TestProp",
                        []
                    }
                }
            };
            var parametersAsString = GetQueryParameters(query);
            Assert.Contains("metaproperty.TestProp=", parametersAsString);
        }

        [Fact]
        public void StringPropertyCanBeErased()
        {
            ModifyMediaQuery query = new ModifyMediaQuery("")
            {
                Name = "",
                Copyright = null,                
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("name=", parametersAsString);
            Assert.DoesNotContain("copyright=", parametersAsString);
        }

        [Fact]
        public void BooleanFlagsNotSentWhenNotSet()
        {
            MediaWithCountQuery query = new MediaWithCountQuery
            {
                IncludeCount = true,
                IncludeMediaItems = false,
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("count=True", parametersAsString);
            Assert.Contains("includeMediaItems=False", parametersAsString);
            Assert.DoesNotContain("total=", parametersAsString);
            Assert.DoesNotContain("includeVersionNumber=", parametersAsString);
        }

        [Fact]
        public void MetapropertyOptionSearchIsEncodedAndHasNoId()
        {
            MetapropertyOptionSearchQuery query = new MetapropertyOptionSearchQuery
            {
                MetapropertyId = "metapropertyId",
                Name = "option & name",
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("name=option%20%26%20name", parametersAsString);
            Assert.DoesNotContain("id=", parametersAsString);
        }

        [Fact]
        public void MediaQueryListsAreCommaSeparated()
        {
            MediaQuery query = new MediaQuery
            {
                Types = [AssetType.Image, AssetType.Video],
                Tags = ["tag1", "tag2"],
                Orientations = ["landscape", "square"],
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("type=image%2Cvideo", parametersAsString);
            Assert.Contains("tags=tag1%2Ctag2", parametersAsString);
            Assert.Contains("orientation=landscape%2Csquare", parametersAsString);
        }

        [Fact]
        public void MediaQueryTypeReplacesTypes()
        {
            MediaQuery query = new MediaQuery
            {
                Types = [AssetType.Image, AssetType.Video],
            };
            query.Type = AssetType.Document;
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("type=document", parametersAsString);
            Assert.DoesNotContain("image", parametersAsString);
            Assert.Equal(AssetType.Document, query.Type);
        }

        [Fact]
        public void SaveMediaMetapropertyOptionsUseOneDot()
        {
            SaveMediaQuery query = new SaveMediaQuery();
            query.AddMetapropertyOptions("metapropertyId", ["option1", "option2"]);
            query.AddMetapropertyOptions("otherMetapropertyId", ["option3"]);
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("metaproperty.metapropertyId=option1%2Coption2", parametersAsString);
            Assert.Contains("metaproperty.otherMetapropertyId=option3", parametersAsString);
            Assert.DoesNotContain("metaproperty..", parametersAsString);
        }

        [Fact]
        public void SaveMediaWithoutMetapropertyOptionsSendsNoMetaproperty()
        {
            SaveMediaQuery query = new SaveMediaQuery();
            var parametersAsString = GetQueryParameters(query);

            Assert.DoesNotContain("metaproperty", parametersAsString);
        }

        [Theory]
        [InlineData(true, "total=True")]
        [InlineData(false, "total=False")]
        public void MediaWithTotalSendsTotalWhenSet(bool includeTotal, string expected)
        {
            MediaWithTotalQuery query = new MediaWithTotalQuery
            {
                IncludeTotal = includeTotal,
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains(expected, parametersAsString);
        }

        [Fact]
        public void MediaWithTotalSendsNoTotalWhenNotSet()
        {
            MediaWithTotalQuery query = new MediaWithTotalQuery();
            var parametersAsString = GetQueryParameters(query);

            Assert.DoesNotContain("total=", parametersAsString);
        }

        [Fact]
        public void MediaQueryTypeIsNullWhenTypesNotSet()
        {
            MediaQuery query = new MediaQuery();

            Assert.Null(query.Type);
        }

        [Fact]
        public void MediaQueryTypeIsNullWhenTypesEmpty()
        {
            MediaQuery query = new MediaQuery
            {
                Types = [],
            };

            Assert.Null(query.Type);
        }

        [Fact]
        public void MediaQueryTypeIsFirstOfTypes()
        {
            MediaQuery query = new MediaQuery
            {
                Types = [AssetType.Video, AssetType.Image],
            };

            Assert.Equal(AssetType.Video, query.Type);
        }

        [Fact]
        public void MediaQueryTypeAloneIsSent()
        {
            MediaQuery query = new MediaQuery
            {
                Type = AssetType.Image,
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("type=image", parametersAsString);
        }

        [Fact]
        public void MediaQueryTypeSetToNullClearsTypes()
        {
            MediaQuery query = new MediaQuery
            {
                Types = [AssetType.Image, AssetType.Video],
            };
            query.Type = null;
            var parametersAsString = GetQueryParameters(query);

            Assert.Null(query.Types);
            Assert.DoesNotContain("type=", parametersAsString);
        }

        [Fact]
        public void MediaQueryListsNotSentWhenNotSet()
        {
            MediaQuery query = new MediaQuery();
            var parametersAsString = GetQueryParameters(query);

            Assert.DoesNotContain("type=", parametersAsString);
            Assert.DoesNotContain("tags=", parametersAsString);
            Assert.DoesNotContain("orientation=", parametersAsString);
        }

        [Fact]
        public void MediaQueryTagsAreEncoded()
        {
            MediaQuery query = new MediaQuery
            {
                Tags = ["summer & sun", "Ölgemälde"],
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.Contains("tags=summer%20%26%20sun%2C%C3%96lgem%C3%A4lde", parametersAsString);
        }

        [Fact]
        public void MetapropertyOptionSearchWithoutNameSendsNoName()
        {
            MetapropertyOptionSearchQuery query = new MetapropertyOptionSearchQuery
            {
                MetapropertyId = "metapropertyId",
            };
            var parametersAsString = GetQueryParameters(query);

            Assert.DoesNotContain("name=", parametersAsString);
            Assert.Contains("limit=50", parametersAsString);
            Assert.Contains("page=1", parametersAsString);
        }

        private string GetQueryParameters(object query)
        {
            QueryDecoder queryDecoder = new QueryDecoder();
            var parameters = queryDecoder.GetParameters(query);
            return Url.ConvertToQuery(parameters);
        }

    }
}
