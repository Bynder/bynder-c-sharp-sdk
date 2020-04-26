// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Bynder.Api.Queries;

namespace Bynder.Api.Impl.Oauth
{
    /// <summary>
    /// Interface to send oauth <see cref="Request{T}"/> to the API. 
    /// </summary>
    internal interface IOauthRequestSender : IDisposable
    {
        /// <summary>
        /// Sends the request to Bynder API. It gets all necessary information from <see cref="Request{T}"/>
        /// and deserializes response if needed to specific object.
        /// </summary>
        /// <typeparam name="T">Type we want to deserialize response to</typeparam>
        /// <param name="request">Request with the information to do the API call</param>
        /// <returns>Task returning T</returns>
        Task<T> SendRequestAsync<T>(Request<T> request);

        /// <summary>
        /// Sends the request to Bynder API, where query object will be sent in json format. It gets all necessary information from <see cref="Request{T}"/>
        /// and deserializes response if needed to specific object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<T> SendJsonRequestAsync<T>(Request<T> request);
    }
}