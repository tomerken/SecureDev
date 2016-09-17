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
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 5)]
        public string Password { get; set; }     
    }
}