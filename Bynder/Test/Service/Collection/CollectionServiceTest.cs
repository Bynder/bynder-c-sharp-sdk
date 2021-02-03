// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Service.Collection;
using Moq;
using Xunit;

namespace Bynder.Test.Service.Collection
{
    public class CollectionServiceTest
    {
        private readonly Mock<IApiRequestSender> _apiRequestSenderMock;
        private readonly CollectionService _collectionService;

        public CollectionServiceTest()
        {
            _apiRequestSenderMock = new Mock<IApiRequestSender>();
            _collectionService = new CollectionService(_apiRequestSenderMock.Object);
        }

        [Fact]
        public async Task CreateCollectionCallsRequestSenderWithValidRequest()
        {
            var result = new { message = "Created", statuscode = 201 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var createCollectionQuery = new CreateCollectionQuery("name");
            await _collectionService.CreateCollectionAsync(createCollectionQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == "/api/v4/collections/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == createCollectionQuery
                )
            ));
        }

        [Fact]
        public async Task DeleteCollectionCallsRequestSenderWithValidRequest()
        {
            var result = new { };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                 .ReturnsAsync(result);
            var collectionId = "collectionId";
            await _collectionService.DeleteCollectionAsync(collectionId);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == $"/api/v4/collections/{collectionId}/"
                    && req.HTTPMethod == HttpMethod.Delete
                )
            ));
        }

        [Fact]
        public async Task GetCollectionCallsRequestSenderWithValidRequest()
        {
            var result = new Sdk.Model.Collection();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<Sdk.Model.Collection>>()))
                .ReturnsAsync(result);
            var collectionId = "collectionId";
            var collection = await _collectionService.GetCollectionAsync(collectionId);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<Sdk.Model.Collection>>(
                    req => req.Path == $"/api/v4/collections/{collectionId}/"
                    && req.HTTPMethod == HttpMethod.Get
                )
            ));

            Assert.Equal(result, collection);
        }

        [Fact]
        public async Task GetCollectionsCallsRequestSenderWithValidRequest()
        {
            var result = new List<Sdk.Model.Collection>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IList<Sdk.Model.Collection>>>()))
                .ReturnsAsync(result);
            var getCollectionsQuery = new GetCollectionsQuery();
            var collectionList = await _collectionService.GetCollectionsAsync(getCollectionsQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<Sdk.Model.Collection>>>(
                    req => req.Path == "/api/v4/collections/"
                    && req.Query == getCollectionsQuery
                    && req.HTTPMethod == HttpMethod.Get
                )
            ));

            Assert.Equal(result, collectionList);
        }

        [Fact]
        public async Task GetMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var result = new List<string>();
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest<IList<string>>>()))
                .ReturnsAsync(result);
            var getMediaQuery = new GetMediaQuery("collectionId");
            var mediaIds = await _collectionService.GetMediaAsync(getMediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest<IList<string>>>(
                    req => req.Path == $"/api/v4/collections/{getMediaQuery.CollectionId}/media/"
                    && req.HTTPMethod == HttpMethod.Get
                )
            ));

            Assert.Equal(result, mediaIds);
        }

        [Fact]
        public async Task AddMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var result = new { message = "Accepted", statuscode = 202 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var addMediaQuery = new AddMediaQuery("collectionId", new List<string>());
            await _collectionService.AddMediaAsync(addMediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == $"/api/v4/collections/{addMediaQuery.CollectionId}/media/"
                    && req.Query == addMediaQuery
                    && req.HTTPMethod == HttpMethod.Post
                )
            ));
        }

        [Fact]
        public async Task RemoveMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var result = new { };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var removeMediaQuery = new RemoveMediaQuery("collectionId", new List<string>());
            await _collectionService.RemoveMediaAsync(removeMediaQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == $"/api/v4/collections/{removeMediaQuery.CollectionId}/media/"
                    && req.Query == removeMediaQuery
                    && req.HTTPMethod == HttpMethod.Delete
                )
            ));
        }

        [Fact]
        public async Task ShareCollectionAsyncCallsRequestSenderWithValidRequest()
        {
            var result = new { message = "Accepted", statuscode = 202 };
            _apiRequestSenderMock.Setup(sender => sender.SendRequestAsync(It.IsAny<ApiRequest>()))
                .ReturnsAsync(result);
            var shareQuery = new ShareQuery("collectionId", new List<string>(), SharingPermission.View);
            await _collectionService.ShareCollectionAsync(shareQuery);

            _apiRequestSenderMock.Verify(sender => sender.SendRequestAsync(
                It.Is<ApiRequest>(
                    req => req.Path == $"/api/v4/collections/{shareQuery.CollectionId}/share/"
                    && req.Query == shareQuery
                    && req.HTTPMethod == HttpMethod.Post
                )
            ));
        }
    }
}
