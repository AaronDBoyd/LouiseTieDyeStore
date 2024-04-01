using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class CheckoutDTO
    {
        public List<CheckoutItem> Items { get; set; } = new List<CheckoutItem>();
        public decimal SalesTax { get; set; }
        public decimal ShippingCost { get; set; }
        public string UserEmail { get; set; } = string.Empty;
    }

    public partial class CheckoutItem
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
