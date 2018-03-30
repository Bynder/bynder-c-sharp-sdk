using System;
using System.Collections.Generic;
using Bynder.Api.Converters;

namespace Bynder.Api.Queries
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
        [APIField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the media
        /// </summary>
        [APIField("description")]
        public string Description { get; set; }

        /// <summary>
        /// Copyright information for the media
        /// </summary>
        [APIField("copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// Indicates if the media is archived
        /// </summary>
        [APIField("archive")]
        public bool Archive { get; set; }

        /// <summary>
        /// Indicates if the media is public
        /// </summary>
        [APIField("isPublic")]
        public bool IsPublic { get; set; }
    }
}

