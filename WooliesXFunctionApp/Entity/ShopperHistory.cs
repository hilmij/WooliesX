namespace WooliesXFunctionApp.Entity
{
    using System.Collections.Generic;

    public class ShopperHistory
    {
        public long CustomerId { get; set; }

        public List<Product> Products { get; set; }
    }
}
