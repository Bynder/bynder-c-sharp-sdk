// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace Bynder.Sdk.Exceptions
{
    /// <summary>
    /// Invalid configuration exception. Raised when an invalid configuration
    /// is passed to <see cref="BynderClient"/>.
    /// </summary>
    public class InvalidConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Reason of the exception.</param>
        public InvalidConfigurationException(string message)
            : base(message)
        {
        }
    }
}
