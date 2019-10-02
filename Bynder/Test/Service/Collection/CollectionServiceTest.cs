// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model;
using Bynder.Sdk.Query.Collection;
using Bynder.Sdk.Service.Collection;
using Moq;
using Xunit;

namespace Bynder.Test.Service.Collection
{
    public class CollectionServiceTest
    {
        [Fact]
        public async Task CreateCollectionCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = "";
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var createCollectionQuery = new CreateCollectionQuery("name");
            await collectionService.CreateCollectionAsync(createCollectionQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<string>>(
                    req => req.Path == "/api/v4/collections/"
                    && req.HTTPMethod == HttpMethod.Post
                    && req.Query == createCollectionQuery
                    && req.DeserializeResponse == false)));
        }

        [Fact]
        public async Task DeleteCollectionCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = "";
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var collectionId = "collectionId";
            await collectionService.DeleteCollectionAsync(collectionId);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<string>>(
                    req => req.Path == $"/api/v4/collections/{collectionId}/"
                    && req.HTTPMethod == HttpMethod.Delete)));
        }

        [Fact]
        public async Task GetCollectionCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = new Bynder.Sdk.Model.Collection();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<Bynder.Sdk.Model.Collection>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var collectionId = "collectionId";
            var collection = await collectionService.GetCollectionAsync(collectionId);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<Bynder.Sdk.Model.Collection>>(
                    req => req.Path == $"/api/v4/collections/{collectionId}/"
                    && req.HTTPMethod == HttpMethod.Get)));

            Assert.Equal(result, collection);
        }

        [Fact]
        public async Task GetCollectionsCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = (IList<Bynder.Sdk.Model.Collection>) new List<Bynder.Sdk.Model.Collection>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<IList<Bynder.Sdk.Model.Collection>>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var getCollectionsQuery = new GetCollectionsQuery();
            var collectionList = await collectionService.GetCollectionsAsync(getCollectionsQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<IList<Bynder.Sdk.Model.Collection>>>(
                    req => req.Path == "/api/v4/collections/"
                    && req.Query == getCollectionsQuery
                    && req.HTTPMethod == HttpMethod.Get)));

            Assert.Equal(result, collectionList);
        }

        [Fact]
        public async Task GetMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = (IList<string>) new List<string>();
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<IList<string>>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var getMediaQuery = new GetMediaQuery("collectionId");
            var mediaIds = await collectionService.GetMediaAsync(getMediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<IList<string>>>(
                    req => req.Path == $"/api/v4/collections/{getMediaQuery.CollectionId}/media/"
                    && req.HTTPMethod == HttpMethod.Get)));

            Assert.Equal(result, mediaIds);
        }

        [Fact]
        public async Task AddMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = "";
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var addMediaQuery = new AddMediaQuery("collectionId", new List<string>());
            await collectionService.AddMediaAsync(addMediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<string>>(
                    req => req.Path == $"/api/v4/collections/{addMediaQuery.CollectionId}/media/"
                    && req.Query == addMediaQuery
                    && req.HTTPMethod == HttpMethod.Post
                    && req.DeserializeResponse == false)));
        }

        [Fact]
        public async Task RemoveMediaAsyncCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = "";
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var removeMediaQuery = new RemoveMediaQuery("collectionId", new List<string>());
            await collectionService.RemoveMediaAsync(removeMediaQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<string>>(
                    req => req.Path == $"/api/v4/collections/{removeMediaQuery.CollectionId}/media/"
                    && req.Query == removeMediaQuery
                    && req.HTTPMethod == HttpMethod.Delete)));
        }

        [Fact]
        public async Task ShareCollectionAsyncCallsRequestSenderWithValidRequest()
        {
            var apiRequestSender = new Mock<IApiRequestSender>();
            var result = "";
            apiRequestSender.Setup(sender => sender.SendRequestAsync(It.IsAny<Request<string>>()))
             .Returns(Task.FromResult(result));
            var collectionService = new CollectionService(apiRequestSender.Object);
            var shareQuery = new ShareQuery("collectionId", new List<string>(), SharingPermission.View);
            await collectionService.ShareCollectionAsync(shareQuery);

            apiRequestSender.Verify(sender => sender.SendRequestAsync(
                It.Is<Request<string>>(
                    req => req.Path == $"/api/v4/collections/{shareQuery.CollectionId}/share/"
                    && req.Query == shareQuery
                    && req.HTTPMethod == HttpMethod.Post
                    && req.DeserializeResponse == false)));
        }
    }
}
