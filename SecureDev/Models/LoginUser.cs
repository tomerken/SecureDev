using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Vladi2.Models
{
    
    public class LoginUser
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "Password length must be at least 5 characters and at most 12 characters")]
        public string Password { get; set; }     
    }
}