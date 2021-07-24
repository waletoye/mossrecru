using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace mossrecru.Models
{
    public class Context
    {
        protected HttpClient client;
        readonly string ip;

        public Context()
        {
            ip = Settings.AppSettings.BaseIPAddress;
            client = new HttpClient();
        }


        internal async Task<T> GetRequest<T>(string endPoint, CancellationTokenSource cancellation = null)
        {
            try
            {
                var _uri = ip + endPoint;

                HttpRequestMessage request;

                request = new HttpRequestMessage(new HttpMethod("GET"), _uri);

                HttpResponseMessage response;

                if (cancellation != null)
                    response = await client.SendAsync(request, cancellation.Token);
                else
                    response = await client.SendAsync(request);


                var msg = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }

                return default;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return default;
            }
        }

        internal async Task<T> PostRequest<T>(dynamic values, string endPoint, CancellationTokenSource cancellation = null)
        {
            try
            {
                var _uri = endPoint;

                var request = new HttpRequestMessage(new HttpMethod("POST"), _uri)
                {
                    Content = Utilities.Functions.Stringify(values)
                };


                HttpResponseMessage response;

                if (cancellation != null)
                    response = await client.SendAsync(request, cancellation.Token);
                else
                    response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(content);

                    return JsonConvert.DeserializeObject<T>(content);
                }

                return default;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                return default;
            }
        }
    }
}
