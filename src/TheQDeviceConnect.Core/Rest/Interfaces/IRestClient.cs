using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheQDeviceConnect.Core.Rest.Implementations;

namespace TheQDeviceConnect.Core.Rest.Interfaces
{
    public interface IRestClient
    {
        Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method,
            object data = null, bool useHotspot = true,
            string resolved_ip_address = null,
            HttpRequestConfig config = null) where TResult : class;

        Task<bool> GetInternetReachability(string url);
        string BaseEndPoint { get; }
    }

    public interface IHttpRequestConfig
    {
        TimeSpan timeout { get; set; }
    }

}

