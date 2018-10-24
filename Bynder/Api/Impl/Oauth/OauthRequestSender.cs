// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Bynder.Api.Queries;
using Newtonsoft.Json;

namespace Bynder.Api.Impl.Oauth
{
    /// <summary>
    /// Implementation for <see cref="IOauthRequestSender"/>
    /// </summary>
    internal class OauthRequestSender : IOauthRequestSender
    {
        /// <summary>
        /// Base Url where API requests will be sent
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Credentials used to generate oauth header
        /// </summary>
        private Credentials _credentials;

        /// <summary>
        /// Query decoder to get parameters from query objects
        /// </summary>
        private QueryDecoder _queryDecoder = new QueryDecoder();

        /// <summary>
        /// HttpClient to send the API requests.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="credentials">Credentials need to authenticate request</param>
        /// <param name="baseUrl">base Url where to send API requests</param>
        public OauthRequestSender(Credentials credentials, string baseUrl)
        {
            _credentials = credentials;
            _baseUrl = baseUrl;
            _httpClient = new HttpClient(new OAuthMessageHandler(credentials, new HttpClientHandler()));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Disposes httpClient instance
        /// </summary>
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        /// Check <see cref="IOauthRequestSender"/>
        /// </summary>
        /// <typeparam name="T">Check <see cref="IOauthRequestSender"/></typeparam>
        /// <param name="request">Check <see cref="IOauthRequestSender"/></param>
        /// <param name="converters">params of custom <see cref="JsonConverter"/> implementations</param>
        /// <returns>Check <see cref="IOauthRequestSender"/></returns>
        public Task<T> SendRequestAsync<T>(Request<T> request, params JsonConverter[] converters)
        {
            var parameters = _queryDecoder.GetParameters(request.Query);
            return BynderRestCallAsync<T>(request.HTTPMethod, request.Uri, parameters, request.DeserializeResponse, converters);
        }

        /// <summary>
        /// Gets response to an API call and deserialize response to object if needed
        /// </summary>
        /// <typeparam name="T">Object type we want to deserialize to</typeparam>
        /// <param name="method">HTTP Method to use (GET, POST,...)</param>
        /// <param name="uri">Uri to be appended to <see cref="_baseUrl"/> to do the request</param>
        /// <param name="requestParams">Parameters to be used for the request</param>
        /// <param name="deserializeToJson">if T is a string we maybe don't want to deserialize to JSON. If T is string and this parameter is false, response is not deserialized</param>
        /// <param name="converters">params of custom <see cref="JsonConverter"/> implementations</param>
        /// <returns>Task with response as T</returns>
        private async Task<T> BynderRestCallAsync<T>(HttpMethod method, string uri, NameValueCollection requestParams, bool deserializeToJson, params JsonConverter[] converters)
        {
            var responseString = await BynderRestCallAsync(method, uri, requestParams).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(responseString))
            {
                if (typeof(T) == typeof(string)
                    && !deserializeToJson)
                {
                    // We can't return responseString directly. 
                    return (T)Convert.ChangeType(responseString, typeof(T));
                }
                if (converters?.Length > 0)
                {
                    var jsonSerializerSettings = new JsonSerializerSettings() {DateParseHandling = DateParseHandling.None,Converters = converters.ToList()};
                    return JsonConvert.DeserializeObject<T>(responseString, jsonSerializerSettings);
                }
                return JsonConvert.DeserializeObject<T>(responseString);
            }

            return default(T);
        }

        /// <summary>
        /// Gets reponse as raw string to an API call
        /// </summary>
        /// <param name="method">HTTP Method to use (GET, POST,...)</param>
        /// <param name="uri">Uri to be appended to <see cref="_baseUrl"/> to do the request</param>
        /// <param name="requestParams">Parameters to be used for the request</param>
        /// <returns>Task returning raw response string</returns>
        private async Task<string> BynderRestCallAsync(HttpMethod method, string uri, NameValueCollection requestParams)
        {
            Uri baseUri = new Uri(_baseUrl);
            Uri uploadUri = new Uri(baseUri, uri);

            using (var httpResponseMessage = await SendOauthRequestAsync(method, uploadUri, requestParams).ConfigureAwait(false))
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return content;
                }

                throw new HttpRequestException(content);
            }
        }

        /// <summary>
        /// Sends an API request adding appropriate Oauth headers. It returns a task with the HttpResponseMessage received.
        /// </summary>
        /// <param name="method">HTTP Method to use (GET, POST,...)</param>
        /// <param name="uri">Uri to be appended to <see cref="_baseUrl"/> to do the request</param>
        /// <param name="requestParams">Parameters to be used for the request</param>
        /// <returns>Task returning HttpResponseMessage received</returns>
        private async Task<HttpResponseMessage> SendOauthRequestAsync(HttpMethod method, Uri uri, NameValueCollection requestParams)
        {
            string parametersUrlEncoded = ConvertToUrlQuery(requestParams);
            uri = new Uri(uri, $"?{parametersUrlEncoded}");
            using (HttpRequestMessage request = new HttpRequestMessage(method, uri))
            {
                if (method == HttpMethod.Post)
                {
                    request.Content = new StringContent(parametersUrlEncoded, Encoding.UTF8, "application/x-www-form-urlencoded");
                }

                // We need to await here so request is not disposed before the call finishes
                return await _httpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Converts parameters collection to Url query string
        /// </summary>
        /// <param name="parameters">parameters collection</param>
        /// <returns>URL Encoded string</returns>
        private string ConvertToUrlQuery(NameValueCollection parameters)
        {
            var encodedValues = parameters.AllKeys
                .OrderBy(key => key)
                .Select(key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(parameters[key])}");
            var queryUri = string.Join("&", encodedValues);

            // We need encoded values to be uppercase.
            return Regex.Replace(queryUri, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
        }
    }
}
