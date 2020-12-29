using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalAdoption.Models
{
    public class Volunteer
    {
        public int VolunteerId { get; set; }

        [MinLength(2, ErrorMessage = "Name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Name cannot be more than 100!")]
        public string VolunteerName { get; set; }

        [Range(16, 70, ErrorMessage = "Age must be between 1 and 70")]
        public int Age { get; set; }

        // many-to-many relationship
        public virtual ICollection<Shelter> Shelters { get; set; }

        // checkboxes list
        [NotMapped]
        public List<CheckBoxViewModel> SheltersList { get; set; }
    }
}