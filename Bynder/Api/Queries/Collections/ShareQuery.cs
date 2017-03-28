// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Api.Converters;
using Bynder.Api.Queries.Collections.PermissionOptions;

namespace Bynder.Api.Queries.Collections
{
    /// <summary>
    /// Query to share a collection with 
    /// </summary>
    public class ShareQuery
    {
        /// <summary>
        /// Initializes the class with required information
        /// </summary>
        /// <param name="collectionId">The collection to be shared</param>
        /// <param name="recipients">Email addresses of the people to share the collection with</param>
        /// <param name="permission">permission rights of the recipients</param>
        public ShareQuery(string collectionId, IList<string> recipients, SharingPermssion permission)
        {
            CollectionId = collectionId;
            Recipients = recipients;
            Permission = permission;
        }

        /// <summary>
        /// Id of the collection to share
        /// </summary>
        public string CollectionId { get; private set; }

        /// <summary>
        /// Email addresses of the 
        /// </summary>
        [APIField("recipients", Converter = typeof(ListConverter))]
        public IList<string> Recipients { get; private set; }

        /// <summary>
        /// Permission rights of the recipients
        /// </summary>
        [APIField("collectionOptions", Converter = typeof(LowerCaseEnumConverter))]
        public SharingPermssion Permission { get; private set; }

        /// <summary>
        /// Flags if the recipients should login to view the collection
        /// </summary>
        [APIField("loginRequired")]
        public bool LoginRequired { get; set; }

        /// <summary>
        /// Sharing start date
        /// </summary>
        [APIField("dateStart", Converter = typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? DateStart { get; set; }

        /// <summary>
        /// Sharing end date
        /// </summary>
        [APIField("dateEnd", Converter = typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? DateEnd { get; set; }

        /// <summary>
        /// Flags if the recipients should recieve an email
        /// </summary>
        [APIField("sendMail")]
        public bool SendMail { get; set; }

        /// <summary>
        /// Message to include in the email that will be sent
        /// </summary>
        [APIField("message")]
        public string Message { get; set; }
    }
}
