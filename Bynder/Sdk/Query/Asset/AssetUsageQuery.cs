using Bynder.Sdk.Query.Decoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Operation on the Asset Usage API.
    /// </summary>
    public class AssetUsageQuery
    {
        /// <summary>
        /// Initializes the class with asset usage information.
        /// </summary>
        /// <param name="integrationId">ID of the integration that the usage applies to.</param>
        /// <param name="assetId">ID of the asset that the usage applies to.</param>
        /// <param name="uri">Location of the asset usage.</param>
        /// <param name="additional">Additional information.</param>
        public AssetUsageQuery(string integrationId, string assetId, string uri = null, string additional = null)
        {
            IntegrationId = integrationId;
            AssetId = assetId;
            Uri = uri;
            Additional = additional;
        }

        /// <summary>
        /// ID of the integration that the usage applies to.
        /// </summary>
        [ApiField("integration_id")]
        public string IntegrationId { get; set; }

        /// <summary>
        /// ID of the asset that the usage applies to.
        /// </summary>
        [ApiField("asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Timestamp of the operation.
        /// </summary>
        [ApiField("timestamp")]
        public string Timestamp => DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

        /// <summary>
        /// Location of the asset usage.
        /// </summary>
        [ApiField("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Additional information.
        /// </summary>
        [ApiField("additional")]
        public string Additional { get; set; }
    }
}
