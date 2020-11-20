// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Model;

namespace Bynder.Sdk.Service.Upload
{
    /// <summary>
    /// Implementation of <see cref="IAmazonApi"/>
    /// </summary>
    internal class AmazonApi : IAmazonApi
    {
        /// <summary>
        /// Check <see cref="IAmazonApi"/> for more information
        /// </summary>
        /// <param name="filename">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="awsBucket">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="uploadRequest">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="chunkNumber">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="fileContent">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="numberOfBytes">Check <see cref="IAmazonApi"/> for more information</param>
        /// <param name="numberOfChunks">Check <see cref="IAmazonApi"/> for more information</param>
        /// <returns>Check <see cref="IAmazonApi"/> for more information</returns>
        public async Task UploadPartToAmazon(string filename, string awsBucket, UploadRequest uploadRequest, uint chunkNumber, byte[] fileContent, int numberOfBytes, uint numberOfChunks)
        {
            var finalKey = string.Format("{0}/p{1}", uploadRequest.MultipartParams.Key, chunkNumber);

            using (var client = new HttpClient())
            {
                var formData = new MultipartFormDataContent
                {
                    { new StringContent(uploadRequest.MultipartParams.AWSAccessKeyid), "x-amz-credential" },
                    { new StringContent(finalKey), "key" },
                    { new StringContent(uploadRequest.MultipartParams.Policy), "Policy" },
                    { new StringContent(uploadRequest.MultipartParams.Signature), "X-Amz-Signature" },
                    { new StringContent(uploadRequest.MultipartParams.Acl), "acl" },
                    { new StringContent(uploadRequest.MultipartParams.Algorithm), "x-amz-algorithm" },
                    { new StringContent(uploadRequest.MultipartParams.Date), "x-amz-date" },
                    { new StringContent(uploadRequest.MultipartParams.SuccessActionStatus), "success_action_status" },
                    { new StringContent(uploadRequest.MultipartParams.ContentType), "Content-Type" },
                    { new StringContent(filename), "name" },
                    { new StringContent(chunkNumber.ToString()), "chunk" },
                    { new StringContent(numberOfChunks.ToString()), "chunks" },
                    { new StringContent(finalKey), "Filename" },
                    { new ByteArrayContent(fileContent, 0, numberOfBytes), "file" }
                };

                var response = await client.PostAsync(awsBucket, formData).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
