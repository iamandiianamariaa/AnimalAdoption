using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            List<Shelter> shelters = db.Shelters.Include("ShelterContactInfo").ToList();
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
        public ActionResult New()
        {
            ShelterContactViewModel shelter = new ShelterContactViewModel();
            shelter.Shelter = new Shelter();
            shelter.ShelterContactInfo = new ShelterContactInfo();
            return View(shelter);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(ShelterContactViewModel shelterViewModel)
        {
            try
            {
                ModelState.Remove("Shelter.ShelterContactInfo");
                if (ModelState.IsValid)
                {
                    ShelterContactInfo contact = new ShelterContactInfo
                    {
                        Address = shelterViewModel.ShelterContactInfo.Address,
                        PhoneNumber = shelterViewModel.ShelterContactInfo.PhoneNumber,
                        City = shelterViewModel.ShelterContactInfo.City,
                        County = shelterViewModel.ShelterContactInfo.County,
                        Email = shelterViewModel.ShelterContactInfo.Email
                    };
                    db.ShelterContactInfos.Add(contact);
                    Shelter shelter = new Shelter
                    {
                        ShelterName = shelterViewModel.Shelter.ShelterName,
                        ShelterContactInfo = contact
                    };
                    contact.Shelter = shelter;
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
                ShelterContactViewModel contact = new ShelterContactViewModel();
                contact.Shelter = shelter;
                contact.ShelterContactInfo = shelter.ShelterContactInfo;
                return View(contact);
            }
            return HttpNotFound("Couldn't find the shelter with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, ShelterContactViewModel shelterRequestor)
        {
            try
            {
                ModelState.Remove("Shelter.ShelterContactInfo");
                if (ModelState.IsValid)
                {
                    Shelter shelter = db.Shelters.Find(id);
                    if (TryUpdateModel(shelter))
                    {
                        shelter.ShelterName = shelterRequestor.Shelter.ShelterName;
                        db.SaveChanges();
                    }
                    ShelterContactInfo contactInfo = db.ShelterContactInfos.Find(shelter.ShelterContactInfo.ShelterContactInfoId);
                    if (TryUpdateModel(contactInfo))
                    {
                        contactInfo.Address = shelterRequestor.ShelterContactInfo.Address;
                        contactInfo.PhoneNumber = shelterRequestor.ShelterContactInfo.PhoneNumber;
                        contactInfo.City = shelterRequestor.ShelterContactInfo.City;
                        contactInfo.County = shelterRequestor.ShelterContactInfo.County;
                        contactInfo.Email = shelterRequestor.ShelterContactInfo.Email;
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