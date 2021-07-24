using System;
using System.Net.Http;
using System.Text;

namespace mossrecru.Utilities
{
    public class Functions
    {
        internal static StringContent Stringify(dynamic values)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(values);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
