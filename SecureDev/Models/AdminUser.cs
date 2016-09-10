using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class AdminUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int isAdmin { get; set; }
    }
}