// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bynder.Sample.OauthUtils
{
    /// <summary>
    /// Class to open the Browser with a specific Url.
    /// </summary>
    public sealed class Browser
    {
        /// <summary>
        /// Creates an instance of browser that will open the default browser pointing to the Url passed.
        /// </summary>
        /// <param name="url">Url we want to open the browser with</param>
        /// <param name="waitForToken">token to notify if possible, if the user closes the browser</param>
        public Browser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }

            Console.WriteLine(string.Format("Open the url in your browser: {0}", url));
        }
    }
}
