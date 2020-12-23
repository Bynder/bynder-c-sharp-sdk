using Newtonsoft.Json;

namespace Bynder.Sdk.Model.Upload
{
    /// <summary>
    /// Represents the response body of the "save media" request
    /// in <see cref="FileUploader"/>.
    /// </summary>
    public class SaveMediaResponse
    {
        [JsonProperty("accessRequestId")]
        public string AccessRequestId { get; set; }

        [JsonProperty("mediaid")]
        public string MediaId { get; set; }

        [JsonProperty("batchId")]
        public string BatchId { get; set; }

        [JsonProperty("success")]
        public bool IsSuccessful { get; set; }
    }
}
