using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class ShippingInfoDTO
    {
        public string LineOne { get; set; } = string.Empty;
        public string LineTwo { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        [DataType(DataType.PostalCode)]
        public int Zip { get; set; }
    }
}
