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
            return Build(HttpStatusCode.OK, JsonConvert.SerializeObject(value));
        }

        public static HttpResponseMessage Build(HttpStatusCode code, string message)
        {
            return new HttpResponseMessage(code)
            {
                Content = new StringContent(message, Encoding.UTF8, MediaType.ApplicationJson)
            };
        }
    }
}
