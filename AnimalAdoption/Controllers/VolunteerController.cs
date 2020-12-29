using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalAdoption.Models;
using System.Data.Entity;

namespace AnimalAdoption.Controllers
{
    public class VolunteerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Volunteer> volunteers = db.Volunteers.ToList();
            ViewBag.Volunteers = volunteers;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Volunteer volunteer = db.Volunteers.Find(id);
                if (volunteer != null)
                {
                    return View(volunteer);
                }
                return HttpNotFound("Couldn't find the volunteer with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing volunteer id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            Volunteer volunteer = new Volunteer();
            return View(volunteer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Volunteer volunteer)
        {
            try
            {
                Shelter shelter = (Shelter)db.Shelters.Include(ceva => ceva.Volunteers);

                if (ModelState.IsValid)
                {
                    db.Volunteers.Add(volunteer);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Volunteer");
                }
                return View(volunteer);
            }
            catch (Exception e)
            {
                return View(volunteer);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer != null)
            {
                db.Volunteers.Remove(volunteer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the volunteer with id " + id.ToString());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Volunteer volunteer = db.Volunteers.Find(id);

                if (volunteer == null)
                {
                    return HttpNotFound("Couldn't find the volunteer with id " + id.ToString() + "!");
                }
                return View(volunteer);
            }
            return HttpNotFound("Couldn't find the volunteer with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Volunteer volunteerRequestor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Volunteer volunteer = db.Volunteers.Find(id);
                    if (TryUpdateModel(volunteer))
                    {
                        volunteer.VolunteerName = volunteerRequestor.VolunteerName;
                        volunteer.Age = volunteerRequestor.Age;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(volunteerRequestor);
            }
            catch (Exception e)
            {
                return View(volunteerRequestor);
            }
        }
    }
}