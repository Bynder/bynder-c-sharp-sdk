// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Service.Upload
{
    /// <summary>
    /// Interface to upload file parts to Amazon
    /// </summary>
    internal interface IAmazonApi
    {
        /// <summary>
        /// Uploads a file part to Amazon 
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="awsBucket">AWS bucket with the Url to upload the part to</param>
        /// <param name="uploadRequest">Upload request information</param>
        /// <param name="chunkNumber">chunk number</param>
        /// <param name="fileContent">content to be uploaded</param>
        /// <param name="numberOfBytes">number of bytes to upload</param>
        /// <param name="numberOfChunks">total number of chunks</param>
        /// <returns>Task to represent the upload</returns>
        Task UploadPartToAmazon(string filename, string awsBucket, UploadRequest uploadRequest, uint chunkNumber, byte[] fileContent, int numberOfBytes, uint numberOfChunks);
    }
}