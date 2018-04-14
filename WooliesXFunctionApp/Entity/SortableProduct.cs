namespace WooliesXFunctionApp.Entity
{
    public class SoldProduct : Product
    {
        public SoldProduct(Product product, long soldCount)
        {
            base.Name = product.Name;
            base.Price = product.Price;
            base.Quantity = product.Quantity;
            this.SoldCount = soldCount;
        }

        public long SoldCount { get; set; }

        public Product ToProduct()
        {
            return (Product) this;
        }
    }
}
