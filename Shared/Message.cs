using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 30 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 30 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required, Phone(ErrorMessage = "Please Enter a Valid Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public bool Read { get; set; } = false;

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is Required")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Subject should be between 1 and 60 characters")]
        public string Subject { get; set; } = string.Empty;

    }
}
