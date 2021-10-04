using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class TherapistModelsController : Controller
    {
        private readonly IRepo<PatientModel> _patientRepo;

        public TherapistModelsController(IRepo<PatientModel> repo)
        {
            _patientRepo = repo;
        }
        // GET: TherapistModelsController
        public ActionResult Index()
        {
            return View();
        }

        // The ID here should be the ID of the therapist.
        // GET: TherapistModelsController/Details/5
        public ActionResult Details(int id)
        {
            // the ID here is the Therapist ID
            // This method needs to get all the treatments that are assigned to me.
            var patients = _patientRepo.Get().Where(p => p.PatientDossier.Therapist.Id == id).OrderBy(p => p.PatientDossier.PlannedDate);

            return View(patients);
        }

        // GET: TherapistModelsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TherapistModelsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TherapistModelsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TherapistModelsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TherapistModelsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TherapistModelsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
