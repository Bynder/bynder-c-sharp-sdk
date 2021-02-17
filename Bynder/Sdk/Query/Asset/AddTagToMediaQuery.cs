using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;
using System.Collections.Generic;

namespace Bynder.Sdk.Query.Asset
{
    public class AddTagToMediaQuery
    {
        /// <summary>
        /// Initializes the class with needed information
        /// </summary>
        /// <param name="tagId">The id of the tag on which the operation will be performed</param>
        /// <param name="mediaIds">list of asset ids to which you'd like to add the tag</param>
        public AddTagToMediaQuery(string tagId, IList<string> mediaIds)
        {
            TagId = tagId;
            MediaIds = mediaIds;
        }

        /// <summary>
        /// Id of the tag on which to perform the action
        /// </summary>
        public string TagId { get; private set; }

        /// <summary>
        /// list of asset ids to which you'd like to add the tag
        /// </summary>
        [ApiField("data", Converter = typeof(JsonConverter))]
        public IList<string> MediaIds { get; private set; }
    }
}
