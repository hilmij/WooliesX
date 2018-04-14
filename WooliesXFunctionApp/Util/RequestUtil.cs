namespace WooliesXFunctionApp.Util
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Newtonsoft.Json;
    using WooliesXFunctionApp.Entity;

    public class RequestUtil
    {
        public static T GetEntity<T>(HttpRequestMessage request)
        {
            return (T)JsonConvert.DeserializeObject(request.Content.ReadAsStringAsync().Result, typeof(T));
        }

        public static SortOption GetSortOption(HttpRequestMessage req)
        {
            if (req.GetQueryNameValuePairs() == null)
            {
                return SortOption.Ascending;
            }

            var sortOptionInput = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "sortOption", true) == 0)
                .Value;
            var sortOption = ParseEnum<SortOption>(sortOptionInput);
            return Enum.IsDefined(typeof(SortOption), sortOption) ? sortOption : SortOption.Ascending;
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
    }
}
