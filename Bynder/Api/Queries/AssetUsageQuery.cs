namespace Bynder.Api.Queries
{
    public class AssetUsageQuery
    {
        /// <summary>
        /// Id of the integration which defines the third party application
        /// In order to find an existing integration id visit https://help.bynder.com/integrations/asset-tracker.htm
        /// </summary>
        [APIField("integration_id")]
        public string IntegrationId { get; set; }

        /// <summary>
        /// Media id
        /// </summary>
        [APIField("asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Uri of the location
        /// </summary>
        [APIField("uri")]
        public string Uri { get; set; }
    }
}