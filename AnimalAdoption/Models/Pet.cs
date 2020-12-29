using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimalAdoption.Models
{
    public class Pet
    {
        public int PetId { get; set; }

        [MinLength(2, ErrorMessage = "Name cannot be less than 2!"),
         MaxLength(200, ErrorMessage = "Name cannot be more than 50!")]
        public string PetName { get; set; }

        [MinLength(3, ErrorMessage = "Color cannot be less than 3!"),
        MaxLength(50, ErrorMessage = "Color cannot be more than 50!")]
        public string Color { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Gender must be M or F")]
        public Gender Gender { get; set; }

        [MinLength(2, ErrorMessage = "Description cannot be less than 2!"),
        MaxLength(5000, ErrorMessage = "Description cannot be more than 5000!")]
        public string Description { get; set; }

        [MinLength(2, ErrorMessage = "Species name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Species name cannot be more than 50!")]
        public string Species { get; set; }

        [MinLength(2, ErrorMessage = "Breed name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Breed name cannot be more than 50!")]
        public string Breed { get; set; }

        // one-to-many relationship
        public int ShelterId { get; set; }
        public virtual Shelter Shelter { get; set; }
    }
    public enum Gender
    {
        M,
        F
    }
}