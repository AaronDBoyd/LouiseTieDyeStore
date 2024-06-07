using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class ShippingResponse
    {
        public Address VerifiedAddress { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
