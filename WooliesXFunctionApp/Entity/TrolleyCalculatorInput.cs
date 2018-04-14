namespace WooliesXFunctionApp.Entity
{
    using System.Collections.Generic;

    public class TrolleyCalculatorInput
    {
        public List<Product> Products { get; set; }

        public List<Product> Quantities { get; set; }

        public List<Special> Specials { get; set; }
    }
}
