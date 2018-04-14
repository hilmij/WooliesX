namespace WooliesXFunctionApp.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using WooliesXFunctionApp.Entity;

    public static class ProductExtensions
    {
        public static List<Product> Sort(this List<Product> products, SortOption sortOption)
        {
            if (products == null || products.Count == 0)
            {
                return products;
            }

            List<Product> result;
            switch (sortOption)
            {
                case SortOption.High:
                    result = products.OrderByDescending(o => o.Price).ToList();
                    break;
                case SortOption.Low:
                    result = products.OrderBy(o => o.Price).ToList();
                    break;
                case SortOption.Ascending:
                    result = products.OrderBy(o => o.Name).ToList();
                    break;
                case SortOption.Descending:
                    result = products.OrderByDescending(o => o.Name).ToList();
                    break;
                case SortOption.Recommended:
                    result = products.OrderByDescending(o => o.Quantity).ToList();
                    break;
                default:
                    result = products;
                    break;
            }

            return result;
        }
    }
}
