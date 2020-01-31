namespace Bynder.Api.Queries
{
    public class CreateAssetUsageQuery: AssetUsageQuery
    {
        /// <summary>
        /// Timestamp of the asset usage
        /// Datetime. ISO8601 format: yyyy-mm-ddThh:mm:ssZ.
        /// </summary>
        [APIField("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Additional information  
        /// </summary>
        [APIField("additional")]
        public string Additional { get; set; }
    }
}