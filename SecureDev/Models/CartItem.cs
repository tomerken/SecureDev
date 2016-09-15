using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class CartItem
    {
        public CartItem (string petName, string petType, int Price)
        {
            this.petName = petName;
            this.petType = petType;
            this.Price = Price;
        }
        public string petName { get; set; }
        public string petType { get; set; }

        public int Price { get; set; }
    }
}