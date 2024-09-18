// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
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
        /// Retrieve specific Metaproperty
        /// </summary>
        /// <param name="query">query containing the metaproperty ID</param>
        /// <returns>Structure representing the metaproperty</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Metaproperty> GetMetapropertyAsync(MetapropertyQuery query);

        /// <summary>
        /// Retrieve metaproperty dependencies
        /// </summary>
        /// <param name="query">iquery containing the metaproperty ID</param>
        /// <returns>List of the metaproperty's dependencies</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<string>> GetMetapropertyDependenciesAsync(MetapropertyQuery query);

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
        /// Uploads a file based on a filepath in the query
        /// </summary>
        /// <param name="query">Information to upload a file</param>
        /// <returns>Task representing the upload</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        /// <exception cref="BynderUploadException">Can be thrown when upload does not finish within expected time</exception>
        Task<SaveMediaResponse> UploadFileAsync(UploadQuery query);

        /// <summary>
        /// Uploads a file as a stream
        /// </summary>
        /// <param name="fileStream">Stream representing the file to be uploaded</param>
        /// <param name="query">Information to upload a file</param>
        /// <returns>Task representing the upload</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        /// <exception cref="BynderUploadException">Can be thrown when upload does not finish within expected time</exception>

        Task<SaveMediaResponse> UploadFileAsync(FileStream fileStream, UploadQuery query);


        /// <summary>
        /// Modifies a media
        /// </summary>
        /// <param name="query">Information needed to modify a media</param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> ModifyMediaAsync(ModifyMediaQuery query);

        /// <summary>
        /// Retrieve tags
        /// </summary>
        /// <param name="query">Filters for searching tags</param>
        /// <returns>Task with list of tags</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Tag>> GetTagsAsync(GetTagsQuery query);

        /// <summary>
        /// Add tag to assets
        /// </summary>
        /// <param name="query">Information about tag which will be set to media files</param>
        /// <returns>Task representing the upload</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> AddTagToMediaAsync(AddTagToMediaQuery query);

        /// <summary>
        /// Create an asset usage operation to track usage of Bynder assets in third party applications.
        /// </summary>
        /// <param name="query">Information about the asset usage</param>
        /// <returns>Task representing the operation</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> CreateAssetUsage(AssetUsageQuery query);

        /// <summary>
        /// Delete an asset usage operation to track usage of Bynder assets in third party applications.
        /// </summary>
        /// <param name="query">Information about the asset usage</param>
        /// <returns>Task representing the operation</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> DeleteAssetUsage(AssetUsageQuery query);

        /// <summary>
        /// Get a full list of Bynder assets including the total number of matching results
        /// </summary>
        /// <param name="query">Information to correctly filter/paginate media</param>
        /// <returns>Task representing the full result, including the total number of matches</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        /// <remarks>This method can be used to implement pagination in your app. The MediaFullResult that gets returned has a Total.Count property, which contains the total number of matching assets, not just the number of assets in the current result page</remarks>
        Task<MediaFullResult> GetMediaFullResultAsync(MediaQuery query);

        /// Delete an asset 
        /// </summary>
        /// <param name="assetId">Id of the asset to remove</param>
        /// <returns>Task representing the operation</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> DeleteAssetAsync(string assetId);
    }
}
