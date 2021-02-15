namespace Bynder.Sdk.Query.Asset
{
    public class MetapropertiesQuery
    {
        /// <summary>
        /// Initializes the class with required information
        /// </summary>
        /// <param name="metadataId">The metadata to be modified</param>
        public MetapropertiesQuery(string metadataId)
        {
            MetadataId = metadataId;
        }

        /// <summary>
        /// Id of the media to modify
        /// </summary>
        public string MetadataId { get; private set; }
    }
}
