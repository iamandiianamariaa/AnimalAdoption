using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AnimalAdoption.Models.MyValidation;

namespace AnimalAdoption.Models
{
    public class Shelter
    {
        public int ShelterId { get; set; }

        [MinLength(2, ErrorMessage = "Shelter name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Shelter name cannot be more than 50!")]
        public string ShelterName { get; set; }

        // one-to one-relationship
        [Required]
        public virtual ShelterContactInfo ShelterContactInfo { get; set; }

        // many-to-one relationship
        public virtual ICollection<Pet> Pets { get; set; }

        // many-to-many relationship
        public virtual ICollection<Volunteer> Volunteers { get; set; }

    }
}