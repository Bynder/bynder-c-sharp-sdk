// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Converters;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Impl.Upload;
using Bynder.Api.Queries;
using Bynder.Models;

namespace Bynder.Api.Impl
{
    /// <summary>
    /// Implementation of <see cref="IAssetBankManager"/>
    /// </summary>
    internal class AssetBankManager : IAssetBankManager
    {
        /// <summary>
        /// Request sender to communicate with the Bynder API
        /// </summary>
        private IOauthRequestSender _requestSender;

        /// <summary>
        /// Instance to upload file to Bynder
        /// </summary>
        private FileUploader _uploader;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="requestSender">instance to communicate with the Bynder API</param>
        public AssetBankManager(IOauthRequestSender requestSender)
        {
            _requestSender = requestSender;
            _uploader = FileUploader.Create(_requestSender);
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task<IList<Brand>> GetBrandsAsync()
        {
            var request = new Request<IList<Brand>>
            {
                Uri = "/api/v4/brands/",
                HTTPMethod = HttpMethod.Get
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task<IDictionary<string, Metaproperty>> GetMetapropertiesAsync()
        {
            var request = new Request<IDictionary<string, Metaproperty>>
            {
                Uri = "/api/v4/metaproperties/",
                HTTPMethod = HttpMethod.Get
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetBankManager"/> for more information</param>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task<IList<Media>> RequestMediaListAsync(MediaQuery query)
        {
            var request = new Request<IList<Media>>
            {
                Uri = "/api/v4/media/",
                HTTPMethod = HttpMethod.Get,
                Query = query
            };

            return _requestSender.SendRequestAsync(request, new MediaConverter());
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetBankManager"/> for more information</param>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public async Task<Uri> GetDownloadFileUrlAsync(DownloadMediaQuery query)
        {
            string uri = string.Empty;
            if (query.MediaItemId == null)
            {
                uri = $"/api/v4/media/{query.MediaId}/download/";
            }
            else
            {
                uri = $"/api/v4/media/{query.MediaId}/download/{query.MediaItemId}/";
            }

            var request = new Request<DownloadFileUrl>
            {
                Uri = uri,
                HTTPMethod = HttpMethod.Get
            };

            var downloadFileInformation = await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
            return downloadFileInformation.S3File;
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetBankManager"/> for more information</param>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task UploadFileAsync(UploadQuery query)
        {
            return _uploader.UploadFile(query);
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetBankManager"/> for more information</param>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task<Media> RequestMediaInfoAsync(MediaInformationQuery query)
        {
            var request = new Request<Media>
            {
                Uri = $"/api/v4/media/{query.MediaId}/",
                HTTPMethod = HttpMethod.Get,
                Query = query
            };

            return _requestSender.SendRequestAsync(request, new MediaConverter());
        }

        /// <summary>
        /// Check <see cref="IAssetBankManager"/> for more information
        /// </summary>
        /// <param name="query">Check <see cref="IAssetBankManager"/> for more information</param>
        /// <returns>Check <see cref="IAssetBankManager"/> for more information</returns>
        public Task ModifyMediaAsync(ModifyMediaQuery query)
        {
            var request = new Request<string>
            {
                Uri = $"/api/v4/media/{query.MediaId}/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            return _requestSender.SendRequestAsync(request);
        }
    }
}
