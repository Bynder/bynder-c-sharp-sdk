// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Service.Collection
{
    /// <summary>
    /// Interface to represent operations that can be done to the Bynder Media Collections
    /// </summary>
    public interface ICollectionService
    {
        /// <summary>
        /// Gets Collections Async
        /// </summary>
        /// <param name="query">information to correctly filter/paginate</param>
        /// <returns>Task with a list of <see cref="Collection"/> items</returns>
        Task<IList<Model.Collection>> GetCollectionsAsync(GetCollectionsQuery query);

        /// <summary>
        /// Gets a specific colection
        /// </summary>
        /// <param name="id">The uuid of the specific <see cref="Collection"/></param>
        /// <returns>Task that contains the specific <see cref="Collection"/></returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Model.Collection> GetCollectionAsync(string id);

        /// <summary>
        /// Creates a new Collection
        /// </summary>
        /// <param name="query">information of the collection to be created</param>
        /// <returns>Task with the Uri of the created Collection</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task CreateCollectionAsync(CreateCollectionQuery query);

        /// <summary>
        /// Deletes a specific colection
        /// </summary>
        /// <param name="id">The uuid of the specific <see cref="Collection"/></param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task DeleteCollectionAsync(string id);

        /// <summary>
        /// Gets Media of Collection Async
        /// </summary>
        /// <param name="query">information needed to retrieve Media of a Collection</param>
        /// <returns>Task with a list of <see cref="Collection"/> items</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<string>> GetMediaAsync(GetMediaQuery query);

        /// <summary>
        /// Adds Media to Collection
        /// </summary>
        /// <param name="query">information needed to add media to a Collection</param>
        /// <returns>Task with the Uri of the created Collection</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task AddMediaAsync(AddMediaQuery query);

        /// <summary>
        /// Removes Media from Colection
        /// </summary>
        /// <param name="query">information needed in order to remove media from a Collection</param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task RemoveMediaAsync(RemoveMediaQuery query);

        /// <summary>
        /// Shares a Colection
        /// </summary>
        /// <param name="query">information required for sharing a collection</param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task ShareCollectionAsync(ShareQuery query);
    }
}
