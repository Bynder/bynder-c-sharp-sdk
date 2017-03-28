// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Impl;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Api.Queries.Collections;
using Bynder.Api.Queries.Collections.PermissionOptions;
using Bynder.Models;
using Moq;
using NUnit.Framework;

namespace Bynder.Test.Collections
{
    /// <summary>
    /// Class to test Asset bank implementation
    /// </summary>
    [TestFixture]
    public class CollectionsManagerTest
    {
        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.GetCollectionsAsync(GetCollectionsQuery)"/>, request has correct values
        /// in Url, HTTPMethod and Query.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenGetCollectionsAsyncCalledContainsExpectedResult()
        {
            var query = new GetCollectionsQuery() { Limit = 50, Page = 1 };

            IList<Collection> mediaCollections = new List<Collection>();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<IList<Collection>>>())).Returns(Task.FromResult(mediaCollections));
            
            var manager = new CollectionsManager(mock.Object);
            var collectionList = await manager.GetCollectionsAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<IList<Collection>>>(req => req.Uri == $"/api/v4/collections/"
                && req.HTTPMethod == HttpMethod.Get
                && req.Query == query)));
            Assert.AreEqual(mediaCollections, collectionList);
        }

        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.GetCollectionAsync(string)"/>"/>, request has correct
        /// values in Url and HTTPMethod.
        /// </summary>
        /// <param name="id">Id of a collection</param>
        /// <returns>Task to wait</returns>
        [TestCase("10D82138-AA72-4824-967C6E425810FBDF")]
        public async Task WhenGetCollectionsAsyncCalledContainsExpectedResult(string id)
        {
            Collection mediaCollection = new Collection();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<Collection>>())).Returns(Task.FromResult(mediaCollection));

            var manager = new CollectionsManager(mock.Object);
            var collection = await manager.GetCollectionAsync(id);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<Collection>>(req => req.Uri == $"/api/v4/collections/{id}/"
                && req.HTTPMethod == HttpMethod.Get)));
            Assert.AreEqual(mediaCollection, collection);
        }

        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.GetMediaAsync(GetMediaQuery)(string)"/>"/>, request has correct
        /// values in Url and HTTPMethod.
        /// </summary>
        /// <param name="id">Id of a collection</param>
        /// <returns>Task to wait</returns>
        [TestCase("00000000-0000-0000-0000000000000000")]
        public async Task WhenGetMediaAsyncCalledContainsExpectedResult(string id)
        {
            var query = new GetMediaQuery(id);

            IList<string> mediaList = new List<string>();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<IList<string>>>())).Returns(Task.FromResult(mediaList));

            var manager = new CollectionsManager(mock.Object);
            var mediaOfCollection = await manager.GetMediaAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<IList<string>>>(req => req.Uri == $"/api/v4/collections/{id}/media/"
                && req.HTTPMethod == HttpMethod.Get)));
            Assert.AreEqual(mediaList, mediaOfCollection);
        }

        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.AddMediaAsync(GetMediaQuery)(string)"/>"/>, request has correct
        /// values in Url and HTTPMethod.
        /// </summary>
        /// <param name="id">Id of a collection</param>
        /// <param name="mediaId">Id of a media asset</param>
        /// <returns>Task to wait</returns>
        [TestCase("00000000-0000-0000-0000000000000000", "00000000-0000-0000-0000000000000000")]
        public async Task WhenAddMediaAsyncCalledContainsExpectedResult(string id, string mediaId)
        {
            var query = new AddMediaQuery(id, new[] { mediaId });

            IList<string> mediaList = new List<string>();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<string>>())).Returns(Task.FromResult(string.Empty));

            var manager = new CollectionsManager(mock.Object);
            await manager.AddMediaAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<string>>(req => req.Uri == $"/api/v4/collections/{id}/media/"
                && req.HTTPMethod == HttpMethod.Post
                && ((AddMediaQuery)req.Query).MediaIds.Contains(mediaId))));
        }

        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.RemoveMediaAsync(RemoveMediaQuery)"/>"/>, request has correct
        /// values in Url and HTTPMethod.
        /// </summary>
        /// <param name="id">Id of a collection</param>
        /// <param name="mediaId">Id of a media asset</param>
        /// <returns>Task to wait</returns>
        [TestCase("00000000-0000-0000-0000000000000000", "00000000-0000-0000-0000000000000000")]
        public async Task WhenRemoveMediaAsyncCalledContainsExpectedResult(string id, string mediaId)
        {
            var query = new RemoveMediaQuery(id, new[] { mediaId });

            IList<string> mediaList = new List<string>();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<string>>())).Returns(Task.FromResult(string.Empty));

            var manager = new CollectionsManager(mock.Object);
            await manager.RemoveMediaAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<string>>(req => req.Uri == $"/api/v4/collections/{id}/media/"
                && req.HTTPMethod == HttpMethod.Delete
                && ((RemoveMediaQuery)req.Query).MediaIds.Contains(mediaId))));
        }

        /// <summary>
        /// Tests that when <see cref="Api.ICollectionsManager.ShareCollectionAsync(ShareQuery)"/>, request has correct
        /// values in Url and HTTPMethod.
        /// </summary>
        /// <param name="id">Id of a collection</param>
        /// <param name="emailAddresses">Comma separated email addresses of the recipients</param>
        /// <returns>Task to wait</returns>
        [TestCase("00000000-0000-0000-0000000000000000", "someone@bynder.com")]
        [TestCase("00000000-0000-0000-0000000000000000", "someone1@bynder.com,someone2@bynder.com")]
        public async Task WhenShareAsyncCalledContainsExpectedResult(string id, string emailAddresses)
        {
            IList<string> recipients = emailAddresses.Split(',');

            var query = new ShareQuery(id, recipients, SharingPermssion.View);

            IList<string> mediaList = new List<string>();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<string>>())).Returns(Task.FromResult(string.Empty));

            var manager = new CollectionsManager(mock.Object);
            await manager.ShareCollectionAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<string>>(req => req.Uri == $"/api/v4/collections/{id}/share/"
                && req.HTTPMethod == HttpMethod.Post
                && ((ShareQuery)req.Query).Recipients.Count == recipients.Count
                && ((ShareQuery)req.Query).Permission == SharingPermssion.View)));
        }
    }
}
