using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;
using System.Collections.Generic;

namespace Bynder.Sdk.Query.Asset
{
    public class AddTagToAssetsQuery
    {
        /// <summary>
        /// Initializes the class with needed information
        /// </summary>
        /// <param name="collectionId">The id of the tag on which the operation will be performed</param>
        /// <param name="mediaIds">List with the Ids of the media</param>
        public AddTagToAssetsQuery(string tagId, IList<string> mediaIds)
        {
            TagId = tagId;
            MediaIds = mediaIds;
        }

        /// <summary>
        /// Id of the tag on which to perform the action
        /// </summary>
        public string TagId { get; private set; }

        /// <summary>
        /// List with the Ids of the media
        /// </summary>
        [ApiField("data", Converter = typeof(JsonConverter))]
        public IList<string> MediaIds { get; private set; }
    }
}
