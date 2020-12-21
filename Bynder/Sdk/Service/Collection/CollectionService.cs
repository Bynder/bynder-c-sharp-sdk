// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;

namespace Bynder.Sdk.Service.Collection
{
    /// <summary>
    /// Implementation of <see cref="ICollectionService"/>
    /// </summary>
    internal class CollectionService : ICollectionService
    {
        /// <summary>
        /// Request sender to communicate with the Bynder API
        /// </summary>
        private IApiRequestSender _requestSender;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="requestSender">instance to communicate with the Bynder API</param>
        public CollectionService(IApiRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task CreateCollectionAsync(CreateCollectionQuery query)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/collections/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="id">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task DeleteCollectionAsync(string id)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/collections/{id}/",
                HTTPMethod = HttpMethod.Delete,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="id">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task<Model.Collection> GetCollectionAsync(string id)
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<Model.Collection>
            {
                Path = $"/api/v4/collections/{id}/",
                HTTPMethod = HttpMethod.Get,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task<IList<Model.Collection>> GetCollectionsAsync(GetCollectionsQuery query)
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<IList<Model.Collection>>
            {
                Path = "/api/v4/collections/",
                HTTPMethod = HttpMethod.Get,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task<IList<string>> GetMediaAsync(GetMediaQuery query)
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<IList<string>>
            {
                Path = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Get,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task AddMediaAsync(AddMediaQuery query)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task RemoveMediaAsync(RemoveMediaQuery query)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/collections/{query.CollectionId}/media/",
                HTTPMethod = HttpMethod.Delete,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="ICollectionService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="ICollectionService"/> for more information</param>
        /// <returns>Check <see cref="ICollectionService"/> for more information</returns>
        public async Task ShareCollectionAsync(ShareQuery query)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/collections/{query.CollectionId}/share/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
            }).ConfigureAwait(false);
        }
    }
}
