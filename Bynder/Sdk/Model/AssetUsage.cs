using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    public class AssetUsage
    {
        /// <summary>
        /// Id of the integration which defines the third party application
        /// In order to find an existing integration id visit https://help.bynder.com/integrations/asset-tracker.htm
        /// </summary>
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }

        /// <summary>
        /// Media id
        /// </summary>
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Id of the defined usage
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Timestamp of the asset usage
        /// Datetime. ISO8601 format: yyyy-mm-ddThh:mm:ssZ.
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Uri of the location
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Additional information  
        /// </summary>
        [JsonProperty("additional")]
        public string Additional { get; set; }
    }
}
