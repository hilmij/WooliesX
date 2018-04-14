namespace WooliesXFunctionApp.Util
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using WooliesXFunctionApp.Exception;

    public class HttpClient
    {
        private static System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public static T Get<T>(string root, string path, Dictionary<string, string> urlParams)
        {
            var uri = GetUri(root, path, urlParams);
            var response = httpClient.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(json);
            }

            throw new CannotGetResourceException(response.ReasonPhrase);
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
