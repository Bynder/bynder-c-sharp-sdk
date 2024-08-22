using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    public class GetTagsQuerySimple
    {
        /// <summary>
        /// Maximum number of results.
        /// </summary>
        [ApiField("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Offset page for results: return the N-th set of limit-results.
        /// </summary>
        [ApiField("page")]
        public int Page { get; set; }

        /// <summary>
        /// <para>Order of the returned list of tags. </para>
        /// <para>See <see cref="TagsOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy", Converter = typeof(TagsOrderByConverter))]
        public TagsOrderBy OrderBy { get; set; }

        /// <summary>
        /// Search on matching names.
        /// </summary>
        [ApiField("keyword")]
        public string Keyword { get; set; }
    }
    public class GetTagsQuery : GetTagsQuerySimple
    {

        /// <summary>
        /// Minimum media count that the returned tags should have.
        /// </summary>
        [ApiField("mincount")]
        public int MinCount { get; set; }

    }
}
