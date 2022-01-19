using System;
using System.Collections.Generic;

namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }

        public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();

        public BasketModel(string userName)
        {
            this.UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal sum = 0;
                foreach (var item in Items)
                {
                    sum += item.Price * item.Quantitys;
                }
                return sum;
            }
        }
    }
}
