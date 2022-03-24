using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    public class MetapropertyOptionSearchQuery
    {
        /// <summary>
        /// Id of the media
        /// </summary>
        [ApiField("id")]
        public string MetapropertyId { get; set; }

        /// <summary>
        /// Name of the option
        /// </summary>
        [ApiField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Maximum number of results
        /// </summary>
        [ApiField("limit")]
        public int Limit { get; } = 50;

        /// <summary>
        /// Offset page for results: return the N-th set of limit-results
        /// </summary>
        [ApiField("page")]
        public int Page { get; } = 1;
    }
}
