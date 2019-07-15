using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrchestratorClient
{
    public abstract class FluentHttpClient : IFluentHttpClient
    {
        protected static readonly HttpClient Client = new HttpClient();
        protected Dictionary<string, string> Headers = new Dictionary<string, string>();
        protected string TenancyName, UsernameOrEmailAddress, Password;
        protected static readonly Uri BaseUrl = new Uri("http://localhost:6234");
        protected static Uri FullUrl;

        public IFluentHttpClient WithHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    Headers.Add(pair.Key, pair.Value);
                }
            }
            return this;
        }

        public IFluentHttpClient WithBasicAuthentication(string tenantName, string username, string password)
        {
            TenancyName = tenantName;
            UsernameOrEmailAddress = username;
            Password = password;
            return this;
        }
        public IFluentHttpClient WithRelativeUrl(string url)
        {
            FullUrl = new Uri(BaseUrl, url);
            return this;
        }

        public async Task<T> Get<T>(Uri url, CancellationToken ct = default)
            where T : class
        {
            return await RequestAsync<T>(new Uri(BaseUrl, url), HttpMethod.Get, null, Headers, ct);
        }

        public async Task<TResponse> Post<TRequest, TResponse>(Uri url, TRequest body, CancellationToken ct = default)
            where TRequest : class
            where TResponse : class
        {
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await RequestAsync<TResponse>(url, HttpMethod.Post, httpContent, Headers);
        }

        public async Task<List<T>> GetList<T>(Uri url, CancellationToken ct = default) where T : class
        {
            return await RequestAsync<List<T>>(url, HttpMethod.Get, null, Headers);
        }

        public abstract Task<string> LoginAsync();

        protected async Task<T> RequestAsync<T>(Uri serviceUrl, HttpMethod method, HttpContent content, Dictionary<string, string> headers = null, CancellationToken ct = default)
            where T : class
        {
            var response = await RequestAsync(serviceUrl, method, content, headers, ct);

            return string.IsNullOrEmpty(response) ? null : JsonConvert.DeserializeObject<T>(response);
        }

        protected async Task<string> RequestAsync(Uri serviceUrl, HttpMethod method, HttpContent content, Dictionary<string, string> headers = null, CancellationToken ct = default, bool retryConnect = false)
        {
            using (var requestMessage = new HttpRequestMessage
            {
                Content = content,
                RequestUri = new Uri(BaseUrl, serviceUrl),
                Method = method,
            })
            {
                if (headers != null)
                {
                    foreach (var entry in headers)
                    {
                        requestMessage.Headers.Add(entry.Key, entry.Value);
                    }
                }

                var sendMessage = await Client.SendAsync(requestMessage, ct);

                if (sendMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Page not found");
                }

                if (sendMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (retryConnect == false)
                    {
                        var userLogin = await LoginAsync();
                        return await RequestAsync(serviceUrl, method, content, headers, ct, retryConnect: true);
                    }
                    else
                        return null;
                }

                return await sendMessage.Content.ReadAsStringAsync();
            }
        }

        protected StringContent SerializeContent<T>(T payload)
        {
            var serializedObject = JsonConvert.SerializeObject(payload);

            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
