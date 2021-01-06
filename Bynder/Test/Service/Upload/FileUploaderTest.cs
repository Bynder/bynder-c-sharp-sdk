using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model.Upload;
using Bynder.Sdk.Query.Upload;
using Bynder.Sdk.Service.Upload;
using Bynder.Sdk.Utils;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Xunit;

namespace Bynder.Test.Service.Upload
{
    public class FileUploaderTest
    {
        private const string _path = "/path/to/file.ext";
        private const string _brandId = "SOME_BRAND_ID";
        private const string _mediaId = "SOME_MEDIA_ID";
        private const string _fileId = "SOME_FILE_ID";
        private readonly List<byte[]> _chunks = new List<byte[]> {
            new byte[] { 0x12, 0x34, 0x56 },
            new byte[] { 0xd2, 0x27, 0x3d },
            new byte[] { 0xc4, 0x1b },
        };
        private readonly byte[] _fileData;
        private readonly IList<string> _tags = new List<string> { "foo", "bar", "baz" };
        private readonly SaveMediaResponse _saveMediaResponse = new SaveMediaResponse();

        private readonly Mock<IApiRequestSender> _apiRequestSenderMock;
        private readonly FileUploader _uploader;

        public FileUploaderTest()
        {
            _fileData = _chunks.SelectMany(x => x).ToArray();

            _apiRequestSenderMock = new Mock<IApiRequestSender>();

            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.Is<ApiRequest<PrepareUploadResponse>>(
                    req => req.Path == "/v7/file_cmds/upload/prepare"
                )))
                .ReturnsAsync(new PrepareUploadResponse { FileId = _fileId });

            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.Is<ApiRequest<SaveMediaResponse>>(
                    req => req.Path == $"/api/v4/media/save/{_fileId}"
                )))
                .ReturnsAsync(_saveMediaResponse);

            _apiRequestSenderMock
                .Setup(sender => sender.SendRequestAsync(It.Is<ApiRequest<SaveMediaResponse>>(
                    req => req.Path == $"/api/v4/media/{_mediaId}/save/{_fileId}"
                )))
                .ReturnsAsync(_saveMediaResponse);

            _uploader = new FileUploader(
                _apiRequestSenderMock.Object,
                fileSystem: new MockFileSystem(
                    new Dictionary<string, MockFileData>
                    {
                        { _path, new MockFileData(_fileData) }
                    }
                ),
                chunkSize: 3
            );
        }

        private void CheckFilesServiceRequests()
        {
            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(It.Is<ApiRequest<PrepareUploadResponse>>(
                req => req.Path == "/v7/file_cmds/upload/prepare"
                    && req.HTTPMethod == HttpMethod.Post
            )));

            for (int i = 0; i < _chunks.Count; i++)
            {
                byte[] chunk = _chunks[i];
                _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(It.Is<ApiRequest>(
                    req => req.Path == $"/v7/file_cmds/upload/{_fileId}/chunk/{i}"
                        && req.HTTPMethod == HttpMethod.Post
                        && Enumerable.SequenceEqual(req.BinaryContent, chunk)
                        && Enumerable.SequenceEqual(
                            req.Headers,
                            new Dictionary<string, string>
                            {
                                { "Content-SHA256", SHA256Utils.SHA256hex(chunk) }
                            }
                        )
                )));
            }

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(It.Is<ApiRequest>(
                req => req.Path == $"/v7/file_cmds/upload/{_fileId}/finalise_api"
                    && req.HTTPMethod == HttpMethod.Post
                    && new CompareLogic().Compare(
                        req.Query,
                        new FinalizeUploadQuery
                        {
                            ChunksCount = _chunks.Count,
                            Filename = Path.GetFileName(_path),
                            FileSize = _fileData.Length,
                            SHA256 = SHA256Utils.SHA256hex(_fileData),
                        }
                    ).AreEqual
            )));
        }

        [Fact]
        public async Task UploadFileToNewAssetAsync()
        {
            var saveMediaResponse = await _uploader.UploadFileToNewAssetAsync(_path, _brandId, _tags);

            Assert.Equal(_saveMediaResponse, saveMediaResponse);

            CheckFilesServiceRequests();

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(It.Is<ApiRequest<SaveMediaResponse>>(
                req => req.Path == $"/api/v4/media/save/{_fileId}"
                    && req.HTTPMethod == HttpMethod.Post
                    && new CompareLogic().Compare(
                        req.Query,
                        new SaveMediaQuery
                        {
                            Filename = Path.GetFileName(_path),
                            BrandId = _brandId,
                            Tags = _tags,
                        }
                    ).AreEqual
            )));
        }

        [Fact]
        public async Task UploadFileToExistingAssetAsync()
        {
            var saveMediaResponse = await _uploader.UploadFileToExistingAssetAsync(_path, _mediaId);

            Assert.Equal(_saveMediaResponse, saveMediaResponse);

            CheckFilesServiceRequests();

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(It.Is<ApiRequest<SaveMediaResponse>>(
                req => req.Path == $"/api/v4/media/{_mediaId}/save/{_fileId}"
                    && req.HTTPMethod == HttpMethod.Post
            )));
        }
    }
}
