using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Vladi2.Models
{
    
    public class SummeryShop
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is a required field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only in name please")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only in name please")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "ID is a required field")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Id needs to be 9 digits")]
        [Display(Name = "ID")]
        public int UserId { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Credit Number is a required field")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Credit Number length must be at least 8 characters and at most 16 digits")]
        [Display(Name = "Cradit Number")]
        public string CreditNumber { get; set; }


        [Required(ErrorMessage = "Card Number is a required field")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "3 digits in the back of the card")]
        [Display(Name = "3 Digits")]
        public string BehindDigits { get; set; }

        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid phone number with the format XXXXXXXXXX (10 digits)")]
        public string Phone { get; set; }




    }
}