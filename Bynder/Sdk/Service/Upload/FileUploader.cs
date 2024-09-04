// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Exceptions;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Upload;
using System.Linq;
using System.Web;

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
        private readonly IApiRequestSender _requestSender;

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
        public FileUploader(IApiRequestSender requestSender, IAmazonApi amazonApi)
        {
            _requestSender = requestSender;
            _amazonApi = amazonApi;
        }

        /// <summary>
        /// Creates a new instance of <see cref="FileUploader"/>
        /// </summary>
        /// <param name="requestSender">Request sender to communicate with Bynder API</param>
        /// <returns>new instance</returns>
        public static FileUploader Create(IApiRequestSender requestSender)
        {
            return new FileUploader(requestSender, new AmazonApi());
        }

        /// <summary>
        /// Uploads a file with the data specified in query parameter
        /// </summary>
        /// <param name="fileStream">Stream of the file to upload</param>
        /// <param name="query">Upload query information to upload a file</param>
        /// <returns>Task representing the upload</returns>
        public async Task<SaveMediaResponse> UploadFileAsync(Stream fileStream, UploadQuery query)
        {
            return await UploadFileAsync(fileStream, query, query.OriginalFileName ?? Path.GetFileName(query.Filepath));
        }

        /// <summary>
        /// Uploads a file with the data specified in query parameter
        /// </summary>
        /// <param name="query">Upload query information to upload a file</param>
        /// <returns>Task representing the upload</returns>
        public async Task<SaveMediaResponse> UploadFileAsync(UploadQuery query)
        {
            var filename = !string.IsNullOrEmpty(query.OriginalFileName) ? query.OriginalFileName : Path.GetFileName(query.Filepath);
            var uploadRequest = await RequestUploadInformationAsync(new RequestUploadQuery { Filename = filename }).ConfigureAwait(false);

            uint chunkNumber = 0;

            var fileStream = File.Open(query.Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            return await UploadFileAsync(fileStream, query, filename);

            
        }

        private async Task<UploadRequest> GetUploadRequest(string fileName)
        {
            return await RequestUploadInformationAsync(new RequestUploadQuery { Filename = fileName }).ConfigureAwait(false);
        }


        private async Task<SaveMediaResponse> UploadFileAsync(Stream fileStream, UploadQuery query, string filename)
        {
            uint chunkNumber = 0;
            var uploadRequest = await GetUploadRequest(filename);
            using (fileStream)
            {
                int bytesRead = 0;
                var buffer = new byte[CHUNK_SIZE];
                long numberOfChunks = (fileStream.Length + CHUNK_SIZE - 1) / CHUNK_SIZE;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ++chunkNumber;
                    await UploadPartAsync(Path.GetFileName(query.Filepath), buffer, bytesRead, chunkNumber, uploadRequest, (uint)numberOfChunks).ConfigureAwait(false);
                }
            }

            var finalizeResponse = await FinalizeUploadAsync(uploadRequest, chunkNumber, query.CustomParameters).ConfigureAwait(false);

            if (await HasFinishedSuccessfullyAsync(finalizeResponse).ConfigureAwait(false))
            {
                return await SaveMediaAsync(new SaveMediaQuery
                {
                    Filename = query.Name ?? query.Filepath,
                    BrandId = query.BrandId,
                    ImportId = finalizeResponse.ImportId,
                    MediaId = query.MediaId,
                    Tags = query.Tags,
                    Description = query.Description,
                    Copyright = query.Copyright,
                    IsPublic = query.IsPublic,
                    MetapropertyOptions = query.MetapropertyOptions,
                    PublishedDate = query.PublishedDate
                }).ConfigureAwait(false);
            }
            else
            {
                throw new BynderUploadException("Converter did not finish. Upload not completed");
            }
        }

        /// <summary>
        /// Gets the closest s3 endpoint. This is needed to know to which bucket Url it uploads chunks
        /// </summary>
        /// <returns>Task containting string with the Url</returns>
        private async Task<string> GetClosestS3EndpointAsync()
        {
            if (string.IsNullOrEmpty(_awsBucket))
            {
                var request = new ApiRequest<string>
                {
                    Path = "/api/upload/endpoint",
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
        private async Task<SaveMediaResponse> SaveMediaAsync(SaveMediaQuery query)
        {
            query.Filename = Path.GetFileName(query.Filename);

            string path = null;
            if (query.MediaId == null)
            {
                path = $"/api/v4/media/save/{query.ImportId}/";
            }
            else
            {
                path = $"/api/v4/media/{query.MediaId}/save/{query.ImportId}/";
            }

            var request = new ApiRequest<SaveMediaResponse>
            {
                Path = path,
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            // No need to check response. It will only gets here if SaveMedia call gets a success code response
            return await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Polls status of a file using <see cref="FinalizeResponse"/>. This is used to know
        /// if Bynder already converted the file, or if it failed.
        /// </summary>
        /// <param name="finalizeResponse">finalize response information</param>
        /// <returns>Task with poll status information</returns>
        private async Task<PollStatus> PollStatusAsync(FinalizeResponse finalizeResponse)
        {
            return await PollStatusAsync(new PollQuery
            {
                Items = new List<string> { finalizeResponse.ImportId }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Polls status of a file. This is used to know
        /// if Bynder already converted the file, or if it failed.
        /// </summary>
        /// <param name="query">query information</param>
        /// <returns>Task with poll status information</returns>
        private async Task<PollStatus> PollStatusAsync(PollQuery query)
        {
            var request = new ApiRequest<PollStatus>
            {
                Path = "/api/v4/upload/poll/",
                HTTPMethod = HttpMethod.Get,
                Query = query
            };

            return await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
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
        /// Registers a chunk in Bynder using <see cref="GetUploadRequest"/>.
        /// </summary>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="chunkNumber">Current chunk number</param>
        /// <returns>Task representing the register chunk process</returns>
        private async Task RegisterChunkAsync(UploadRequest uploadRequest, uint chunkNumber)
        {
            var query = new RegisterChunkQuery
            {
                TargetId = uploadRequest.S3File.TargetId,
                UploadId = uploadRequest.S3File.UploadId,
                S3Filename = uploadRequest.S3Filename,
                ChunkNumber = chunkNumber.ToString()
            };

            query.S3Filename = $"{query.S3Filename}/p{query.ChunkNumber}";

            var request = new ApiRequest
            {
                Path = $"/api/v4/upload/{query.UploadId}/",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Requests information to start a new upload
        /// </summary>
        /// <param name="query">Contains the information needed to request upload information</param>
        /// <returns>Task containing <see cref="GetUploadRequest"/> information</returns>
        private async Task<UploadRequest> RequestUploadInformationAsync(RequestUploadQuery query)
        {
            var request = new ApiRequest<UploadRequest>
            {
                Path = "/api/upload/init",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };

            return await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Finalizes an upload using <see cref="GetUploadRequest"/>.
        /// </summary>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="chunkNumber">chunk number</param>
        /// <returns>Task with <see cref="FinalizeResponse"/> information</returns>
        private async Task<FinalizeResponse> FinalizeUploadAsync(UploadRequest uploadRequest, uint chunkNumber, IEnumerable<KeyValuePair<string, string>> customParameters)
        {
            var query = new FinalizeUploadQuery
            {
                TargetId = uploadRequest.S3File.TargetId,
                UploadId = uploadRequest.S3File.UploadId,
                S3Filename = $"{uploadRequest.S3Filename}/p{chunkNumber}",
                Chunks = chunkNumber.ToString()
            };
            var requestParameters = "";
            if (customParameters != null)
            {
                requestParameters = string.Join('&', customParameters.Select(p => HttpUtility.UrlEncode(p.Key) + "=" + HttpUtility.UrlEncode(p.Value)));
                if (!string.IsNullOrEmpty(requestParameters))
                {
                    requestParameters = "?" + requestParameters;
                }
            }
            var request = new ApiRequest<FinalizeResponse>
            {
                Path = $"/api/v4/upload/{query.UploadId}/{requestParameters}",
                HTTPMethod = HttpMethod.Post,
                Query = query
            };
            return await _requestSender.SendRequestAsync(request).ConfigureAwait(false);
        }
    }
}
