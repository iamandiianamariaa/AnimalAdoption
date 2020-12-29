using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalAdoption.Models;

namespace AnimalAdoption.Controllers
{
    public class ShelterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Shelter> shelters = db.Shelters.ToList();
            ViewBag.Shelters = shelters;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Shelter shelter = db.Shelters.Find(id);
                if (shelter != null)
                {
                    return View(shelter);
                }
                return HttpNotFound("Couldn't find the shelter with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing shelter id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            ShelterContactViewModel shelter = new ShelterContactViewModel();
            return View(shelter);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(ShelterContactViewModel shelterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShelterContactInfo contact = new ShelterContactInfo
                    {
                        Address = shelterViewModel.Address,
                        PhoneNumber = shelterViewModel.PhoneNumber,
                        City = shelterViewModel.City,
                        County = shelterViewModel.County,
                        Email = shelterViewModel.Email
                    };
                    db.ShelterContactInfos.Add(contact);
                    Shelter shelter = new Shelter
                    {
                        ShelterName = shelterViewModel.ShelterName,
                        ShelterContactInfo = contact
                    };
                    db.Shelters.Add(shelter);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Shelter");
                }
                return View(shelterViewModel);
            }
            catch (Exception e)
            {
                return View(shelterViewModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Shelter shelter = db.Shelters.Find(id);
            ShelterContactInfo contact = db.ShelterContactInfos.Find(shelter.ShelterContactInfo.ShelterContactInfoId);
            if (shelter != null)
            {
                db.Shelters.Remove(shelter);
                db.ShelterContactInfos.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the shelter with id " + id.ToString());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Shelter shelter = db.Shelters.Find(id);

                if (shelter == null)
                {
                    return HttpNotFound("Couldn't find the shelter with id " + id.ToString() + "!");
                }
                return View(shelter);
            }
            return HttpNotFound("Couldn't find the shelter with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Shelter shelterRequestor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Shelter shelter = db.Shelters.Find(id);
                    if (TryUpdateModel(shelter))
                    {
                        shelter.ShelterName = shelterRequestor.ShelterName;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(shelterRequestor);
            }
            catch (Exception e)
            {
                return View(shelterRequestor);
            }
        }
    }
}