// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bynder.Test.Utils
{
    /// <summary>
    /// Helper class to mock server responses. 
    /// It answers to requests with the content specified in file
    /// It uses <see cref="HttpListener"/>
    /// </summary>
    public sealed class TestHttpListener : IDisposable
    {
        /// <summary>
        /// HttpListener to listen to specific Url
        /// </summary>
        private readonly HttpListener _httpListener;

        /// <summary>
        /// Path of the file which contents will be the response to requests.
        /// </summary>
        private readonly string _responsePath;

        /// <summary>
        /// Status code we want to answer to requests. This helps to simulate
        /// HTTP error codes
        /// </summary>
        private readonly HttpStatusCode _statusCode;

        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="url">Url we start listening to</param>
        /// <param name="statusCode">HTTP status code that the class will return when having requests</param>
        /// <param name="responsePath">File whose contents the class will return in the response</param>
        public TestHttpListener(string url, HttpStatusCode statusCode, string responsePath)
        {
            _responsePath = responsePath;
            _statusCode = statusCode;

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(url);
            _httpListener.Start();

            _httpListener.BeginGetContext(new AsyncCallback(BeginContextCallback), _httpListener);
        }

        /// <summary>
        /// Event raised when a message is received. This is handy to add specific 
        /// checks to the request send to the server.
        /// </summary>
        public event EventHandler<HttpListenerRequestEventArgs> MessageReceived;

        /// <summary>
        /// Disposes the httpListener
        /// </summary>
        public void Dispose()
        {
            _httpListener.Close();
        }

        /// <summary>
        /// Function called when we start receiving a request
        /// </summary>
        /// <param name="result">Async result</param>
        private void BeginContextCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            MessageReceived?.Invoke(this, new HttpListenerRequestEventArgs(request));

            SendResponse(context.Response);
        }

        /// <summary>
        /// Sends as response the contents of the file specified.
        /// </summary>
        /// <param name="response">Http response</param>
        private void SendResponse(HttpListenerResponse response)
        {
            response.StatusCode = (int)_statusCode;
            var dr = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!string.IsNullOrEmpty(_responsePath))
            {
                using (var fs = new FileStream(Path.Combine(dr, _responsePath), FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(response.OutputStream);
                }
            }

            response.Close();
        }
    }
}
