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
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(uploadRequest.MultipartParams.AWSAccessKeyid), "x-amz-credential");
                    formData.Add(new StringContent(finalKey), "key");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.Policy), "Policy");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.Signature), "X-Amz-Signature");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.Acl), "acl");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.Algorithm), "x-amz-algorithm");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.Date), "x-amz-date");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.SuccessActionStatus), "success_action_status");
                    formData.Add(new StringContent(uploadRequest.MultipartParams.ContentType), "Content-Type");
                    formData.Add(new StringContent(filename), "name");
                    formData.Add(new StringContent(chunkNumber.ToString()), "chunk");
                    formData.Add(new StringContent(numberOfChunks.ToString()), "chunks");
                    formData.Add(new StringContent(finalKey), "Filename");
                    formData.Add(new ByteArrayContent(fileContent, 0, numberOfBytes), "file");

                    using (var response = await client.PostAsync(awsBucket, formData).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }
    }
}
