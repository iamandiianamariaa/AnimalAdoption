using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimalAdoption.Models
{
    public class Adoption
    {
        public int AdoptionId { get; set; }

        // one-to-many relationship
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        // one-to-many relationship
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}