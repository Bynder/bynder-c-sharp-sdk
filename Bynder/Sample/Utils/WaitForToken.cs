// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Bynder.Sample.Utils
{
    /// <summary>
    /// Class to notify waiting threads when oauth login proccess ended.
    /// </summary>
    public sealed class WaitForToken : IDisposable
    {
        /// <summary>
        /// True if Login was successful.
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Token passed in the URL when login is successful.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Event to wait until login is successful or timeout.
        /// </summary>
        public ManualResetEvent WaitHandle { get; } = new ManualResetEvent(false);

        /// <summary>
        /// Disposes waitHandle
        /// </summary>
        public void Dispose()
        {
            WaitHandle.Dispose();
        }
    }
}
