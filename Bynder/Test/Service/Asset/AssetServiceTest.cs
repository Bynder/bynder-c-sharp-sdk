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
        [Fact]
        public async Task GetBrandsCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = (IList<Brand>) new List<Brand>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<IList<Brand>>>()))
             .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var brandList = await assetService.GetBrandsAsync();

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<IList<Brand>>>(
                    req => req.Path == "/api/v4/brands/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null)));

            Assert.Equal(result, brandList);
        }

        [Fact]
        public async Task GetMetapropertiesCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = (IDictionary<string, Metaproperty>) new Dictionary<string, Metaproperty>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<IDictionary<string, Metaproperty>>>()))
             .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var metaproperties = await assetService.GetMetapropertiesAsync();

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<IDictionary<string, Metaproperty>>>(
                    req => req.Path == "/api/v4/metaproperties/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == null)));

            Assert.Equal(result, metaproperties);
        }

        [Fact]
        public async Task GetMediaListCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = (IList<Media>) new List<Media>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<IList<Media>>>()))
             .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var mediaQuery = new MediaQuery();
            var mediaList = await assetService.GetMediaListAsync(mediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<IList<Media>>>(
                    req => req.Path == "/api/v4/media/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == mediaQuery)));

            Assert.Equal(result, mediaList);
        }

        [Fact]
        public async Task GetDownloadFileUrlCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = new DownloadFileUrl();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<DownloadFileUrl>>()))
                            .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var downloadMediaQuery = new DownloadMediaQuery{
                MediaId = "mediaId"
            };
            var downloadUrl = await assetService.GetDownloadFileUrlAsync(downloadMediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<DownloadFileUrl>>(
                    req => req.Path == $"/api/v4/media/{downloadMediaQuery.MediaId}/download/"
                    && req.HTTPMethod == HttpMethod.Get)));

            Assert.Equal(result.S3File, downloadUrl);
        }

        [Fact]
        public async Task GetMediaInfoCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = new Media();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<Media>>()))
             .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var mediaInformationQuery = new MediaInformationQuery{
                MediaId = "mediaId"
            };
            var media = await assetService.GetMediaInfoAsync(mediaInformationQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<Media>>(
                    req => req.Path == $"/api/v4/media/{mediaInformationQuery.MediaId}/"
                    && req.HTTPMethod == HttpMethod.Get
                    && req.Query == mediaInformationQuery)));

            Assert.Equal(result, media);
        }

        [Fact]
        public async Task ModifyMediaCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            object result = new { message = "Accepted", statuscode = 202 };
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<object>>()))
             .Returns(Task.FromResult(result));
            var assetService = new AssetService(apiRequestSender.Object);
            var modifyMediaQuery = new ModifyMediaQuery("mediaId");
            await assetService.ModifyMediaAsync(modifyMediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<object>>(
                    req => req.Path == $"/api/v4/media/{modifyMediaQuery.MediaId}/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == modifyMediaQuery
                    && req.DeserializeResponse == false)));
        }
    }
}
