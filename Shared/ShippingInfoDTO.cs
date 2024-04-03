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


        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Please Select State")]
        public string State { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{5}$",
            ErrorMessage = "Please Enter a 5 Digit US Postal Code")]
        public string Zip { get; set; }

        public int ItemCount { get; set; } = 1;
    }
}
