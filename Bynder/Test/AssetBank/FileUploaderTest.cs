// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Impl.Upload;
using Bynder.Api.Queries;
using Bynder.Models;
using Moq;
using NUnit.Framework;

namespace Bynder.Test.AssetBank
{
    /// <summary>
    /// File to test <seealso cref="FileUploader"/> implementation
    /// </summary>
    [TestFixture]
    public class FileUploaderTest
    {
        /// <summary>
        /// Import id to return from finalize upload and poll status
        /// </summary>
        private const string ImportId = "ImportId";

        /// <summary>
        /// Tests that correct sequence is called when <seealso cref="FileUploader.UploadFile(UploadQuery)"/>
        /// The order it tests is:
        /// 1. Init upload
        /// 2. Get closest s3 endpoint
        /// 3. Upload part to Amazon
        /// 4. Register chunk in Bynder
        /// 5. Finalize upload.
        /// 6. Poll status
        /// 7. Save
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenFileUploadedThenUploadSequenceIsCalled()
        {
            var mock = new Mock<IOauthRequestSender>(MockBehavior.Strict);
            var awsmock = new Mock<IAmazonApi>(MockBehavior.Strict);
            var sequence = new MockSequence();

            var filename = Path.GetTempFileName();
            var uploadRequest = GetUploadRequest();
            var finalizeResponse = GetFinalizeResponse();

            // Init upload
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetValidUploadRequest(filename)))
                .Returns(Task.FromResult(uploadRequest));

            // Get Closest s3 endpoint to upload parts
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetValidClosestS3EndpointRequest()))
                .Returns(Task.FromResult("\"http://test.amazon.com/\""));

            // Upload Part to Amazon
            awsmock.InSequence(sequence).Setup(amazonApiMock => amazonApiMock.UploadPartToAmazon(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<UploadRequest>(),
                It.IsAny<uint>(),
                It.IsAny<byte[]>(),
                It.IsAny<int>(),
                It.IsAny<uint>())).Returns(Task.FromResult(true));

            // Register chunk in bynder
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetValidRegisterChunkRequest(uploadRequest)))
                .Returns(Task.FromResult(string.Empty));

            // Finalizes the upload
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetValidFinalizeResponseRequest(uploadRequest)))
                .Returns(Task.FromResult(finalizeResponse));

            // Poll status 
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetValidPollStatus(finalizeResponse)))
                .Returns(Task.FromResult(GetPollStatus()));

            // Saves media
            mock.InSequence(sequence).Setup(reqSenderMock => reqSenderMock.SendRequestAsync(GetSaveMediaRequest(filename, finalizeResponse)))
                .Returns(Task.FromResult(string.Empty));

            FileUploader uploader = new FileUploader(mock.Object, awsmock.Object);
            using (var fileStream = File.OpenWrite(filename))
            {
                var bytes = Encoding.UTF8.GetBytes("mockdata");
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }

            try
            {
                await uploader.UploadFile(new UploadQuery
                {
                    Filepath = filename
                });
            }
            finally
            {
                File.Delete(filename);
            }
        }

        /// <summary>
        /// Returns Request that has valid values for upload request 
        /// </summary>
        /// <param name="filename">filename of the file we want to upload</param>
        /// <returns>request to initialize upload</returns>
        private Request<UploadRequest> GetValidUploadRequest(string filename)
        {
            return It.Is<Request<UploadRequest>>(req =>
                req.Uri == "/api/upload/init"
                && req.HTTPMethod == HttpMethod.Post
                && ((RequestUploadQuery)req.Query).Filename == filename);
        }

        /// <summary>
        ///  Returns Request that has valid values to get Closest S3 enpdoint 
        /// </summary>
        /// <returns>request to get closest endpoint</returns>
        private Request<string> GetValidClosestS3EndpointRequest()
        {
            return It.Is<Request<string>>(req =>
                req.Uri == "/api/upload/endpoint"
                && req.HTTPMethod == HttpMethod.Get
                && req.Query == null);
        }

        /// <summary>
        /// Returns Request that has valid values to register chunk
        /// </summary>
        /// <param name="uploadRequest">upload request. Needed to validate request values</param>
        /// <returns>request to register chunk</returns>
        private Request<string> GetValidRegisterChunkRequest(UploadRequest uploadRequest)
        {
            return It.Is<Request<string>>(req =>
                IsRegisterChunkValid(req, uploadRequest));
        }

        /// <summary>
        /// Returns Request that has valid values to finalize response
        /// </summary>
        /// <param name="uploadRequest">upload request. Needed to validate request values</param>
        /// <returns>request to finalize upload</returns>
        private Request<FinalizeResponse> GetValidFinalizeResponseRequest(UploadRequest uploadRequest)
        {
            return It.Is<Request<FinalizeResponse>>(req => IsFinalizeResponseValid(req, uploadRequest));
        }

        /// <summary>
        /// Returns Request that has valid values to poll status
        /// </summary>
        /// <param name="finalizeResponse">Finalize response. Needed to validate request values</param>
        /// <returns>Request to poll status</returns>
        private Request<PollStatus> GetValidPollStatus(FinalizeResponse finalizeResponse)
        {
            return It.Is<Request<PollStatus>>(req =>
                req.Uri == "/api/v4/upload/poll/"
                && req.HTTPMethod == HttpMethod.Get
                && ((PollQuery)req.Query).Items.Contains(finalizeResponse.ImportId));
        }

        /// <summary>
        /// Returns Request that has valid values to save media
        /// </summary>
        /// <param name="filename">filename of the media. Needed to validate request values</param>
        /// <param name="finalizeResponse">finalize response. Needed to validate request values</param>
        /// <returns>Request to save media</returns>
        private Request<string> GetSaveMediaRequest(string filename, FinalizeResponse finalizeResponse)
        {
            return It.Is<Request<string>>(req =>
                req.Uri == $"/api/v4/media/save/{finalizeResponse.ImportId}/"
                && req.HTTPMethod == HttpMethod.Post
                && ((SaveMediaQuery)req.Query).Filename == Path.GetFileName(filename));
        }

        /// <summary>
        /// Helper function to check if a request is valid to call register chunk.
        /// </summary>
        /// <param name="request">request to validate</param>
        /// <param name="uploadRequest">upload request</param>
        /// <returns>true if request is valid</returns>
        private bool IsRegisterChunkValid(Request<string> request, UploadRequest uploadRequest)
        {
            var registerChunkQuery = (RegisterChunkQuery)request.Query;

            return request.Uri == $"/api/v4/upload/{uploadRequest.S3File.UploadId}/"
                && request.HTTPMethod == HttpMethod.Post
                && registerChunkQuery.ChunkNumber == "1"
                && registerChunkQuery.TargetId == uploadRequest.S3File.TargetId
                && registerChunkQuery.S3Filename == $"{uploadRequest.S3Filename}/p1";
        }

        /// <summary>
        /// Helper function to check if a request is valid to call finalize upload.
        /// </summary>
        /// <param name="request">request to validate</param>
        /// <param name="uploadRequest">upload request</param>
        /// <returns>true if request is valid</returns>
        private bool IsFinalizeResponseValid(Request<FinalizeResponse> request, UploadRequest uploadRequest)
        {
            var registerChunkQuery = (FinalizeUploadQuery)request.Query;

            return request.Uri == $"/api/v4/upload/{uploadRequest.S3File.UploadId}/"
                && request.HTTPMethod == HttpMethod.Post
                && registerChunkQuery.Chunks == "1"
                && registerChunkQuery.TargetId == uploadRequest.S3File.TargetId
                && registerChunkQuery.S3Filename == $"{uploadRequest.S3Filename}/p1";
        }

        /// <summary>
        /// Returns Stub date to return when Request Upload is called
        /// </summary>
        /// <returns>Returns stub instance</returns>
        private UploadRequest GetUploadRequest()
        {
            return new UploadRequest
            {
                S3File = new S3File
                {
                    TargetId = "targetid",
                    UploadId = "uploadid"
                },
                S3Filename = "filename"
            };
        }

        /// <summary>
        /// Returns Stub finalize response to return when Finalize upload is called
        /// </summary>
        /// <returns>Returns stub instance</returns>
        private FinalizeResponse GetFinalizeResponse()
        {
            return new FinalizeResponse
            {
                ImportId = ImportId
            };
        }

        /// <summary>
        /// Returns Stub finalize response to return when Poll status is called
        /// </summary>
        /// <returns>Returns stub instance</returns>
        private PollStatus GetPollStatus()
        {
            return new PollStatus
            {
                ItemsDone = new HashSet<string> { ImportId }
            };
        }
    }
}
