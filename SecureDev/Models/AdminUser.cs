using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class AdminUser
    {
        // A model for admin user
        public int ID { get; set; }
        public string Username { get; set; }
        [RegularExpression(@"[0-1]", ErrorMessage = "Enter only 0 or 1 please")]
        public int isAdmin { get; set; }
    }
}