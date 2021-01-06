// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bynder.Sdk.Utils
{
    internal class SHA256Utils
    {
        /// <summary>
        /// Computes a SHA-256 hash from a <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">the byte array</param>
        /// <returns>SHA-256 hash of the <see cref="byte"/> array</returns>
        internal static string SHA256hex(byte[] bytes)
        {
            using (var sha256 = SHA256.Create())
            {
                return SHA256hexString(sha256.ComputeHash(bytes));
            }
        }

        /// <summary>
        /// Computes a SHA-256 hash from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="bytes">the <see cref="Stream"/></param>
        /// <returns>SHA-256 hash of the <see cref="Stream"/></returns>
        internal static string SHA256hex(Stream stream)
        {
            using (var sha256 = SHA256.Create())
            {
                stream.Position = 0;
                return SHA256hexString(sha256.ComputeHash(stream));
            }
        }

        private static string SHA256hexString(byte[] hash)
        {
            var hashString = new StringBuilder();
            foreach (var b in hash)
            {
                hashString.AppendFormat("{0:x2}", b);
            }
            return hashString.ToString();
        }

    }
}
