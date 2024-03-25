using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{

    // TODO: Change to "ShippingInfo"
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 30 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 30 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required, DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter a Valid Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string LineOne { get; set; } = string.Empty;

        public string LineTwo { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z',.\s-]{1,25}$", ErrorMessage = "Please Enter a Valid City Name")]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{5}$",
            ErrorMessage = "Please Enter a 5 Digit US Postal Code")]
        public string Zip { get; set; }
        //public string Zip { get; set; } = string.Empty;

        public string Country { get; set; } = "US"; // Should this default to "USA"??
    }
}
