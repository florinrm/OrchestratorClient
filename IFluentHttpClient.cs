using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrchestratorClient
{
    public interface IFluentHttpClient
    {
        IFluentHttpClient WithHeaders(Dictionary<string, string> headers);
        IFluentHttpClient WithBasicAuthentication(string tenantName, string username, string password);
        IFluentHttpClient WithRelativeUrl(string url);

        Task<T> Get<T>(Uri url, CancellationToken ct = default) where T : class;
        Task<List<T>> GetList<T>(Uri url, CancellationToken ct = default) where T : class;
        Task<TResponse> Post<TRequest, TResponse>(Uri url, TRequest body, CancellationToken ct = default)
            where TRequest : class
            where TResponse : class;

        Task<string> LoginAsync();
    }
}
