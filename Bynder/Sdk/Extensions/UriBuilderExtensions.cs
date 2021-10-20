using System;

namespace Bynder.Sdk.Extensions
{
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Allows to append a path to a url without overriding the existing path
        /// (which happens when you would set the Path property).
        ///
        /// For example, UriBuilder("https://example.com/base") { Path = "path/to/something" }
        /// will result in "https://example.com/path/to/something" (removing the "base" part)
        ///
        /// While UriBuilder("https://example.com/base").AppendPath("path/to/something")
        /// will result in "https://example.com/base/path/to/something"
        ///
        /// It will automatically ensure that the paths are combined correctly using only one "/".
        /// </summary>
        /// <param name="uriBuilder">extended UriBuilder instance</param>
        /// <param name="path">path to be appended to the full url</param>
        /// <returns>UriBuilder instance with the appended path</returns>
        public static UriBuilder AppendPath(this UriBuilder uriBuilder, String path)
        {
            uriBuilder.Path = $"{uriBuilder.Path.TrimEnd('/')}/{path.TrimStart('/')}";
            return uriBuilder;
        }
    }
}
