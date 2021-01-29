using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JaegerNetCore
{
    public class HttpClientHelper
    {
        public static async Task<T> ReadAsObjectAsync<T>(HttpContent httpContent)
        {
            var stream = await httpContent.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream,new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public class DataResponse<T>
        {
            public T Data { get; set; }
            public bool Success { get; set; }
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
