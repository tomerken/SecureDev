using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class ChangePassword
    {
        // A model that represents a change password scenario
        [Required(ErrorMessage = "Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "Password length must be at least 5 characters and at most 12 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirmation Password")]
        public string ComparePassword { get; set; }
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "Password length must be at least 5 characters and at most 12 characters")]
        public string OldPassword { get; set; }
    }
}