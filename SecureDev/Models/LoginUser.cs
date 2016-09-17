using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Vladi2.Models
{
    
    public class LoginUser
    {
        // A model for a login form user
        [Required(ErrorMessage ="Username is a required field")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters and numbers only in username please")]
        [StringLength(12, ErrorMessage = "Username length must be at most 12 characters")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 5)]
        public string Password { get; set; }     
    }
}