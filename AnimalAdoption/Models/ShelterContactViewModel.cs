using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AnimalAdoption.Models.MyValidation;

namespace AnimalAdoption.Models
{
    public class ShelterContactViewModel
    {
        public Shelter Shelter { get; set; }
        [Required]
        public ShelterContactInfo ShelterContactInfo { get; set; }
    }
}