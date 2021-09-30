using Newtonsoft.Json;

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// The response received when uploading an asset
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// AccessRequestId
        /// </summary>
        [JsonProperty("accessRequestId")]
        public string AccessRequestId { get; set; }

        /// <summary>
        /// MediaId of the uploaded asset
        /// </summary>
        [JsonProperty("mediaid")]
        public string MediaId { get; set; }

        /// <summary>
        /// BatchId
        /// </summary>
        [JsonProperty("batchId")]
        public string BatchId { get; set; }

        /// <summary>
        /// Indicates if the upload was success
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// MediaItems that were created as part of the upload
        /// </summary>
        [JsonProperty("mediaitems")]
        public Mediaitem[] Mediaitems { get; set; }

        public class Mediaitem
        {
            [JsonProperty("original")]
            public string Original { get; set; }

            [JsonProperty("destination")]
            public string Destination { get; set; }
        }
    }
}
