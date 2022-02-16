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
        /// <para>Desired order of returned assets.</para>
        /// <para>See <see cref="Model.AssetOrderBy"/> for possible values.</para>
        /// </summary>
        [ApiField("orderBy")]
        public string OrderBy { get; set; }
    }
}
