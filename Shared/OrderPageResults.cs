using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class OrderPageResults
    {
        public List<OrderOverviewResponse> Orders { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
