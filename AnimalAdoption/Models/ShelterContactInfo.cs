using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AnimalAdoption.Models.MyValidation;

namespace AnimalAdoption.Models
{
    public class ShelterContactInfo
    {
        public int ShelterContactInfoId { get; set; }

        [Required(ErrorMessage = "The address must exist!")]
        [MinLength(2, ErrorMessage = "Address cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Address cannot be more than 100!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The phone number must exist!")]
        [RegularExpression(@"^07(\d{8})$", ErrorMessage = "This is not a valid phone number!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The city must exist!")]
        [MinLength(2, ErrorMessage = "City name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "City name cannot be more than 50!")]
        public string City { get; set; }

        [Required(ErrorMessage = "The county must exist!")]
        [MinLength(2, ErrorMessage = "County name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "County name cannot be more than 50!")]
        public string County { get; set; }

        [Required(ErrorMessage = "The email must exist!")]
        public string Email { get; set; }

        public virtual Shelter Shelter { get; set; }
    }
}