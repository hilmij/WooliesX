namespace WooliesXFunctionApp.Entity
{
    public class SoldProduct : Product
    {
        public SoldProduct(Product product, long soldCount) : base(product)
        {
            this.SoldCount = soldCount;
        }

        public long SoldCount { get; set; }

        public Product ToProduct()
        {
            return (Product)this;
        }
    }
}
