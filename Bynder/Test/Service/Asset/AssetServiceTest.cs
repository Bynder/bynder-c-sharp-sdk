// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Service.Asset;
using Moq;
using Xunit;

namespace Bynder.Test.Service.Asset
{
    public class AssetServiceTest
    {
        private readonly Mock<IApiRequestSender> _apiRequestSenderMock;
        private readonly AssetService _assetService;

        public AssetServiceTest()
        {
            _apiRequestSenderMock = new Mock<IApiRequestSender>();
            _assetService = new AssetService(_apiRequestSenderMock.Object);
        }

        [Fact]
        public async Task GetBrandsCallsRequestSenderWithValidRequest()
        {
            var result = new List<Brand>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IList<Brand>>>()))
                .ReturnsAsync(result);
            var brandList = await _assetService.GetBrandsAsync();

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<Brand>>>(
                    req => req.Path == "/api/v4/brands/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null
                )
            ));

            Assert.Equal(result, brandList);
        }

        [Fact]
        public async Task GetMetapropertiesCallsRequestSenderWithValidRequest()
        {
            var result = new Dictionary<string, Metaproperty>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IDictionary<string, Metaproperty>>>()))
                .ReturnsAsync(result);
            var metaproperties = await _assetService.GetMetapropertiesAsync();

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IDictionary<string, Metaproperty>>>(
                    req => req.Path == "/api/v4/metaproperties/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null
                )
            ));

            Assert.Equal(result, metaproperties);
        }

        [Fact]
        public async Task GetMediaListCallsRequestSenderWithValidRequest()
        {
            var result = new List<Media>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IList<Media>>>()))
                .ReturnsAsync(result);
            var mediaQuery = new MediaQuery();
            var mediaList = await _assetService.GetMediaListAsync(mediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<Media>>>(
                    req => req.Path == "/api/v4/media/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == mediaQuery
                )
            ));

            Assert.Equal(result, mediaList);
        }

        [Fact]
        public async Task GetDownloadFileUrlCallsRequestSenderWithValidRequest()
        {
            var result = new DownloadFileUrl();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<DownloadFileUrl>>()))
                .ReturnsAsync(result);
            var downloadMediaQuery = new DownloadMediaQuery{
                MediaId = "mediaId"
            };
            var downloadUrl = await _assetService.GetDownloadFileUrlAsync(downloadMediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<DownloadFileUrl>>(
                    req => req.Path == $"/api/v4/media/{downloadMediaQuery.MediaId}/download/"
                    && req.HTTPMethod == HttpMethod.Get
                )
            ));

            Assert.Equal(result.S3File, downloadUrl);
        }

        [Fact]
        public async Task GetMediaInfoCallsRequestSenderWithValidRequest()
        {
            var result = new Media();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<Media>>()))
                .ReturnsAsync(result);
            var mediaInformationQuery = new MediaInformationQuery{
                MediaId = "mediaId"
            };
            var media = await _assetService.GetMediaInfoAsync(mediaInformationQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<Media>>(
                    req => req.Path == $"/api/v4/media/{mediaInformationQuery.MediaId}/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == mediaInformationQuery
                )
            ));

            Assert.Equal(result, media);
        }

        [Fact]
        public async Task ModifyMediaCallsRequestSenderWithValidRequest()
        {
            var result = new { message = "Accepted", statuscode = 202 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var modifyMediaQuery = new ModifyMediaQuery("mediaId");
            await _assetService.ModifyMediaAsync(modifyMediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == $"/api/v4/media/{modifyMediaQuery.MediaId}/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == modifyMediaQuery
                )
            ));
        }
    }
}
