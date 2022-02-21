using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    public class MediaWithTotalQuery : MediaQuery
    {
        /// <summary>
        /// Indicating whether or not the response should include the total count of results.
        /// </summary>
        [ApiField("total", Converter = typeof(BoolConverter))]
        public bool IncludeTotal { get; set; }

        /// <summary>
        /// <para>Desired order of returned assets.</para>
        /// <para>See <see cref="Model.AssetOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }
    }
}
