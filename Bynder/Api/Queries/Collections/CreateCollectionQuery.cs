// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Api.Queries.Collections
{
    /// <summary>
    /// Query to create collections
    /// </summary>
    public class CreateCollectionQuery
    {
        /// <summary>
        /// Initializes the class with needed information
        /// </summary>
        /// <param name="name">Name of collection</param>
        public CreateCollectionQuery(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of collection
        /// </summary>
        [APIField("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Description of collection
        /// </summary>
        [APIField("description")]
        public string Description { get; set; }
    }
}
