namespace WooliesXFunctionApp.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Helpers;

    public class ProductService
    {
        private IResourceService resourceService;

        /// <summary>
        /// Constructs a ProductService instance.
        /// </summary>
        /// <param name="resourceService">An instance of a IResourceService to get resources from</param>
        public ProductService(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Returns a product list based on the given sort option.
        /// "Low" - Low to High Price
        /// "High" - High to Low Price
        /// "Ascending" - A - Z sort on the Name
        /// "Descending" - Z - A sort on the Name
        /// "Recommended" - this will call the "shopperHistory" resource to get a list of customers orders and needs to return based on popularity
        /// </summary>
        /// <param name="sortOption">Sort option</param>
        /// <returns>A list of products</returns>
        public List<Product> GetProducts(SortOption sortOption)
        {
            return sortOption == SortOption.Recommended ? this.GetSortedRecomendedProducts() : this.resourceService.GetProducts().Sort(sortOption);
        }

        /// <summary>
        /// Returns the lowest possible total based on provided lists of prices, specials and quantities.
        /// </summary>
        /// <param name="input">An instance of TrolleyCalculatorInput with prices, specials and quantities</param>
        /// <returns>The lowest possible total as decimal</returns>
        public decimal GetMinPriceAsDecimal(TrolleyCalculatorInput input) => (decimal)this.GetMinPrice(input).Total;

        /// <summary>
        /// Returns the lowest possible total based on provided lists of prices, specials and quantities.
        /// </summary>
        /// <param name="input">An instance of TrolleyCalculatorInput with prices, specials and quantities</param>
        /// <returns>An instance of TrolleyCalculatorOutput with the lowest possible total</returns>
        public TrolleyCalculatorOutput GetMinPrice(TrolleyCalculatorInput input)
        {
            var maxPrice = this.GetMaxPrice(input);
            if (maxPrice == 0)
            {
                return this.CreateTrolleyCalculatorOutput(0);
            }

            var specials = this.GetApplicableSpecials(input, maxPrice);
            var shoppedProducts = this.GetShoppedProducts(input);
            var minPrice = maxPrice;
            var i = 0;
            var minPriceUpdated = false;
            while (!minPriceUpdated)
            {
                minPriceUpdated = false;
                for (var j = 0; j < specials.Count; ++j)
                {
                    GetMinPrice(input, maxPrice, specials, shoppedProducts, ref minPrice, i, ref minPriceUpdated, j);
                }

                i++;
            }

            return this.CreateTrolleyCalculatorOutput(minPrice);
        }

        private void GetMinPrice(TrolleyCalculatorInput input, float maxPrice, List<Special> specials, Dictionary<string, long> shoppedProducts, ref float minPrice, int i, ref bool minPriceUpdated, int j)
        {
            var specialCounts = new List<int>();
            specialCounts.InitList(i, specials.Count);
            var more = true;
            for (var k = j; more; ++k)
            {
                int oldValue = specialCounts[j];
                specialCounts[j] = oldValue + k;
                if (this.IsApplicable(shoppedProducts, specials, specialCounts))
                {
                    var priceForSpecials = this.GetPriceForSpecials(specials, specialCounts);
                    if (maxPrice > priceForSpecials)
                    {
                        var priceForRest = this.GetPriceForNonSpecials(input.Products, specials, specialCounts, shoppedProducts);
                        var price = priceForSpecials + priceForRest;
                        if (price < minPrice)
                        {
                            minPrice = price;
                            minPriceUpdated = true;
                        }

                        specialCounts[j] = oldValue;
                    }
                }
                else
                {
                    more = false;
                }
            }
        }

        private TrolleyCalculatorOutput CreateTrolleyCalculatorOutput(float total)
        {
            return new TrolleyCalculatorOutput()
            {
                Total = total
            };
        }

        /// <summary>
        /// The method returns a summarised shopper history. In other words this method returns a list of 
        /// products with total quantities from the history.
        /// </summary>
        /// <returns>A list of products</returns>
        private List<Product> GetSortedRecomendedProducts()
        {
            var shopperHistories = this.resourceService.GetShopperHistory();
            var productsDict = new Dictionary<string, SoldProduct>();

            foreach (ShopperHistory shopperHistory in shopperHistories)
            {
                foreach (Product product in shopperHistory.Products)
                {
                    if (productsDict.ContainsKey(product.Name))
                    {
                        productsDict[product.Name].Quantity += product.Quantity;
                        productsDict[product.Name].SoldCount++;
                    }
                    else
                    {
                        productsDict.Add(product.Name, new SoldProduct(product, 1));
                    }
                }
            }

            return productsDict.Values.ToList().OrderByDescending(p => p.SoldCount).ThenByDescending(p => p.Quantity).Select(p => p.ToProduct()).ToList();
        }

        private float GetMaxPrice(TrolleyCalculatorInput input)
        {
            float price = 0f;
            foreach (Product quantity in input.Quantities)
            {
                foreach (Product product in input.Products)
                {
                    if (product.Name == quantity.Name)
                    {
                        price += quantity.Quantity * product.Price;
                        break;
                    }
                }
            }

            return price;
        }

        private List<Special> GetApplicableSpecials(TrolleyCalculatorInput input, float maxPrice)
        {
            // The price is less than the max price when buying the individual items.
            var result = input.Specials.Where(s => s.Total < maxPrice).ToList();
            
            // Should not to have any products that are not in the shopping cart.
            var shoppedProductNames = input.Quantities.Select(q => q.Name).ToList();
            result = result.Where(s => s.Quantities.All(q => shoppedProductNames.Contains(q.Name))).ToList();
            
            // Should not to have more products than the requested quantity.
            var shoppedProductQuntities = input.Quantities.ToDictionary(q => q.Name, q => q.Quantity);
            return result.Where(s => s.Quantities.All(q => { shoppedProductQuntities.TryGetValue(q.Name, out long v); return v >= q.Quantity; })).ToList();
        }

        private Dictionary<string, long> GetShoppedProducts(TrolleyCalculatorInput input)
        {
            return input.Quantities.ToDictionary(q => q.Name, q => q.Quantity);
        }

        private float GetPriceForNonSpecials(List<Product> products, List<Special> specials, List<int> specialCounts, Dictionary<string, long> shoppedProducts)
        {
            float result = 0f;
            foreach (KeyValuePair<string, long> entry in shoppedProducts)
            {
                float productPrice = this.GetProductPrice(products, entry.Key);
                result += (entry.Value - this.GetQuantityByProduct(specials, specialCounts, entry.Key)) * productPrice;
            }

            return result;
        }

        private float GetProductPrice(List<Product> products, string productName)
        {
            Product product = products.Where(p => p.Name == productName).FirstOrDefault();
            return product == default(Product) ? 0f : product.Price;
        }

        private long GetQuantityByProduct(List<Special> specials, List<int> specialCounts, string productName)
        {
            long quantity = 0;
            int idx = 0;
            foreach (Special special in specials)
            {
                var product = special.Quantities.FirstOrDefault(p => p.Name == productName);
                if (product != null)
                {
                    quantity += product.Quantity * specialCounts[idx];
                }

                idx++;
            }

            return quantity;
        }

        private float GetPriceForSpecials(List<Special> specials, List<int> specialCounts)
        {
            float result = 0f;
            for (int i = 0; i < specialCounts.Count; i++)
            {
                result += specialCounts[i] * specials[i].Total;
            }

            return result;
        }

        private bool IsApplicable(Dictionary<string, long> shoppedProducts, List<Special> specials, List<int> specialCounts)
        {
            foreach (KeyValuePair<string, long> entry in shoppedProducts)
            {
                shoppedProducts.TryGetValue(entry.Key, out long maxCount);
                long count = 0;
                int idx = 0;
                foreach (Special special in specials)
                {
                    var product = special.Quantities.FirstOrDefault(p => p.Name == entry.Key);
                    if (product != null)
                    {
                        count += specialCounts[idx] * product.Quantity;
                    }

                    idx++;
                }

                if (count > maxCount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
