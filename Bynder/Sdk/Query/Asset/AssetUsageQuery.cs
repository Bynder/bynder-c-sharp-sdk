using Bynder.Sdk.Api.Converters;
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
        public AssetUsageQuery(string integrationId, string assetId)
        {
            IntegrationId = integrationId;
            AssetId = assetId;
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
        [ApiField("timestamp", Converter = typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

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
