// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Service.Upload;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Upload;

namespace Bynder.Sdk.Service.Asset
{
    /// <summary>
    /// Implementation of <see cref="IAssetService"/>
    /// </summary>
    internal class AssetService : IAssetService
    {
        /// <summary>
        /// Request sender to communicate with the Bynder API
        /// </summary>
        private IApiRequestSender _requestSender;

        /// <summary>
        /// Instance to upload file to Bynder
        /// </summary>
        private FileUploader _uploader;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="requestSender">instance to communicate with the Bynder API</param>
        public AssetService(IApiRequestSender requestSender)
        {
            _requestSender = requestSender;
            _uploader = FileUploader.Create(_requestSender);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task<IList<Brand>> GetBrandsAsync()
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<IList<Brand>>
            {
                Path = "/api/v4/brands/",
                HTTPMethod = HttpMethod.Get,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task<IDictionary<string, Metaproperty>> GetMetapropertiesAsync()
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<IDictionary<string, Metaproperty>>
            {
                Path = "/api/v4/metaproperties/",
                HTTPMethod = HttpMethod.Get,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetService"/> for more information</param>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task<IList<Media>> GetMediaListAsync(MediaQuery query)
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<IList<Media>>
            {
                Path = "/api/v4/media/",
                HTTPMethod = HttpMethod.Get,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetService"/> for more information</param>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task<Uri> GetDownloadFileUrlAsync(DownloadMediaQuery query)
        {
            string path;
            if (query.MediaItemId == null)
            {
                path = $"/api/v4/media/{query.MediaId}/download/";
            }
            else
            {
                path = $"/api/v4/media/{query.MediaId}/download/{query.MediaItemId}/";
            }

            var downloadFileInformation = await _requestSender.SendRequestAsync(new ApiRequest<DownloadFileUrl>
            {
                Path = path,
                HTTPMethod = HttpMethod.Get,
            }).ConfigureAwait(false);
            return downloadFileInformation.S3File;
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetService"/> for more information</param>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task UploadFileAsync(UploadQuery query)
        {
            await _uploader.UploadFileAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetService"/> for more information</param>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task<Media> GetMediaInfoAsync(MediaInformationQuery query)
        {
            return await _requestSender.SendRequestAsync(new ApiRequest<Media>
            {
                Path = $"/api/v4/media/{query.MediaId}/",
                HTTPMethod = HttpMethod.Get,
                Query = query,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Check <see cref="IAssetService"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetService"/> for more information</param>
        /// <returns>Check <see cref="IAssetService"/> for more information</returns>
        public async Task ModifyMediaAsync(ModifyMediaQuery query)
        {
            await _requestSender.SendRequestAsync(new ApiRequest
            {
                Path = $"/api/v4/media/{query.MediaId}/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
            }).ConfigureAwait(false);
        }
    }
}
