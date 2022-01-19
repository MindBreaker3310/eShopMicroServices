using System;
namespace Shopping.Aggregator.Models
{
    public class BasketItemExtendedModel
    {
        public int Quantitys { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        //產品相關物件
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
    }
}
