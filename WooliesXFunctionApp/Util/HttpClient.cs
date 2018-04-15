namespace WooliesXFunctionApp.Util
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class HttpClient
    {
        private static System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public static async Task<T> Get<T>(string root, string path, Dictionary<string, string> urlParams)
        {
            var uri = GetUri(root, path, urlParams);
            var response = httpClient.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }

        private static string GetUri(string root, string path, Dictionary<string, string> urlParams)
        {
            var query = string.Empty;
            foreach (KeyValuePair<string, string> entry in urlParams)
            {
                if (query.Length > 0)
                {
                    query += "&";
                }

                query += entry.Key + "=" + entry.Value;
            }

            var builder = new UriBuilder(root)
            {
                Port = -1,
                Path = path,
                Query = query
            };

            return builder.ToString();
        }
    }
}
