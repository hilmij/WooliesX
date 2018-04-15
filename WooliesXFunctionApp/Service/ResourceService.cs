namespace WooliesXFunctionApp.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WooliesXFunctionApp.Core;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Exception;
    using WooliesXFunctionApp.Util;

    public class ResourceService : IResourceService
    {
        private const string UrlPathBase = "api/resource/";
        private const string UrlPathProducts = UrlPathBase + "products";
        private const string UrlPathShopperHistory = UrlPathBase + "shopperHistory";

        public List<Product> GetProducts()
        {
            var result = this.GetResource<List<Product>>(UrlPathProducts).Result;
            if (result == default(List<Product>))
            {
                throw new CannotGetResourceException("Failed to get the products.");
            }

            return result;
        }

        public List<ShopperHistory> GetShopperHistory()
        {
            var result = this.GetResource<List<ShopperHistory>>(UrlPathShopperHistory).Result;
            if (result == default(List<ShopperHistory>))
            {
                throw new CannotGetResourceException("Failed to get the shopper history.");
            }

            return result;
        }

        private async Task<T> GetResource<T>(string path)
        {
            return await HttpClient.Get<T>(
                AppConstants.UrlResourceBase, 
                path,
                new Dictionary<string, string>
                {
                    { AppConstants.UrlParamToken, AppConstants.Token }
                });
        }
    }
}
