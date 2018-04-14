namespace WooliesXFunctionApp
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Service;
    using WooliesXFunctionApp.Util;

    public static class WooliesXFunction
    {
        private const string RouteAnswersUser = "answers/user";
        private const string RouteProducts = "products/{sortOption?}";
        private const string RouteTrolleyCalculator = "trolleyCalculator";

        [FunctionName("Answers")]
        public static HttpResponseMessage GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteAnswersUser)]HttpRequestMessage req, TraceWriter log)
        {
            log.Verbose($"HTTP trigger {RouteAnswersUser} function processed a request.");
            return HttpResponseBuilder<User>.Build(new UserService().GetUser());
        }

        [FunctionName("Products")]
        public static HttpResponseMessage GetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteProducts)]HttpRequestMessage req, TraceWriter log)
        {
            log.Verbose($"HTTP trigger {RouteProducts} function processed a request.");
            return HttpResponseBuilder<List<Product>>.Build(new ProductService(new ResourceService()).GetProducts(RequestUtil.GetSortOption(req)));
        }

        [FunctionName("TrolleyCalculator")]
        public static HttpResponseMessage TrolleyCalculator([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteTrolleyCalculator)]HttpRequestMessage req, TraceWriter log)
        {
            log.Verbose($"HTTP trigger {RouteTrolleyCalculator} function processed a request.");
            return HttpResponseBuilder<TrolleyCalculatorOutput>.Build(new ProductService(new ResourceService()).GetMinPrice(RequestUtil.GetEntity<TrolleyCalculatorInput>(req)));
        }
    }
}
