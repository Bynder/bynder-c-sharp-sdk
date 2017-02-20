// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Configuration;
using Bynder.Api;
using Bynder.Api.Queries;
using Bynder.Sample.OauthUtils;

namespace Bynder.Sample
{
    /// <summary>
    /// Class to show a little example on how to use the Bynder C# SDK. Before running this example is important that you fill 
    /// API_BASE_URL, CONSUMER_KEY and CONSUMER_SECRET in App.config
    /// </summary>
    public class ApiSample
    {
        /// <summary>
        /// Waiting milliseconds until users login
        /// </summary>
        private const int LOGIN_WAITING_MILLISECONDS = 60000;

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">arguments to main</param>
        public static void Main(string[] args)
        {
            using (var waitForToken = new WaitForToken())
            using (IBynderApi bynderApi = BynderApiFactory.Create(new Settings
            {
                CONSUMER_KEY = ConfigurationManager.AppSettings["CONSUMER_KEY"],
                CONSUMER_SECRET = ConfigurationManager.AppSettings["CONSUMER_SECRET"],
                TOKEN = ConfigurationManager.AppSettings["TOKEN"],
                TOKEN_SECRET = ConfigurationManager.AppSettings["TOKEN_SECRET"],
                URL = ConfigurationManager.AppSettings["API_BASE_URL"]
            }))
            {
                // We login using the browser. To do that we have to do the following:
                // 1. Request temporary tokens
                // 2. Get an authorized Url passing a callback where we will be redirected when login is successful.
                // 3. Open a browser with the authorized Url and start listening to the callback url.
                // 4. User logs in.
                // 5. We request final access tokens.
                bynderApi.GetRequestTokenAsync().Wait();
                using (var listener = new OauthHttpListener("http://localhost:8891/", waitForToken))
                {
                    using (var browser = new Browser(bynderApi.GetAuthorizeUrl("http://localhost:8891/"), null))
                    {
                        waitForToken.WaitHandle.WaitOne(LOGIN_WAITING_MILLISECONDS);
                    }
                }

                if (waitForToken.Success)
                {
                    bynderApi.GetAccessTokenAsync().Wait();

                    // Once the user has logged in we can start doing calls to asset bank manager to 
                    // get media information or upload new files.
                    var assetBankManager = bynderApi.GetAssetBankManager();

                    var mediaList = assetBankManager.RequestMediaListAsync(new MediaQuery
                    {
                        Page = 1,
                        Limit = 1
                    }).Result;

                    if (mediaList.Count > 0)
                    {
                        var media = assetBankManager.RequestMediaInfoAsync(new MediaInformationQuery { MediaId = mediaList[0].Id, Versions = 0 }).Result;
                        assetBankManager.UploadFileAsync(new UploadQuery { Filepath = @"Image/bynder-logo.png", BrandId = media.BrandId }).Wait();
                    }
                }
            }
        }
    }
}
