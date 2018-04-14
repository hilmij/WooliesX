namespace WooliesXFunctionApp.Service
{
    using System.Collections.Generic;
    using WooliesXFunctionApp.Entity;

    public interface IResourceService
    {
        List<Product> GetProducts();

        List<ShopperHistory> GetShopperHistory();
    }
}
