using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AnimalAdoption.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using AnimalAdoption.Models.MyValidation;

namespace AnimalAdoption.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "The first name must exist!")]
        [MinLength(2, ErrorMessage = "First name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "First name cannot be more than 50!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name must exist!")]
        [MinLength(2, ErrorMessage = "Last name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "Last name cannot be more than 50!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The city must exist!")]
        [MinLength(2, ErrorMessage = "City name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "City name cannot be more than 50!")]
        public string City { get; set; }

        [Required(ErrorMessage = "The county must exist!")]
        [MinLength(2, ErrorMessage = "County name cannot be less than 2!"),
        MaxLength(200, ErrorMessage = "County name cannot be more than 50!")]
        public string County { get; set; }

        [BirthDateValidator]
        [Required(ErrorMessage = "The birth date must exist!")]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Adoption> Adoptions { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Pet> Pets { get; set; }

        public DbSet<Shelter> Shelters { get; set; }

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<Adoption> Adoptions { get; set; }

        public DbSet<ShelterContactInfo> ShelterContactInfos { get; set; }

    }

    public class Initp : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    { // custom initializer
        protected override void Seed(ApplicationDbContext ctx)
        {
            ShelterContactInfo contactInfo = new ShelterContactInfo { Address = "Strada Dobrogeanu Gherea", PhoneNumber = "0728355114", City = "Roman", County = "Neamt", Email = "abc@gmail.com" };
            ctx.ShelterContactInfos.Add(contactInfo);

            Shelter s1 = new Shelter { ShelterName = "Happy Paws", ShelterContactInfo=contactInfo };
            ctx.Shelters.Add(s1);
            Volunteer v1 = new Volunteer { VolunteerName="Andrei Stefan", Age=20,Shelters=new List<Shelter> { s1 } };
            ctx.Volunteers.Add(v1);

            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }
}