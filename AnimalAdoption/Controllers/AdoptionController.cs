using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AnimalAdoption.Models;
using Microsoft.AspNet.Identity;

namespace AnimalAdoption.Controllers
{
    public class AdoptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Adoption
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Adoption> adoptions = db.Adoptions.Include("Pet").ToList();
            ViewBag.Adoptions = adoptions;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Adoption adoption = db.Adoptions.Find(id);
                if (adoption != null)
                {
                    return View(adoption);
                }
                return HttpNotFound("Couldn't find the adoption with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing adoption id parameter!");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Adoption adoption = db.Adoptions.Find(id);
            if (adoption != null)
            {
                adoption.Pet.IsAdopted = false;
                db.Adoptions.Remove(adoption);
                db.SaveChanges();
                return RedirectToAction("Index","Manage");
            }
            return HttpNotFound("Couldn't find the pet with id " + id.ToString());
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult New(int?id)
        {
           if (id.HasValue)
                {
                Pet pet = db.Pets.FirstOrDefault(p=> id == p.PetId);
                pet.IsAdopted = true;
                Adoption adoption = new Adoption
                {
                    UserId = User.Identity.GetUserId(),
                    Pet = pet
                };
                pet.Adoption = adoption;
                db.Adoptions.Add(adoption);
                db.SaveChanges();
                return RedirectToAction("Index", "Pet");
                }
            return HttpNotFound("Couldn't find the pet with id " + id.ToString() + "!");
        }


    }
}