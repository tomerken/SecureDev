using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Vladi2.Models
{
    
    public class Information
    {
        // A model for a user information page
        public Information(int Id, string petName, string petType, decimal Price)
        {
            this.ID = Id;
            this.PetName = petName;
            this.PetType = petType;
            this.PetPrice = Price;
        }
        public int ID { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public decimal PetPrice { get; set; }

    }
}