// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bynder.Sample.Utils
{
    /// <summary>
    /// Helper class to launch a Browser in different platforms.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Launch a browser using the specified URL.
        /// </summary>
        /// <param name="url">URL.</param>
        public static void Launch(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")); // Works ok on windows
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }

            Console.WriteLine(string.Format("Open the url in your browser: {0}", url));
        }
    }
}
