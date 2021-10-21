using AvansFysioOpdrachtIndividueel.Models;
using Core.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientTreatmentPlanModelsController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public PatientTreatmentPlanModelsController(IPatientRepo repo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITherapistRepo therapistRepo)
        {
            _patientRepo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _therapistRepo = therapistRepo;
        }
        // GET: PatientTreatmentPlanModelsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientTreatmentPlanModelsController/Create
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: PatientTreatmentPlanModelsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TreatmentPlanModel collection, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            PatientModel patient = _patientRepo.Get(id);

            patient.PatientDossier.TreatmentPlan = new TreatmentPlanModel();

            patient.PatientDossier.TreatmentPlan.TimeOfTreatment = collection.TimeOfTreatment;
            patient.PatientDossier.TreatmentPlan.AmountOfTreaments = collection.AmountOfTreaments;

            _patientRepo.Update(patient, id);

            return RedirectToAction("Details", "PatientModels", new { id });
        }

        // GET: PatientTreatmentPlanModelsController/Edit/5
        public ActionResult Edit(int id)
        {
            var treatmentPlan = _patientRepo.Get(id).PatientDossier.TreatmentPlan;
            return View(treatmentPlan);
        }

        // POST: PatientTreatmentPlanModelsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TreatmentPlanModel collection)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            PatientModel patient = _patientRepo.Get(id);

            patient.PatientDossier.TreatmentPlan = new TreatmentPlanModel();

            patient.PatientDossier.TreatmentPlan.TimeOfTreatment = collection.TimeOfTreatment;
            patient.PatientDossier.TreatmentPlan.AmountOfTreaments = collection.AmountOfTreaments;

            _patientRepo.Update(patient, id);

            return RedirectToAction("Details", "PatientModels", new { id });
        }
    }
}
