using Newtonsoft.Json;

namespace Bynder.Sdk.Model.Upload
{
    /// <summary>
    /// Represents the response body of the "prepare upload" request
    /// in <see cref="FileUploader"/>.
    /// </summary>
    public class PrepareUploadResponse
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }
}
