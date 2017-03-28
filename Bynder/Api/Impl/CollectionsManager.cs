// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Api.Queries.Collections;
using Bynder.Models;

namespace Bynder.Api.Impl
{
    /// <summary>
    /// Implementation of <see cref="ICollectionsManager"/>
    /// </summary>
    internal class CollectionsManager : ICollectionsManager
    {
        /// <summary>
        /// Request sender to communicate with the Bynder API
        /// </summary>
        private IOauthRequestSender _requestSender;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="requestSender">instance to communicate with the Bynder API</param>
        public CollectionsManager(IOauthRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task CreateCollectionAsync(CreateCollectionQuery query)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/collections/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="id">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task DeleteCollectionAsync(string id)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/collections/{id}/",
                HTTPMethod = HttpMethod.Delete
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="id">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task<Collection> GetCollectionAsync(string id)
        {
            var request = new Request<Collection>
            {
                Uri = $"/api/v4/collections/{id}/",
                HTTPMethod = HttpMethod.Get
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task<IList<Collection>> GetCollectionsAsync(GetCollectionsQuery query)
        {
            var request = new Request<IList<Collection>>
            {
                Uri = "/api/v4/collections/",
                HTTPMethod = HttpMethod.Get,
                Query = query
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task<IList<string>> GetMediaAsync(GetMediaQuery query)
        {
            var request = new Request<IList<string>>
            {
                Uri = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Get
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task AddMediaAsync(AddMediaQuery query)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task RemoveMediaAsync(RemoveMediaQuery query)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Delete,
                Query = query
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="ICollectionsManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionsManager"/> for more information</param>
        /// <returns>Check <see cref="ICollectionsManager"/> for more information</returns>
        public Task ShareCollectionAsync(ShareQuery query)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/collections/{query.CollectionId}/share/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            return _requestSender.SendRequestAsync(request);
        }
    }
}
