namespace WooliesXFunctionApp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Exception;
    using WooliesXFunctionApp.Service;
    using WooliesXFunctionApp.Util;

    public static class WooliesXFunction
    {
        private const string RouteAnswersUser = "answers/user";
        private const string RouteProducts = "sort/{sortOption?}";
        private const string RouteTrolleyCalculator = "trolleyCalculator";
        private const string RouteTrolleyCalculator2 = "trolleyCalculator/trolleyTotal";

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
            return ExecProductServiceFunc(service => HttpResponseBuilder<List<Product>>.Build(service.GetProducts(RequestUtil.GetSortOption(req))), log);
        }

        [FunctionName("TrolleyCalculator")]
        public static HttpResponseMessage TrolleyCalculator([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteTrolleyCalculator)]HttpRequestMessage req, TraceWriter log)
        {
            log.Verbose($"HTTP trigger {RouteTrolleyCalculator} function processed a request.");
            return ExecProductServiceFunc(service => HttpResponseBuilder<TrolleyCalculatorOutput>.Build(service.GetMinPrice(RequestUtil.GetEntity<TrolleyCalculatorInput>(req))), log);
        }

        /// <summary>
        /// This function is to workaround the issue with the test client. It sends a request to trolleyCalculator/trolleyTotal method.
        /// </summary>
        /// <param name="req">JSON with the valid values</param>
        /// <param name="log">The log instance</param>
        /// <returns>The minimum total</returns>
        [FunctionName("TrolleyCalculator2")]
        public static HttpResponseMessage TrolleyCalculator2([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteTrolleyCalculator2)]HttpRequestMessage req, TraceWriter log)
        {
            log.Verbose($"HTTP trigger {RouteTrolleyCalculator} function processed a request.");
            return ExecProductServiceFunc(service => HttpResponseBuilder<decimal>.Build(service.GetMinPriceAsDecimal(RequestUtil.GetEntity<TrolleyCalculatorInput>(req))), log);
        }

        private static HttpResponseMessage ExecProductServiceFunc(Func<ProductService, HttpResponseMessage> func, TraceWriter log)
        {
            try
            {
                return func.Invoke(new ProductService(new ResourceService()));
            }
            catch (CannotGetResourceException e)
            {
                log.Error(e.Message, e);
                return HttpResponseBuilder<string>.Build(HttpStatusCode.ServiceUnavailable, e.Message);
            }
            catch (System.Exception e)
            {
                log.Error(e.Message, e);
                return HttpResponseBuilder<string>.Build(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
