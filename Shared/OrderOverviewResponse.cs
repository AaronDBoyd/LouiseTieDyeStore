using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class OrderOverviewResponse
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string OrderTitle { get; set; } = string.Empty;
        public string OrderImageUrl { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
    }
}
