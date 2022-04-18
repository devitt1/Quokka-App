using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TheQDeviceConnect.Core.Rest.Interfaces
{
    public interface IRestClient
    {
        Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method,
            object data = null, bool useHotspot = true,
            string resolved_ip_address = null) where TResult : class;

        Task<bool> GetInternetReachability();
        string BaseEndPoint { get; }
    }
}
