// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Bynder.Api.Converters;
using Bynder.Models;

namespace Bynder.Api.Queries.Collections
{
    /// <summary>
    /// Query to remove media from a collection
    /// </summary>
    public class RemoveMediaQuery
    {
        /// <summary>
        /// Initializes the class with needed information
        /// </summary>
        /// <param name="collectionId">The id of the collection on which an operation will be performed</param>
        /// <param name="mediaIds">List with the Ids of the media</param>
        public RemoveMediaQuery(string collectionId, IList<string> mediaIds)
        {
            CollectionId = collectionId;
            MediaIds = mediaIds;
        }

        /// <summary>
        /// Id of the collection on which to perform the action
        /// </summary>
        public string CollectionId { get; private set; }

        /// <summary>
        /// List with the Ids of the media
        /// </summary>
        [APIField("deleteIds", Converter = typeof(ListConverter))]
        public IList<string> MediaIds { get; private set; }
    }
}
