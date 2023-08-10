using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bynder.Sdk.Query.Asset
{
    public class SyncAssetUsageQuery
    {
        /// <summary>
        /// Id of the integration which defines the third party application
        /// In order to find an existing integration id visit https://help.bynder.com/integrations/asset-tracker.htm
        /// </summary>
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }

        [JsonProperty("uris")]
        public IList<string> Uris { get; set; }

        [JsonProperty("usages")]
        public IList<Usage> Usages { get; set; }
    }

    public class Usage
    {
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("additional")]
        public string Additional { get; set; }
    }
}
