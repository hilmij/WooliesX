﻿namespace WooliesXFunctionTest.Service
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WooliesXFunctionApp.Entity;
    using WooliesXFunctionApp.Service;

    [TestClass]
    public class ProductServiceTest
    {
        [TestMethod]
        public void TestGetProducts()
        {
            ProductService service = new ProductService(new MockResourceService());
            this.TestProductNamesOrder(service.GetProducts(SortOption.Recommended), MockResourceService.ProductNameA, MockResourceService.ProductNameC);
            this.TestProductNamesOrder(service.GetProducts(SortOption.Ascending), MockResourceService.ProductNameA, MockResourceService.ProductNameF);
            this.TestProductNamesOrder(service.GetProducts(SortOption.Descending), MockResourceService.ProductNameF, MockResourceService.ProductNameA);
            this.TestProductNamesOrder(service.GetProducts(SortOption.High), MockResourceService.ProductNameF, MockResourceService.ProductNameD);
            this.TestProductNamesOrder(service.GetProducts(SortOption.Low), MockResourceService.ProductNameD, MockResourceService.ProductNameF);
        }

        [TestMethod]
        public void TestGetMinPrice()
        {
            var service = new ProductService(new MockResourceService());
            var input = new TrolleyCalculatorInput()
            {
                Products = new List<Product>()
                {
                    new Product { Name = MockResourceService.ProductNameA, Price = 2 },
                    new Product { Name = MockResourceService.ProductNameB, Price = 3 }
                },
                Quantities = new List<Product>()
                {
                    new Product { Name = MockResourceService.ProductNameA, Quantity = 3 },
                    new Product { Name = MockResourceService.ProductNameB, Quantity = 2 }
                },
                Specials = new List<Special>()
                {
                    new Special
                    {
                        Total = 5,
                        Quantities = new List<Product>()
                        {
                            new Product { Name = MockResourceService.ProductNameA, Quantity = 3 },
                            new Product { Name = MockResourceService.ProductNameB, Quantity = 1 }
                        }
                    },
                    new Special
                    {
                        Total = 10,
                        Quantities = new List<Product>()
                        {
                            new Product { Name = MockResourceService.ProductNameA, Quantity = 1 },
                            new Product { Name = MockResourceService.ProductNameB, Quantity = 2 }
                        }
                    }
                }
            };
            Assert.AreEqual(service.GetMinPrice(input).Total, 8);
            Assert.AreEqual(service.GetMinPriceAsDecimal(input), 8);
        }

        [TestMethod]
        public void TestGetMinPriceWith0()
        {
            var service = new ProductService(new MockResourceService());
            var input = new TrolleyCalculatorInput()
            {
                Products = new List<Product>()
                {
                    new Product { Name = MockResourceService.ProductNameA, Price = 0 }
                },
                Quantities = new List<Product>()
                {
                    new Product { Name = MockResourceService.ProductNameA, Quantity = 0 }
                },
                Specials = new List<Special>()
                {
                    new Special
                    {
                        Total = 5,
                        Quantities = new List<Product>()
                        {
                            new Product { Name = MockResourceService.ProductNameA, Quantity = 0 }
                        }
                    }
                }
            };
            Assert.AreEqual(service.GetMinPrice(input).Total, 0);
            Assert.AreEqual(service.GetMinPriceAsDecimal(input), 0);
        }

        private void TestProductNamesOrder(List<Product> products, string firstExpected, string lastExpected)
        {
            Assert.AreEqual(products[0].Name, firstExpected);
            Assert.AreEqual(products[products.Count - 1].Name, lastExpected);
        }
    }
}
