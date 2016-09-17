using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class Pet
    {
        // A model for a pet
        public int PetID { get; set; }
        public string PetName { get; set; }
        public decimal Price { get; set; }
        public string PetType { get; set; }
    }
}