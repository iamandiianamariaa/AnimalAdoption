using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AnimalAdoption.Models
{
    public class Pet
    {
        public int PetId { get; set; }

        [Required(ErrorMessage = "The name must exist")]
        [MinLength(2, ErrorMessage = "Name cannot be less than 2!"),
         MaxLength(200, ErrorMessage = "Name cannot be more than 50!")]
        public string PetName { get; set; }

        [Required(ErrorMessage = "The color must exist")]
        [MinLength(3, ErrorMessage = "Color cannot be less than 3!"),
        MaxLength(50, ErrorMessage = "Color cannot be more than 50!")]
        public string Color { get; set; }

        [Required(ErrorMessage = "The gender must exist")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Gender must be M or F")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "The description must exist")]
        [MinLength(2, ErrorMessage = "Description cannot be less than 2!"),
        MaxLength(5000, ErrorMessage = "Description cannot be more than 5000!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The species must exist")]
        [MinLength(2, ErrorMessage = "Species name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Species name cannot be more than 50!")]
        public string Species { get; set; }

        [Required(ErrorMessage = "The breed must exist")]
        [MinLength(2, ErrorMessage = "Breed name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Breed name cannot be more than 50!")]
        public string Breed { get; set; }

        // one-to-one relationship
        public virtual Adoption Adoption { get; set; }

        [Required(ErrorMessage = "The shelter must be selected")]
        // one-to-many relationship
        public int ShelterId { get; set; }
        public virtual Shelter Shelter { get; set; }

        public IEnumerable<SelectListItem> ShelterList { get; set; }
    }
    public enum Gender
    {
        M,
        F
    }
}