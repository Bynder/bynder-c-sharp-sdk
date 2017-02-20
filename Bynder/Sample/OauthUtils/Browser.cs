// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Bynder.Sample.OauthUtils
{
    /// <summary>
    /// Class to open the Browser with a specific Url. If default explorer is internet explorer
    /// we will add -nomerge parameter so we can close it when login is successful. 
    /// For other browsers probably it will not be able to close it after a successful login.
    /// </summary>
    public sealed class Browser : IDisposable
    {
        /// <summary>
        /// Process information
        /// </summary>
        private readonly Process _proc;

        /// <summary>
        /// Creates an instance of browser that will create a process with the default browser pointing to 
        /// the Url passed.
        /// </summary>
        /// <param name="url">Url we want to open the browser with</param>
        /// <param name="waitForToken">token to notify if possible, if the user closes the browser</param>
        public Browser(string url, WaitForToken waitForToken)
        {
            var browserPath = GetBrowserPath();
            _proc = Process.Start(browserPath, GetBrowserArguments(browserPath, url));
            if (waitForToken != null
                && _proc != null)
            {
                _proc.EnableRaisingEvents = true;
                _proc.Exited += (sender, e) => waitForToken.WaitHandle.Set();
            }
        }

        /// <summary>
        /// Kills started process if possible.
        /// </summary>
        public void Dispose()
        {
            if (_proc != null)
            {
                try
                {
                    _proc.Kill();
                }
                catch (InvalidOperationException)
                {
                    // When openning browser for login, if explorer already opened this exception is thrown. Is not an error, so no action
                    return;
                }
            }
        }

        /// <summary>
        /// Gets default browser path. http://stackoverflow.com/questions/13621467/how-to-find-default-web-browser-using-c
        /// </summary>
        /// <returns>default browser path</returns>
        private static string GetBrowserPath()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command");

                // trim off quotes
                if (key != null)
                {
                    browser = key.GetValue(null).ToString().ToLower().Trim(new[] { '"' });
                }

                const string ExeExtension = ".exe";
                if (!browser.EndsWith(ExeExtension))
                {
                    // get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(ExeExtension, StringComparison.InvariantCultureIgnoreCase) + 4);
                }
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }

            return browser;
        }

        /// <summary>
        /// Gets the arguments to open the default browser with. In case the default browser is
        /// iexplorer it adds the -nomerge argument so a new process is created and we can close it.
        /// </summary>
        /// <param name="browserPath">default browser path</param>
        /// <param name="url">Url we want to open</param>
        /// <returns>arguments string</returns>
        private string GetBrowserArguments(string browserPath, string url)
        {
            if (browserPath.ToLower().Contains("iexplore.exe"))
            {
                return $"-nomerge {url}";
            }

            return url;
        }
    }
}
