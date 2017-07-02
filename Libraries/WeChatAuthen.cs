using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace EverStore.Libraries
{
    public class WeChatAuthen
    {
        public static async Task<Dictionary<string, string>> TouchServer(String uri)
        {
            var client = new HttpClient();

            var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var streamTask = client.GetStreamAsync(uri);
            return serializer.ReadObject(await streamTask) as Dictionary<string, string>;
        }
    }
}