namespace WooliesXFunctionTest.Service
{
    using System.Collections.Generic;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Service;

    public class MockResourceService : IResourceService
    {
        public const string ProductNameA = "Test Product A";
        public const string ProductNameB = "Test Product B";
        public const string ProductNameC = "Test Product C";
        public const string ProductNameD = "Test Product D";
        public const string ProductNameE = "Test Product E";
        public const string ProductNameF = "Test Product F";

        public List<Product> GetProducts() => new List<Product>()
            {
                new Product { Name = ProductNameA, Price = 99.99F, Quantity = 0 },
                new Product { Name = ProductNameB, Price = 101.99F, Quantity = 0 },
                new Product { Name = ProductNameC, Price = 10.99F, Quantity = 0 },
                new Product { Name = ProductNameD, Price = 5F, Quantity = 0 },
                new Product { Name = ProductNameF, Price = 999999999999F, Quantity = 0 }
            };

        public List<ShopperHistory> GetShopperHistory() => new List<ShopperHistory>()
            {
                new ShopperHistory
                {
                    CustomerId = 123,
                    Products = new List<Product> ()
                    {
                        new Product { Name = ProductNameA, Price = 99.99F, Quantity = 3 },
                        new Product { Name = ProductNameB, Price = 101.99F, Quantity = 1 },
                        new Product { Name = ProductNameF, Price = 999999999999F, Quantity = 1 }
                    }
                },
                new ShopperHistory
                {
                    CustomerId = 23,
                    Products = new List<Product> ()
                    {
                        new Product { Name = ProductNameA, Price = 99.99F, Quantity = 2 },
                        new Product { Name = ProductNameB, Price = 101.99F, Quantity = 3 },
                        new Product { Name = ProductNameF, Price = 999999999999F, Quantity = 1 }
                    }
                },
                new ShopperHistory
                {
                    CustomerId = 23,
                    Products = new List<Product> ()
                    {
                        new Product { Name = ProductNameC, Price = 10.99F, Quantity = 2 },
                        new Product { Name = ProductNameF, Price = 999999999999F, Quantity = 2 }
                    }
                },
                new ShopperHistory
                {
                    CustomerId = 23,
                    Products = new List<Product> ()
                    {
                        new Product { Name = ProductNameA, Price = 99.99F, Quantity = 1 },
                        new Product { Name = ProductNameB, Price = 101.99F, Quantity = 1 },
                        new Product { Name = ProductNameC, Price = 10.99F, Quantity = 1 }
                    }
                },
            };
    }
}
