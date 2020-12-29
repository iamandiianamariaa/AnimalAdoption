using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalAdoption.Models;

namespace AnimalAdoption.Controllers
{
    public class ShelterContactInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<ShelterContactInfo> shelterContactInfos = db.ShelterContactInfos.ToList();
            ViewBag.ShelterContactInfos = shelterContactInfos;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                ShelterContactInfo shelterContactInfo = db.ShelterContactInfos.Find(id);
                if (shelterContactInfo != null)
                {
                    return View(shelterContactInfo);
                }
                return HttpNotFound("Couldn't find the shelter information with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing shelter information id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            ShelterContactInfo shelterContactInfo = new ShelterContactInfo();
            return View(shelterContactInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(ShelterContactInfo shelterContactInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ShelterContactInfos.Add(shelterContactInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index", "ShelterContactInfo");
                }
                return View(shelterContactInfo);
            }
            catch (Exception e)
            {
                return View(shelterContactInfo);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ShelterContactInfo shelterContactInfo = db.ShelterContactInfos.Find(id);
            if (shelterContactInfo != null)
            {
                db.ShelterContactInfos.Remove(shelterContactInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the shelter info with id " + id.ToString());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                ShelterContactInfo shelterContactInfo = db.ShelterContactInfos.Find(id);

                if (shelterContactInfo == null)
                {
                    return HttpNotFound("Couldn't find the shelter info with id " + id.ToString() + "!");
                }
                return View(shelterContactInfo);
            }
            return HttpNotFound("Couldn't find the shelter info with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, ShelterContactInfo shelterContactInfoRequestor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShelterContactInfo shelterContactInfo = db.ShelterContactInfos.Find(id);
                    if (TryUpdateModel(shelterContactInfo))
                    {
                        shelterContactInfo.Address = shelterContactInfoRequestor.Address;
                        shelterContactInfo.PhoneNumber = shelterContactInfoRequestor.PhoneNumber;
                        shelterContactInfo.City = shelterContactInfoRequestor.City;
                        shelterContactInfo.County = shelterContactInfoRequestor.County;
                        shelterContactInfo.Email = shelterContactInfoRequestor.Email;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(shelterContactInfoRequestor);
            }
            catch (Exception e)
            {
                return View(shelterContactInfoRequestor);
            }
        }
    }
}