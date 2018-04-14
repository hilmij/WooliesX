namespace WooliesXFunctionApp.Service
{
    using System.Collections.Generic;
    using WooliesXFunctionApp.Core;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Util;

    public class ResourceService : IResourceService
    {
        private const string UrlPathBase = "api/resource/";
        private const string UrlPathProducts = UrlPathBase + "products";
        private const string UrlPathShopperHistory = UrlPathBase + "shopperHistory";

        public List<Product> GetProducts()
        {
            return this.GetResource<List<Product>>(UrlPathProducts);
        }

        public List<ShopperHistory> GetShopperHistory()
        {
            return this.GetResource<List<ShopperHistory>>(UrlPathShopperHistory);
        }

        private T GetResource<T>(string path)
        {
            return HttpClient.Get<T>(
                AppConstants.UrlResourceBase, 
                path,
                new Dictionary<string, string>
                {
                    { AppConstants.UrlParamToken, AppConstants.Token }
                });
        }
    }
}
