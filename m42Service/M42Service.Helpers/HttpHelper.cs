using m42Service.Entities;
using m42Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace M42Service.Helpers
{
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            UseCookies = false,
            ConnectTimeout = TimeSpan.FromMinutes(5),
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        })
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        private static readonly JsonSerializerOptions _defaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNameCaseInsensitive = true
        };

        private static readonly File_Logger _logger = File_Logger.GetInstance("HttpHelper");

        public static JsonSerializerOptions DefaultJsonOptions => _defaultJsonOptions;
        public static HttpResponseMessage AuthPost (string url, Dictionary<string,string> body)
        {
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(body)
            };

            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           


            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST request to URL: {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Headers: {string.Join(", ", requestMessage.Headers)}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Body: {JsonSerializer.Serialize(body, DefaultJsonOptions)}");

            var response = _httpClient.Send(requestMessage);
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status Code: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Headers: {string.Join(", ", response.Headers)}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {response.Content.ReadAsStringAsync().Result}");

            return response;
        }

        public static HttpResponseMessage UpdatePost<Tin>(string url, Tin body)
        {
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(body)
            };

            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");



            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST request to URL: {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Headers: {string.Join(", ", requestMessage.Headers)}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Body: {JsonSerializer.Serialize(body, DefaultJsonOptions)}");

            var response = _httpClient.Send(requestMessage);
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status Code: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Headers: {string.Join(", ", response.Headers)}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {response.Content.ReadAsStringAsync().Result}");

            return response;
        }

    }
}
