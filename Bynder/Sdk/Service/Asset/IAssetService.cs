// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Upload;

namespace Bynder.Sdk.Service.Asset
{
    /// <summary>
    /// Interface to represent operations that can be done to the Bynder Asset Bank
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        /// Gets Brands Async
        /// </summary>
        /// <returns>Task with list of brands</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Brand>> GetBrandsAsync();

        /// <summary>
        /// Gets the download file Url for specific fileItemId. If mediaItemId was not specified, 
        /// it will return the download Url of the media specified by mediaId
        /// </summary>
        /// <param name="query">information with the media we want to get the Url from</param>
        /// <returns>Task that contains the Uri of the media Item</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Uri> GetDownloadFileUrlAsync(DownloadMediaQuery query);

        /// <summary>
        /// Gets a dictionary of the metaproperties async. The key of the dictionary
        /// returned is the name of the metaproperty.
        /// </summary>
        /// <returns>Task with dictionary of metaproperties</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IDictionary<string, Metaproperty>> GetMetapropertiesAsync();

        /// <summary>
        /// Gets all the information for a specific mediaId. This is needed 
        /// to get the media items of a media.
        /// </summary>
        /// <param name="query">Information about the media we want to get the information of.</param>
        /// <returns>Task with the Media information</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Media> GetMediaInfoAsync(MediaInformationQuery query);

        /// <summary>
        /// Gets a list of media using query information. The media information is not complete, for example
        /// media items for media returned are not present. For that client needs to call <see cref="RequestMediaInfoAsync(string)"/>
        /// </summary>
        /// <param name="query">information to correctly filter/paginate media</param>
        /// <returns>Task with List of media.</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Media>> GetMediaListAsync(MediaQuery query);

        /// <summary>
        /// Modifies a media
        /// </summary>
        /// <param name="query">Information needed to modify a media</param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task ModifyMediaAsync(ModifyMediaQuery query);
    }
}
