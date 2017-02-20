// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Api.Impl;
using Bynder.Api.Impl.Oauth;
using Bynder.Api.Queries;
using Bynder.Models;
using Moq;
using NUnit.Framework;

namespace Bynder.Test.AssetBank
{
    /// <summary>
    /// Class to test Asset bank implementation
    /// </summary>
    [TestFixture]
    public class AssetBankTest
    {
        /// <summary>
        /// Tests that when <see cref="Api.IAssetBankManager.RequestMediaInfoAsync(string)"/>, request has correct values
        /// in Url, and HTTPMethod
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenGetMediaInformationByIdThenRequestHasCorrectMediaValues()
        {
            const string MediaId = "8888";
            Media returnedMedia = new Media();
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<Media>>())).Returns(Task.FromResult(returnedMedia));

            var assetBankManager = new AssetBankManager(mock.Object);
            var media = await assetBankManager.RequestMediaInfoAsync(new MediaInformationQuery { MediaId = MediaId });
            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<Media>>(req => req.Uri == $"/api/v4/media/{MediaId}/"
                && req.HTTPMethod == HttpMethod.Get
                && ((MediaInformationQuery)req.Query).Versions == 1)));

            Assert.AreEqual(returnedMedia, media);
        }

        /// <summary>
        /// Tests that when <see cref="Api.IAssetBankManager.RequestMediaListAsync(MediaQuery)"/> is called, request has correct values in
        /// Url, HTTPMethod and Query.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenGetMediaThenRequestContainsExpectedValues()
        {
            IList<Media> returnedMediaList = new List<Media>();

            var query = new MediaQuery
            {
                PropertyOptionId = { "12345", "123123" },
                Limit = 50,
                Page = 1
            };

            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<IList<Media>>>())).Returns(Task.FromResult(returnedMediaList));

            var assetBankManager = new AssetBankManager(mock.Object);
            var mediaList = await assetBankManager.RequestMediaListAsync(query);

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<IList<Media>>>(req => req.Uri == $"/api/v4/media/"
                && req.HTTPMethod == HttpMethod.Get
                && req.Query == query)));
            Assert.AreEqual(returnedMediaList, mediaList);
        }

        /// <summary>
        /// Tests that when <see cref="Bynder.Api.IAssetBankManager.GetMetapropertiesAsync"/>"/>, request has correct
        /// values in Url and HTTPMethod
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenGetMetapropertiesThenRequestContainsExpectedValues()
        {
            var metaproperty = new Metaproperty();

            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<IDictionary<string, Metaproperty>>>()))
                .Returns(Task.FromResult<IDictionary<string, Metaproperty>>(new Dictionary<string, Metaproperty>
                                        {
                                            { "metaproperty1", metaproperty }
                                        }));

            var assetBankManager = new AssetBankManager(mock.Object);
            var metaproperties = await assetBankManager.GetMetapropertiesAsync();

            mock.Verify(reqSenderMock
                => reqSenderMock.SendRequestAsync(It.Is<Request<IDictionary<string, Metaproperty>>>(req => req.Uri == $"/api/v4/metaproperties/"
                && req.HTTPMethod == HttpMethod.Get
                && req.Query == null)));

            Assert.AreEqual(1, metaproperties.Count);
            Assert.AreEqual(metaproperties["metaproperty1"], metaproperty);
        }

        /// <summary>
        /// Tests that when Download Url is called then request has correct values.
        /// It tests when MediaItemId is specified and when it's not.
        /// </summary>
        /// <returns>Task to wait</returns>
        [Test]
        public async Task WhenDownloadUrlIsRequestedThenRequestContainsExpectedValues()
        {
            const string MediaId = "MediaId";
            const string MediaItemId = "MediaItemId";
            var mock = new Mock<IOauthRequestSender>();
            mock.Setup(reqSenderMock => reqSenderMock.SendRequestAsync(It.IsAny<Request<DownloadFileUrl>>()))
                .Returns(Task.FromResult(new DownloadFileUrl()));

            var assetBankManager = new AssetBankManager(mock.Object);
            var downloadFileUrl = await assetBankManager.GetDownloadFileUrlAsync(new DownloadMediaQuery
            {
                MediaId = MediaId
            });

            mock.Verify(reqSenderMock
               => reqSenderMock.SendRequestAsync(It.Is<Request<DownloadFileUrl>>(req => req.Uri == $"/api/v4/media/{MediaId}/download/"
               && req.HTTPMethod == HttpMethod.Get)));

            downloadFileUrl = await assetBankManager.GetDownloadFileUrlAsync(new DownloadMediaQuery
            {
                MediaId = MediaId,
                MediaItemId = MediaItemId
            });

            mock.Verify(reqSenderMock
               => reqSenderMock.SendRequestAsync(It.Is<Request<DownloadFileUrl>>(req => req.Uri == $"/api/v4/media/{MediaId}/download/{MediaItemId}/"
               && req.HTTPMethod == HttpMethod.Get)));
        }
    }
}
