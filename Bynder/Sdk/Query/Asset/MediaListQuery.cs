using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    public class MediaListQuery : MediaQuery
    {
        /// <summary>
        /// Indicating whether or not the response should include the total count of results. Example: 1. Default: 0.
        /// </summary>
        [ApiField("total")]
        public int Total { get; set; }

        /// <summary>
        /// Order of the returned list of assets
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }
    }
}
