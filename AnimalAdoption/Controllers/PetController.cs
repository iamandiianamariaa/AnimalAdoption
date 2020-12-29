using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalAdoption.Models;

namespace AnimalAdoption.Controllers
{
    public class PetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Pet> pets = db.Pets.ToList();
            ViewBag.Pets = pets;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Pet pet = db.Pets.Find(id);
                if (pet != null)
                {
                    return View(pet);
                }
                return HttpNotFound("Couldn't find the pet with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing pet id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            Pet pet = new Pet();
            pet.ShelterList = GetAllShelters();
            return View(pet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Pet pet)
        {
            pet.ShelterList = GetAllShelters();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Pets.Add(pet);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Volunteer");
                }
                return View(pet);
            }
            catch (Exception e)
            {
                return View(pet);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Pet pet = db.Pets.Find(id);
            if (pet != null)
            {
                db.Pets.Remove(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the pet with id " + id.ToString());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Pet pet = db.Pets.Find(id);
                pet.ShelterList = GetAllShelters();

                if (pet == null)
                {
                    return HttpNotFound("Couldn't find the pet with id " + id.ToString() + "!");
                }
                return View(pet);
            }
            return HttpNotFound("Couldn't find the pet with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Pet petRequestor)
        {
            petRequestor.ShelterList = GetAllShelters();
            Pet pet = db.Pets.Include("Shelter").SingleOrDefault(b => b.PetId.Equals(id));
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(pet))
                    {
                        pet.PetName = petRequestor.PetName;
                        pet.Color = petRequestor.Color;
                        pet.Gender = petRequestor.Gender;
                        pet.Description = petRequestor.Description;
                        pet.Species = petRequestor.Species;
                        pet.Breed = petRequestor.Breed;
                        pet.ShelterId = petRequestor.ShelterId;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(petRequestor);
            }
            catch (Exception e)
            {
                return View(petRequestor);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllShelters()
        {
            var selectList = new List<SelectListItem>();
            foreach (var shelter in db.Shelters.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = shelter.ShelterId.ToString(),
                    Text = shelter.ShelterName
                });
            }
            return selectList;
        }
    }
}