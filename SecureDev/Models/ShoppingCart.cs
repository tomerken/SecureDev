using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vladi2.Models
{

    public class ShoppingCart
    {
        // A model for a shopping cart (for the shop page)
        public IEnumerable<SelectListItem> PetType { get; set; }
        public string SelectPetType { get; set; }
        public IEnumerable<SelectListItem> PetName { get; set; }
        public string SelectPetName { get; set; }
        public string Price { get; set; }
        public string SelectPrice { get; set; }
    }
}