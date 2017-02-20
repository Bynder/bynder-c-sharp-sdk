// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Api
{
    /// <summary>
    /// Exception thrown when Upload does not finish 
    /// within the expected time
    /// </summary>
    [Serializable]
    public class BynderUploadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="message">Message explaining the exception</param>
        public BynderUploadException(string message)
            : base(message)
        {
        }
    }
}
