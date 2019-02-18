// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading.Tasks;

namespace Bynder.Sample.OauthUtils
{
    /// <summary>
    /// Util class to listen to specific Url.
    /// It is used to login to Bynder through the browser when we specify the callback Url without the need to add any custom UI.
    /// </summary>
    public sealed class OauthHttpListener : IDisposable
    {
        /// <summary>
        /// HttpListener that listens to Url passed in the constructor
        /// </summary>
        private readonly HttpListener _listener;

        /// <summary>
        /// Creates a new OauthHttpListener that will listen to the Url and will notify waiting threads
        /// when oauth login has completed
        /// </summary>
        /// <param name="url">Url we want to listen to</param>
        /// <param name="waitForTokenHandle">instance to pass token back and to notify waiting threads</param>
        public OauthHttpListener(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _listener.Start();
        }

        /// <summary>
        /// Disposes de listener
        /// </summary>
        public void Dispose()
        {
            _listener.Close();
        }

        /// <summary>
        /// Function that waits utils we start receiving data.
        /// We keep listening until we find oauth_token parameter in the Url.
        /// </summary>
        /// <param name="result">async result</param>
        public async Task<string> WaitForToken()
        {
            string token = null;
            HttpListenerContext context = null;

            // Need to check if we are being requested with oauth_token. Because sometimes we
            // would recieve the favicon call before, so we have to continue listening.
            while (token == null)
            {
                context = await _listener.GetContextAsync();
                token = context.Request.QueryString.Get("oauth_token");
            }

            // Obtain a response object.
            HttpListenerResponse response = context.Response;

            // Construct a response.
            response.RedirectLocation = @"http://www.getbynder.com/en/";
            response.StatusCode = (int)HttpStatusCode.Redirect;
            response.Close();

            return token;
        }
    }
}
