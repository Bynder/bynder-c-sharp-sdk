// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Bynder.Sdk.Model
{
    /// <summary>
    /// Possible values to order assets by
    /// </summary>
    public static class AssetOrderBy
    {
        /// <summary>
        /// Oldest created assets first
        /// </summary>
        public const string DateCreatedAscending = "dateCreated asc";

        /// <summary>
        /// Newest created assets first
        /// </summary>
        public const string DateCreatedDescending = "dateCreated desc";

        /// <summary>
        /// Least recently modified assets first
        /// </summary>
        public const string DateModifiedAscending = "dateModified asc";

        /// <summary>
        /// Most recently modified assets first
        /// </summary>
        public const string DateModifiedDescending = "dateModified desc";

        /// <summary>
        /// Oldest published assets first
        /// </summary>
        public const string DatePublishedAscending = "datePublished asc";

        /// <summary>
        /// Newest published assets first
        /// </summary>
        public const string DatePublishedDescending = "datePublished desc";

        /// <summary>
        /// Assets by name from A to Z
        /// </summary>
        public const string NameAscending = "name asc";

        /// <summary>
        /// Assets by name from Z to A
        /// </summary>
        public const string NameDescending = "name desc";
    }
}
