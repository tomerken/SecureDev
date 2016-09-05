using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Vladi2.Models
{
    
    public class User
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Username is a required field")]
        public string Username { get; set; }
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Password is a required field")]
        [DataType(DataType.Password)]
        [StringLength(12,MinimumLength = 5, ErrorMessage = "Password length must be at least 5 characters and at most 12 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        [Display(Name ="Confirmation Password")]
        public string ComparePassword { get; set; }
        [Required(ErrorMessage = "First name is a required field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only in name please")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is a required field")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only in name please")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is a required field")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }  
        public string Phone { get; set; }
        public string Picture { get; set; }

        

    }
}