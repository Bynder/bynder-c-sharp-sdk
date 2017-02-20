// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Models;

namespace Bynder.Api.Impl.Upload
{
    /// <summary>
    /// Class used to upload files to Bynder
    /// </summary>
    internal class FileUploader
    {
        /// <summary>
        /// Max chunk size
        /// </summary>
        private const int CHUNK_SIZE = 1024 * 1024 * 5;

        /// <summary>
        /// Max polling iterations to wait for the asset to be converted.
        /// </summary>
        private const int MAX_POLLING_ITERATIONS = 60;

        /// <summary>
        /// Iddle time between iterations
        /// </summary>
        private const int POLLING_IDDLE_TIME = 2000;

        /// <summary>
        /// Request sender used to call Bynder API.
        /// </summary>
        private readonly IOauthRequestSender _requestSender;

        /// <summary>
        /// Amazon API used to upload parts
        /// </summary>
        private readonly IAmazonApi _amazonApi;

        /// <summary>
        /// AWS bucket Url to upload chunks
        /// </summary>
        private string _awsBucket = string.Empty;

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="requestSender">Request sender to communicate with Bynder API</param>
        /// <param name="amazonApi">Amazon API to upload parts</param>
        public FileUploader(IOauthRequestSender requestSender, IAmazonApi amazonApi)
        {
            _requestSender = requestSender;
            _amazonApi = amazonApi;
        }

        /// <summary>
        /// Creates a new instance of <see cref="FileUploader"/>
        /// </summary>
        /// <param name="requestSender">Request sender to communicate with Bynder API</param>
        /// <returns>new instance</returns>
        public static FileUploader Create(IOauthRequestSender requestSender)
        {
            return new FileUploader(requestSender, new AmazonApi());
        }

        /// <summary>
        /// Uploads a file with the data specified in query parameter
        /// </summary>
        /// <param name="query">Upload query information to upload a file</param>
        /// <returns>Task representing the upload</returns>
        public async Task UploadFile(UploadQuery query)
        {
            var uploadRequest = await RequestUploadInformationAsync(new RequestUploadQuery { Filename = query.Filepath }).ConfigureAwait(false);

            uint chunkNumber = 0;

            using (var file = File.Open(query.Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                int bytesRead = 0;
                var buffer = new byte[CHUNK_SIZE];
                long numberOfChunks = (file.Length + CHUNK_SIZE - 1) / CHUNK_SIZE;

                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ++chunkNumber;
                    await UploadPartAsync(Path.GetFileName(query.Filepath), buffer, bytesRead, chunkNumber, uploadRequest, (uint)numberOfChunks).ConfigureAwait(false);
                }
            }

            var finalizeResponse = await FinalizeUploadAsync(uploadRequest, chunkNumber).ConfigureAwait(false);

            if (await HasFinishedSuccessfullyAsync(finalizeResponse).ConfigureAwait(false))
            {
                await SaveMediaAsync(new SaveMediaQuery
                {
                    Filename = query.Filepath,
                    BrandId = query.BrandId,
                    ImportId = finalizeResponse.ImportId,
                    MediaId = query.MediaId
                }).ConfigureAwait(false);
            }
            else
            {
                throw new BynderUploadException("Converter did not finished. Upload not completed");
            }
        }

        /// <summary>
        /// Gets the closes s3 endpoint. This is needed to know to which bucket Url it uploads chunks
        /// </summary>
        /// <returns>Task containting string with the Url</returns>
        private async Task<string> GetClosestS3EndpointAsync()
        {
            if (string.IsNullOrEmpty(_awsBucket))
            {
                var request = new Request<string>
                {
                    Uri = "/api/upload/endpoint",
                    HTTPMethod = HttpMethod.Get
                };

                _awsBucket = await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
            }

            return _awsBucket;
        }

        /// <summary>
        /// Saves media async. If MediaId is specified in the query a new version of the asset
        /// will be saved. Otherwise a new asset will be saved.
        /// </summary>
        /// <param name="query">Query with necessary information to save the asset</param>
        /// <returns>Task that represents the save</returns>
        private Task SaveMediaAsync(SaveMediaQuery query)
        {
            query.Filename = Path.GetFileName(query.Filename);

            string uri = null;
            if (query.MediaId == null)
            {
                uri = $"/api/v4/media/save/{query.ImportId}/";
            }
            else
            {
                uri = $"/api/v4/media/{query.MediaId}/save/{query.ImportId}/";
            }

            var request = new Request<string>
            {
                Uri = uri,
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            // No need to check response. It will only gets here if SaveMedia call gets a success code response
            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Polls status of a file using <see cref="FinalizeResponse"/>. This is used to know
        /// if Bynder already converted the file, or if it failed.
        /// </summary>
        /// <param name="finalizeResponse">finalize response information</param>
        /// <returns>Task with poll status information</returns>
        private Task<PollStatus> PollStatusAsync(FinalizeResponse finalizeResponse)
        {
            return PollStatusAsync(new PollQuery
            {
                Items = new List<string> { finalizeResponse.ImportId }
            });
        }

        /// <summary>
        /// Polls status of a file. This is used to know
        /// if Bynder already converted the file, or if it failed.
        /// </summary>
        /// <param name="query">query information</param>
        /// <returns>Task with poll status information</returns>
        private Task<PollStatus> PollStatusAsync(PollQuery query)
        {
            var request = new Request<PollStatus>
            {
                Uri = "/api/v4/upload/poll/",
                HTTPMethod = HttpMethod.Get,
                Query = query
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Uploads a part to Amazon and registers the part in Bynder.
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="buffer">content to be uploaded</param>
        /// <param name="bytesRead">number of bytes to upload</param>
        /// <param name="chunkNumber">chunk number</param>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="numberOfChunks">total number of chunks</param>
        /// <returns>Task that represent the upload part</returns>
        private async Task UploadPartAsync(string filename, byte[] buffer, int bytesRead, uint chunkNumber, UploadRequest uploadRequest, uint numberOfChunks)
        {
            var awsBucket = await GetClosestS3EndpointAsync().ConfigureAwait(false);

            await _amazonApi.UploadPartToAmazon(filename, awsBucket, uploadRequest, chunkNumber, buffer, bytesRead, numberOfChunks).ConfigureAwait(false);
            await RegisterChunkAsync(uploadRequest, chunkNumber).ConfigureAwait(false);
        }

        /// <summary>
        /// Function to check if file has finished converting within expected timeout.
        /// </summary>
        /// <param name="finalizeResponse">Finalize response information</param>
        /// <returns>Task returning true if file has finished converting successfully. False otherwise</returns>
        private async Task<bool> HasFinishedSuccessfullyAsync(FinalizeResponse finalizeResponse)
        {
            for (int iterations = MAX_POLLING_ITERATIONS; iterations > 0; --iterations)
            {
                var pollStatus = await PollStatusAsync(finalizeResponse).ConfigureAwait(false);
                if (pollStatus != null)
                {
                    if (pollStatus.ItemsDone.Contains(finalizeResponse.ImportId))
                    {
                        return true;
                    }

                    if (pollStatus.ItemsFailed.Contains(finalizeResponse.ImportId))
                    {
                        return false;
                    }
                }

                await Task.Delay(POLLING_IDDLE_TIME).ConfigureAwait(false);
            }

            return false;
        }

        /// <summary>
        /// Registers a chunk in Bynder.
        /// </summary>
        /// <param name="query">Query information to be able to register chunk</param>
        /// <returns>Task representing the register chunk process</returns>
        private Task RegisterChunkAsync(RegisterChunkQuery query)
        {
            query.S3Filename = $"{query.S3Filename}/p{query.ChunkNumber}";

            var request = new Request<string>
            {
                Uri = $"/api/v4/upload/{query.UploadId}/",
                HTTPMethod = HttpMethod.Post,
                Query = query,
                DeserializeResponse = false
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Registers a chunk in Bynder using <see cref="UploadRequest"/>.
        /// </summary>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="chunkNumber">Current chunk number</param>
        /// <returns>Task representing the register chunk process</returns>
        private Task RegisterChunkAsync(UploadRequest uploadRequest, uint chunkNumber)
        {
            return RegisterChunkAsync(new RegisterChunkQuery
            {
                TargetId = uploadRequest.S3File.TargetId,
                UploadId = uploadRequest.S3File.UploadId,
                S3Filename = uploadRequest.S3Filename,
                ChunkNumber = chunkNumber.ToString()
            });
        }

        /// <summary>
        /// Requests information to start a new upload
        /// </summary>
        /// <param name="query">Contains the information needed to request upload information</param>
        /// <returns>Task containing <see cref="UploadRequest"/> information</returns>
        private Task<UploadRequest> RequestUploadInformationAsync(RequestUploadQuery query)
        {
            var request = new Request<UploadRequest>
            {
                Uri = "/api/upload/init",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            return _requestSender.SendRequestAsync(request);
        }

        /// <summary>
        /// Finalizes an upload using <see cref="UploadRequest"/>.
        /// </summary>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="chunkNumber">chunk number</param>
        /// <returns>Task with <see cref="FinalizeResponse"/> information</returns>
        private Task<FinalizeResponse> FinalizeUploadAsync(UploadRequest uploadRequest, uint chunkNumber)
        {
            return FinalizeUploadAsync(
                new FinalizeUploadQuery
                {
                    TargetId = uploadRequest.S3File.TargetId,
                    UploadId = uploadRequest.S3File.UploadId,
                    S3Filename = uploadRequest.S3Filename,
                    Chunks = chunkNumber.ToString()
                });
        }

        /// <summary>
        /// Finalizes an upload
        /// </summary>
        /// <param name="query">Instance with necessary information to Finalize Upload</param>
        /// <returns>Task with <see cref="FinalizeResponse"/> information</returns>
        private Task<FinalizeResponse> FinalizeUploadAsync(FinalizeUploadQuery query)
        {
            query.S3Filename = $"{query.S3Filename}/p{query.Chunks}";

            var request = new Request<FinalizeResponse>
            {
                Uri = $"/api/v4/upload/{query.UploadId}/",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            return _requestSender.SendRequestAsync(request);
        }
    }
}
