
using Core.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Web
{
    

    public class StandardHttpClient : IHttpClient, IDisposable
    {
        private HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);

        public StandardHttpClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.Timeout = TimeSpan.FromMinutes(30);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var response = await RetrieveGetResponse(uri, authorizationToken, authorizationMethod);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Stream> GetStreamAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var response = await RetrieveGetResponse(uri, authorizationToken, authorizationMethod);
            return await response.Content.ReadAsStreamAsync();
        }
        private Task<HttpResponseMessage> RetrieveGetResponse(string uri, string authorizationToken, string authorizationMethod)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            SetAuthorizationHeader(requestMessage, authorizationToken, authorizationMethod);
            return _client.SendAsync(requestMessage);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri,
            T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            var requestMessage = new HttpRequestMessage(method, uri);
            SetAuthorizationHeader(requestMessage, authorizationToken, authorizationMethod);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");

            if (requestId != null)
            {
                requestMessage.Headers.Add("x-requestid", requestId);
            }

            var response = await _client.SendAsync(requestMessage);

            // raise exception if HttpResponseCode 500
            // needed for circuit breaker to track fails
            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    logger.Debug($"The StatusCode is {response.StatusCode}, the uri is {uri}, the string is {s}");
                }
            }
            catch (Exception e)
            {
                logger.Warn($"An error occurred while execute DoPostPutAsync: {e}");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            SetAuthorizationHeader(requestMessage, authorizationToken, authorizationMethod);

            if (requestId != null)
            {
                requestMessage.Headers.Add("x-requestid", requestId);
            }

            return await _client.SendAsync(requestMessage);
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage, string authorizationToken, string authorizationMethod)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null && _httpContextAccessor.HttpContext.Request.Headers != null)
            {
                var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
                }
            }

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public void SetTimeout(TimeSpan timeout)
        {
            _client.Timeout = timeout;
        }
    }
}