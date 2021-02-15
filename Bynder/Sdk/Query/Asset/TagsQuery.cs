using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    public class TagsQuery
    {
        /// <summary>
        /// 
        /// </summary>
        [ApiField("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Keyword that the asset has to have to appear in the results
        /// </summary>
        [ApiField("keyword")]
        public string Keyword { get; set; }
    }
}
