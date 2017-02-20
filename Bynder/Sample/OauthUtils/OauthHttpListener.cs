// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Net;

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
        /// We use this class to notify waiting threads that a call with oauth_token is done 
        /// to the Url we are listening to.
        /// </summary>
        private readonly WaitForToken _waitForTokenHandle;

        /// <summary>
        /// Creates a new OauthHttpListener that will listen to the Url and will notify waiting threads 
        /// when oauth login has completed
        /// </summary>
        /// <param name="url">Url we want to listen to</param>
        /// <param name="waitForTokenHandle">instance to pass token back and to notify waiting threads</param>
        public OauthHttpListener(string url, WaitForToken waitForTokenHandle)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _waitForTokenHandle = waitForTokenHandle;
            _listener.Start();
            _listener.BeginGetContext(new AsyncCallback(BeginContextCallback), _listener);
        }

        /// <summary>
        /// Disposes de listener
        /// </summary>
        public void Dispose()
        {
            _listener.Close();
        }

        /// <summary>
        /// Function callback that is called when we start receiving data. 
        /// We keep listening until we find oauth_token parameter in the Url.
        /// </summary>
        /// <param name="result">async result</param>
        private void BeginContextCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context;
            try
            {
                context = listener.EndGetContext(result);
            }
            catch (ObjectDisposedException)
            {
                // When Close this method is called with disposed object. Just swallow the exception
                return;
            }

            HttpListenerRequest request = context.Request;

            // Need to check if we are being requested with oauth_token. Because sometimes we 
            // would recieve the favicon call before, so we have to continue listening.
            if (!request.RawUrl.Contains("oauth_token"))
            {
                _listener.BeginGetContext(new AsyncCallback(BeginContextCallback), _listener);
                return;
            }

            var token = request.QueryString["oauth_token"];
            _waitForTokenHandle.Success = token != null;
            if (token != null)
            {
                _waitForTokenHandle.Token = token;
            }

            // Obtain a response object.
            HttpListenerResponse response = context.Response;

            // Construct a response.
            response.RedirectLocation = @"http://www.getbynder.com/en/";
            response.StatusCode = (int)HttpStatusCode.Redirect;
            response.Close();

            _waitForTokenHandle.WaitHandle.Set();
        }
    }
}
