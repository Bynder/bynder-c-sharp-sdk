// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
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
        public async Task GetMetapropertyCallsRequestSenderWithValidRequest()
        {
            var result = new Metaproperty();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<Metaproperty>>()))
                .ReturnsAsync(result);
            var query = new MetapropertyQuery("metapropertyId");
            var metaproperty = await _assetService.GetMetapropertyAsync(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<Metaproperty>>(req =>
                    req.Path == $"/api/v4/metaproperties/{query.MetapropertyId}"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null
                )
            ));

            Assert.Equal(result, metaproperty);
        }

        [Fact]
        public async Task GetMetapropertyDependenciesCallsRequestSenderWithValidRequest()
        {
            var result = new List<string>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IList<string>>>()))
                .ReturnsAsync(result);
            var query = new MetapropertyQuery("metapropertyId");
            var dependencies = await _assetService.GetMetapropertyDependenciesAsync(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<string>>>(req =>
                    req.Path == $"api/v4/metaproperties/{query.MetapropertyId}/dependencies/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null
                )
            ));

            Assert.Equal(result, dependencies);
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
        public async Task GetMediaListItemsCallsRequestSenderWithValidRequest()
        {
            var result = new MediaList();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<MediaList>>()))
                .ReturnsAsync(result);
            var mediaListQuery = new MediaListQuery();
            var mediaList = await _assetService.GetMediaListAsync(mediaListQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<MediaList>>(
                    req => req.Path == "/api/v4/media/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == mediaListQuery
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
            var result = new Status { Message = "Accepted", StatusCode = 202 };
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

        [Fact]
        public async Task GetTagsCallsRequestSenderWithValidRequest()
        {
            var result = new Status { Message = "Accepted", StatusCode = 202 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var query = new GetTagsQuery { };
            await _assetService.GetTagsAsync(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<Tag>>>(req =>
                    req.Path == "/api/v4/tags/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == query
                )
            ));
        }

        [Fact]
        public async Task AddTagToMediaCallsRequestSenderWithValidRequest()
        {
            var result = new Status { Message = "Accepted", StatusCode = 202 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var query = new AddTagToMediaQuery("tagId", new List<string>());
            await _assetService.AddTagToMediaAsync(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(req =>
                    req.Path == $"/api/v4/tags/{query.TagId}/media/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == query
                )
            ));
        }

        [Fact]
        public async Task CreateAssetUsageCallsRequestSenderWithValidRequest()
        {
            var result = new Status { Message = "Accepted", StatusCode = 200 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var query = new AssetUsageQuery("integrationId", "assetId");
            await _assetService.CreateAssetUsage(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(req =>
                    req.Path == $"/api/media/usage/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == query
                )
            ));
        }

        [Fact]
        public async Task DeleteAssetUsageCallsRequestSenderWithValidRequest()
        {
            var result = new Status { Message = "Accepted", StatusCode = 204 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);

            var query = new AssetUsageQuery("integrationId", "assetId") { Uri = "/test/test.jpg" };
            await _assetService.DeleteAssetUsage(query);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(req =>
                    req.Path == $"/api/media/usage/"
                    && req.HTTPMethod == HttpMethod.Delete
                    && req.Query == query
                )
            ));
        }
    }
}
