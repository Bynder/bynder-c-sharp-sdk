// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model.Upload;
using Bynder.Sdk.Query.Upload;
using Bynder.Sdk.Utils;

namespace Bynder.Sdk.Service.Upload
{
    /// <summary>
    /// Class used to upload files to Bynder
    /// </summary>
    internal class FileUploader
    {
        /// <summary>
        /// Max chunk size
        /// </summary>
        private readonly int _chunkSize;

        /// <summary>
        /// Request sender used to call Bynder API.
        /// </summary>
        private readonly IApiRequestSender _requestSender;

        readonly IFileSystem _fileSystem;

        // <summary>Create FileUploader with values injected for testing.</summary>
        internal FileUploader(IApiRequestSender requestSender, IFileSystem fileSystem, int chunkSize)
        {
            _requestSender = requestSender;
            _fileSystem = fileSystem;
            _chunkSize = chunkSize;
        }

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="requestSender">Request sender to communicate with Bynder API</param>
        internal FileUploader(IApiRequestSender requestSender) : this(
            requestSender,
            fileSystem: new FileSystem(),
            chunkSize: 1024 * 1024 * 5
        )
        { }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="brandId">Brand ID to save the asset to.</param>
        /// <returns>Information about the created asset</returns>
        internal async Task<SaveMediaResponse> UploadFileToNewAssetAsync(string path, string brandId)
        {
            var fileId = await UploadFileAsync(path);
            return await SaveMediaAsync(fileId, brandId, path).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new version of an existing asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="mediaId">Asset ID for which to save the new version.</param>
        /// <returns>Information about the created asset</returns>
        internal async Task<SaveMediaResponse> UploadFileToExistingAssetAsync(string path, string mediaId)
        {
            var fileId = await UploadFileAsync(path);
            return await SaveMediaAsync(mediaId, fileId).ConfigureAwait(false);
        }

        private async Task<string> UploadFileAsync(string path)
        {
            // Prepare the upload to retrieve the file ID.
            var fileId = await PrepareAsync().ConfigureAwait(false);

            using (var fileStream = _fileSystem.File.OpenRead(path))
            using (var reader = new BinaryReader(fileStream))
            {
                int chunksUploaded = 0;

                // Upload the file divided in chunks.
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    await UploadChunkAsync(
                        fileId,
                        chunksUploaded++,
                        reader.ReadBytes(_chunkSize)
                    ).ConfigureAwait(false);
                }

                // Finalize the upload.
                await FinalizeAsync(
                    fileId,
                    chunksUploaded,
                    Path.GetFileName(path),
                    fileStream
                ).ConfigureAwait(false);
            }

            return fileId;
        }

        #region API calls

        private async Task<string> PrepareAsync()
        {
            var response = await _requestSender.SendRequestAsync(
                new ApiRequest<PrepareUploadResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = "/v7/file_cmds/upload/prepare",
                }
            ).ConfigureAwait(false);
            return response.FileId;
        }

        private async Task UploadChunkAsync(string fileId, int chunkNumber, byte[] chunk)
        {
            await _requestSender.SendRequestAsync(
                new ApiRequest
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/v7/file_cmds/upload/{fileId}/chunk/{chunkNumber}",
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-SHA256", SHA256Utils.SHA256hex(chunk) }
                    },
                    BinaryContent = chunk,
                }
            ).ConfigureAwait(false);
        }

        private async Task FinalizeAsync(string fileId, int chunksUploaded, string filename, Stream fileStream)
        {
            await _requestSender.SendRequestAsync(
                new ApiRequest
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/v7/file_cmds/upload/{fileId}/finalise_api",
                    Query = new FinalizeUploadQuery
                    {
                        ChunksCount = chunksUploaded,
                        Filename = filename,
                        FileSize = fileStream.Length,
                        SHA256 = SHA256Utils.SHA256hex(fileStream),
                    },
                }
            ).ConfigureAwait(false);
        }

        private async Task<SaveMediaResponse> SaveMediaAsync(string fileId, string brandId, string path)
        {
            return await _requestSender.SendRequestAsync(
                new ApiRequest<SaveMediaResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/api/v4/media/save/{fileId}",
                    Query = new SaveMediaQuery
                    {
                        Filename = Path.GetFileName(path),
                        BrandId = brandId,
                    },
                }
            ).ConfigureAwait(false);
        }

        private async Task<SaveMediaResponse> SaveMediaAsync(string mediaId, string fileId)
        {
            return await _requestSender.SendRequestAsync(
                new ApiRequest<SaveMediaResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/api/v4/media/{mediaId}/save/{fileId}",
                }
             ).ConfigureAwait(false);
        }

        #endregion

    }
}
