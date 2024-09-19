using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Decoder;
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

        private string GetQueryParameters(ModifyMediaQuery query)
        {
            QueryDecoder queryDecoder = new QueryDecoder();
            var parameters = queryDecoder.GetParameters(query);
            return Url.ConvertToQuery(parameters);
        }

    }
}
