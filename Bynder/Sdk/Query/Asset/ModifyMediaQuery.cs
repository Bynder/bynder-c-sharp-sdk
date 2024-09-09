using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to modify a media with 
    /// </summary>
    public class ModifyMediaQuery
    {
        /// <summary>
        /// Initializes the class with required information
        /// </summary>
        /// <param name="mediaId">The media to be modified</param>
        public ModifyMediaQuery(string mediaId)
        {
            MediaId = mediaId;
        }

        /// <summary>
        /// Id of the media to modify
        /// </summary>
        public string MediaId { get; private set; }

        /// <summary>
        /// Name of the media
        /// </summary>
        [ApiField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the media
        /// </summary>
        [ApiField("description")]
        public string Description { get; set; }

        /// <summary>
        /// Copyright information for the media
        /// </summary>
        [ApiField("copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// Published date for the media
        /// </summary>
        [ApiField("datePublished")]
        public string PublishedDate { get; set; }

        /// <summary>
        /// Indicates if the media is archived
        /// </summary>
        [ApiField("archive")]
        public bool? Archive { get; set; }

        /// <summary>
        /// Indicates if the media is public
        /// </summary>
        [ApiField("isPublic")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// Metaproperty options to set on the asset.
        /// </summary>
        [ApiField("metaproperty", Converter = typeof(MetapropertyOptionsConverter))]
        public IDictionary<string, IList<string>> MetapropertyOptions { get; set; }

        /// <summary>
        /// Add a set of options to a metaproperty
        /// </summary>
        /// <param name="metapropertyId">metaproperty ID</param>
        /// <param name="optionIds">set of options</param>
        public void AddMetapropertyOptions(string metapropertyId, IList<string> optionIds)
        {
            MetapropertyOptions.Add(metapropertyId, optionIds);
        }
    }
}

