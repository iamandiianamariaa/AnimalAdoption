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
        public ActionResult New()
        {
            Volunteer volunteer = new Volunteer();
            volunteer.SheltersList = GetAllShelters();
            volunteer.Shelters = new List<Shelter>();
            return View(volunteer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Volunteer volunteer)
        {
            var selectedShelters = volunteer.SheltersList.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    volunteer.Shelters = new List<Shelter>();
                    for (int i = 0; i < selectedShelters.Count(); i++)
                    {
                        Shelter shelter = db.Shelters.Find(selectedShelters[i].Id);
                        volunteer.Shelters.Add(shelter);
                    }
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
                volunteer.SheltersList = GetAllShelters();

                foreach (Shelter checkedShelter in volunteer.Shelters)
                {
                    volunteer.SheltersList.FirstOrDefault(g => g.Id == checkedShelter.ShelterId).Checked = true;
                }

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
            Volunteer volunteer = db.Volunteers.Find(id);
            var selectedShelters = volunteerRequestor.SheltersList.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(volunteer))
                    {
                        volunteer.VolunteerName = volunteerRequestor.VolunteerName;
                        volunteer.Age = volunteerRequestor.Age;
                        volunteer.Shelters.Clear();
                        volunteer.Shelters = new List<Shelter>();

                        for (int i = 0; i < selectedShelters.Count(); i++)
                        {
                            Shelter shelter = db.Shelters.Find(selectedShelters[i].Id);
                            volunteer.Shelters.Add(shelter);
                        }
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

        [NonAction]
        public List<CheckBoxViewModel> GetAllShelters()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var shelter in db.Shelters.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = shelter.ShelterId,
                    Name = shelter.ShelterName,
                    Checked = false
                });
            }
            return checkboxList;
        }

    }
}