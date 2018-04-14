namespace WooliesXFunctionApp.Util
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Newtonsoft.Json;
    using WooliesXFunctionApp.Core;

    public class HttpResponseBuilder<T>
    {
        public static HttpResponseMessage Build(T value)
        {
            var json = JsonConvert.SerializeObject(value);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, MediaType.ApplicationJson)
            };
        }
    }
}
