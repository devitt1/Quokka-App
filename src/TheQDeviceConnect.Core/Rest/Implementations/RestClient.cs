using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Rest.Interfaces;

namespace TheQDeviceConnect.Core.Rest.Implementations
{
    public class RestClient : IRestClient
    {
        public RestClient()
        {
            _device_local_ip_address = DEVICE_LOCAL_GATEWAY_IP_ADDRESS;
            BaseEndPoint = "http://" + _device_local_ip_address + ":" + PORT + "/";
            Client = new HttpClient();
        }

        private static string DEVICE_LOCAL_GATEWAY_IP_ADDRESS = "192.168.4.1";
        private static string DEVICE_LOCAL_HOSTNAME = "theqbox1.local";
        private static string PORT = "5001";
        private HttpClient _client;
        public HttpClient Client
        {
            get => _client;
            set
            {
                if (_client == null)
                {
                    _client = value;
                }
            }

        }
        private string _device_local_ip_address = "";

        public string BaseEndPoint { get; private set; }


        public async Task<bool> GetInternetReachability(string url)
        {
            try
            {
                var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = HttpMethod.Get };
                HttpResponseMessage response = new HttpResponseMessage();
                response = await Client.SendAsync(request).ConfigureAwait(true);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                {
                    DebugHelper.Error(this, ex);
                    return false;
                }
                throw new Exception("Error getting checking device internet reachability", ex);
            }
        }

        public async Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method, object data = null, bool useHotspot = true, string resolved_ip_address = null) where TResult : class
        {
            if (useHotspot)
            {
                string baseEndPoint = "http://" + DEVICE_LOCAL_GATEWAY_IP_ADDRESS + ":" + PORT + "/";
                url = baseEndPoint + url;
            }
            else
            {
                _device_local_ip_address = resolved_ip_address;
                BaseEndPoint = "http://" + _device_local_ip_address + ":" + PORT + "/";
                url = BaseEndPoint + url;
            }

            DebugHelper.Info(this, $"MakeApi call to {url}");

            var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = method };


            if (method != HttpMethod.Get)
            {
                var json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await Client.SendAsync(request).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                DebugHelper.Error(this, ex);
            }

            var stringSerialized = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                DebugHelper.Error(this, $"ERROR ON REQUEST {stringSerialized}");
            }
            else
            {
                try
                {
                    var conversion = JsonConvert.DeserializeObject<TResult>(stringSerialized);
                    return conversion;
                }
                catch (Exception e)
                {
                    DebugHelper.Error(this, $"DeserializedObject failed {e}");
                }
            }
            return null;
            
        }
    }
}
