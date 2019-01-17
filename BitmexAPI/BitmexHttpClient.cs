using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BitmexAPI
{
    public class BitmexHttpClient
    {
        HttpClient _httpClient;
        private readonly ExpiresTimeProvider _expiresTimeProvider;
        private readonly SignatureProvider _signatureProvider;
        private const string _secret = "_fzjFhDzQLrizBrWZl7QxK7wBVn-a2HKIrhVQ7baAgmVvzN8";
        private const string _key = "jgfHziVHF4Z46pp7hpqJjTGU";
        public BitmexHttpClient()
        {
            _expiresTimeProvider = new ExpiresTimeProvider();
            _signatureProvider = new SignatureProvider();
            //_httpClient = new HttpClient { BaseAddress = new Uri($"https://{Environments.Values[_bitmexAuthorization.BitmexEnvironment]}") };

            _httpClient = new HttpClient { BaseAddress = new Uri($"https://testnet.bitmex.com") };

            _httpClient.DefaultRequestHeaders.Add("api-key", _key ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/javascript"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));
        }

        public void PlaceTestOrder()
        {
            var order = OrderPOSTRequestParams.CreateSimpleMarket("XBTUSD", 3, OrderSide.Buy);
            var postQueryParams = order as IJsonQueryParams;
            var resultJson = Post("order", postQueryParams).Result;

            var result = Execute(Order.PostOrder, order).Result;
        }

        public class ApiActionAttributes<TParams, TResult>
        {
            public string Action { get; }
            public HttpMethods Method { get; }

            public ApiActionAttributes(string action, HttpMethods method)
            {
                Action = action;
                Method = method;
            }

        }
        public enum HttpMethods
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4
        }

        public static partial class Order
        {
            public static ApiActionAttributes<OrderPOSTRequestParams, OrderDto> PostOrder = new ApiActionAttributes<OrderPOSTRequestParams, OrderDto>("order", HttpMethods.POST);
        }
        
        public async Task<TResult> Execute<TParams, TResult>(ApiActionAttributes<TParams, TResult> apiAction, TParams @params)
        {
            switch (apiAction.Method)
            {
                case HttpMethods.GET:
                    var getQueryParams = @params as IQueryStringParams;
                    return JsonConvert.DeserializeObject<TResult>(
                        await Get(apiAction.Action, getQueryParams));
                case HttpMethods.POST:
                    var postQueryParams = @params as IJsonQueryParams;
                    return JsonConvert.DeserializeObject<TResult>(
                        await Post(apiAction.Action, postQueryParams));
                case HttpMethods.PUT:
                    var putQueryParams = @params as IJsonQueryParams;
                    return JsonConvert.DeserializeObject<TResult>(
                        await Put(apiAction.Action, putQueryParams));
                //case HttpMethods.DELETE:
                //    var deleteQueryParams = @params as IQueryStringParams;
                //    return JsonConvert.DeserializeObject<TResult>(
                //        await Delete(apiAction.Action, deleteQueryParams));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //public Task<string> Delete(string action, IQueryStringParams parameters)
        //{
        //    var query = parameters?.ToQueryString() ?? string.Empty;
        //    var request = new HttpRequestMessage(HttpMethod.Delete, GetUrl(action) + (string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query));

        //    return SendAndGetResponseAsync(request);
        //}

        public Task<string> Post(string action, IJsonQueryParams parameters) => SendAndGetResponseAsync(HttpMethod.Post, action, parameters);

        public Task<string> Put(string action, IJsonQueryParams parameters) => SendAndGetResponseAsync(HttpMethod.Put, action, parameters);


        private Task<string> SendAndGetResponseAsync(HttpMethod method, string action, IJsonQueryParams parameters)
        {
            var content = parameters?.ToJson() ?? string.Empty;
            var url = GetUrl(action);
            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            return SendAndGetResponseAsync(request, content);
        }

        private async Task<string> SendAndGetResponseAsync(HttpRequestMessage request, string @params = null)
        {
            Sign(request, @params);

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseString);
            }

            return responseString;
        }

        private void Sign(HttpRequestMessage request, string @params)
        {
            request.Headers.Add("api-expires", _expiresTimeProvider.Get().ToString());
            request.Headers.Add("api-signature", _signatureProvider.CreateSignature(_secret ?? string.Empty,
                $"{request.Method}{request.RequestUri}{_expiresTimeProvider.Get().ToString()}{@params}"));
        }

        private static string GetUrl(string action) => "/api/v1/" + action;

    }
}
