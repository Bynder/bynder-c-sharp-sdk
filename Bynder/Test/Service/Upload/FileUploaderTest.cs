// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Upload;
using Bynder.Sdk.Service.Upload;
using Moq;
using Xunit;

namespace Bynder.Test.Service.Upload
{
    public class FileUploaderTest
    {
        private const int ChunkSize = 1024 * 1024 * 5;
        private const int MaxBytesPerRead = 1000;
        private const string ImportId = "importId";

        private readonly Mock<IApiRequestSender> _apiRequestSenderMock;
        private readonly Mock<IAmazonApi> _amazonApiMock;
        private readonly FileUploader _fileUploader;
        private readonly List<UploadedPart> _uploadedParts = [];
        private readonly List<Request<Status>> _registerChunkRequests = [];
        private Request<FinalizeResponse> _finalizeRequest;
        private Request<SaveMediaResponse> _saveMediaRequest;

        public FileUploaderTest()
        {
            _apiRequestSenderMock = new Mock<IApiRequestSender>();
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<UploadRequest>>()))
                .ReturnsAsync(new UploadRequest
                {
                    S3Filename = "s3Filename",
                    S3File = new S3File { UploadId = "uploadId", TargetId = "targetId" },
                });
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
                .ReturnsAsync("https://bucket.example.com/");
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<Status>>()))
                .Callback<Request<Status>>(request => _registerChunkRequests.Add(request))
                .ReturnsAsync(new Status());
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<FinalizeResponse>>()))
                .Callback<Request<FinalizeResponse>>(request => _finalizeRequest = request)
                .ReturnsAsync(new FinalizeResponse { ImportId = ImportId });
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<PollStatus>>()))
                .ReturnsAsync(new PollStatus { ItemsDone = [ImportId], ItemsFailed = [] });
            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.IsAny<Request<SaveMediaResponse>>()))
                .Callback<Request<SaveMediaResponse>>(request => _saveMediaRequest = request)
                .ReturnsAsync(new SaveMediaResponse());

            _amazonApiMock = new Mock<IAmazonApi>();
            _amazonApiMock
                .Setup(amazonApi => amazonApi.UploadPartToAmazon(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UploadRequest>(), It.IsAny<uint>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<uint>()))
                .Callback<string, string, UploadRequest, uint, byte[], int, uint>((filename, awsBucket, uploadRequest, chunkNumber, fileContent, numberOfBytes, numberOfChunks) =>
                    _uploadedParts.Add(new UploadedPart(chunkNumber, fileContent.Take(numberOfBytes).ToArray(), numberOfChunks)))
                .Returns(Task.CompletedTask);

            _fileUploader = new FileUploader(_apiRequestSenderMock.Object, _amazonApiMock.Object);
        }

        [Theory]
        [InlineData(100, new[] { 100 })]
        [InlineData(ChunkSize, new[] { ChunkSize })]
        [InlineData(ChunkSize + 1, new[] { ChunkSize, 1 })]
        [InlineData(2 * ChunkSize, new[] { ChunkSize, ChunkSize })]
        [InlineData(2 * ChunkSize + 12345, new[] { ChunkSize, ChunkSize, 12345 })]
        public async Task EveryChunkExceptTheLastIsFullWhenStreamReturnsPartialReads(int fileSize, int[] expectedPartSizes)
        {
            var content = CreateContent(fileSize);

            await _fileUploader.UploadFileAsync(new PartialReadStream(content), CreateUploadQuery());

            Assert.Equal(expectedPartSizes, _uploadedParts.Select(part => part.Content.Length));
        }

        [Fact]
        public async Task ChunksAreNumberedInOrderAndCountIsSentWithEach()
        {
            var content = CreateContent(2 * ChunkSize + 12345);

            await _fileUploader.UploadFileAsync(new PartialReadStream(content), CreateUploadQuery());

            Assert.Equal(new uint[] { 1, 2, 3 }, _uploadedParts.Select(part => part.ChunkNumber));
            Assert.All(_uploadedParts, part => Assert.Equal(3u, part.NumberOfChunks));
            Assert.Equal(3, _registerChunkRequests.Count);
            Assert.Equal("3", ((FinalizeUploadQuery)_finalizeRequest.Query).Chunks);
        }

        [Fact]
        public async Task UploadedContentMatchesStream()
        {
            var content = CreateContent(2 * ChunkSize + 12345);

            await _fileUploader.UploadFileAsync(new PartialReadStream(content), CreateUploadQuery());

            var uploadedContent = _uploadedParts.SelectMany(part => part.Content).ToArray();
            Assert.Equal(content, uploadedContent);
        }

        [Fact]
        public async Task SaveMediaGetsMetapropertyOptionsAuditAndAccessRequestId()
        {
            var uploadQuery = CreateUploadQuery();
            uploadQuery.MetapropertyOptions.Add("metapropertyId", ["option1", "option2"]);
            uploadQuery.Audit = true;
            uploadQuery.AccessRequestId = "accessRequestId";

            await _fileUploader.UploadFileAsync(new PartialReadStream(CreateContent(100)), uploadQuery);

            var saveMediaQuery = (SaveMediaQuery)_saveMediaRequest.Query;
            Assert.Equal(new[] { "option1", "option2" }, saveMediaQuery.MetapropertyOptions["metapropertyId"]);
            Assert.True(saveMediaQuery.Audit);
            Assert.Equal("accessRequestId", saveMediaQuery.AccessRequestId);
            Assert.Equal($"/api/v4/media/save/{ImportId}/", _saveMediaRequest.Path);
        }

        private static UploadQuery CreateUploadQuery()
        {
            return new UploadQuery
            {
                Filepath = "image.jpg",
                OriginalFileName = "image.jpg",
                BrandId = "brandId",
            };
        }

        private static byte[] CreateContent(int size)
        {
            return Enumerable.Range(0, size).Select(index => (byte)(index % 251)).ToArray();
        }

        private record UploadedPart(uint ChunkNumber, byte[] Content, uint NumberOfChunks);

        /// <summary>
        /// Stream that returns fewer bytes than asked on every read, as network streams do
        /// </summary>
        private class PartialReadStream : MemoryStream
        {
            public PartialReadStream(byte[] content) : base(content)
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return base.Read(buffer, offset, Math.Min(count, MaxBytesPerRead));
            }

            public override int Read(Span<byte> buffer)
            {
                return base.Read(buffer[..Math.Min(buffer.Length, MaxBytesPerRead)]);
            }

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return base.ReadAsync(buffer, offset, Math.Min(count, MaxBytesPerRead), cancellationToken);
            }

            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            {
                return base.ReadAsync(buffer[..Math.Min(buffer.Length, MaxBytesPerRead)], cancellationToken);
            }
        }
    }
}
