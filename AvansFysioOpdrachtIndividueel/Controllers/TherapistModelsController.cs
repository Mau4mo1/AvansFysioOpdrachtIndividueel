using AvansFysioOpdrachtIndividueel.Models;
using Core.DomainServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class TherapistModelsController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public TherapistModelsController(IPatientRepo repo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITherapistRepo therapistRepo)
        {
            _patientRepo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _therapistRepo = therapistRepo;
        }
        // GET: TherapistModelsController
        [Authorize(Roles = "Therapist")]
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound();
            }

            // Get the therapist that belongs to the user.
            PersonModel therapist = _therapistRepo.Get().Where(t => t.Email == user.Email).FirstOrDefault();

            List<PatientModel> patients = _patientRepo.Get();
            List<TreatmentViewModel> viewModel = new List<TreatmentViewModel>();

            // now get the therapist.
            foreach (PatientModel pat in patients)
            {
                // Find all treatments that belong to this therapist and that have a treatment today.
                List<TreatmentModel> treatmentModels = pat.PatientDossier.Treatments.
                    Where(t => t.TreatmentDoneBy.Id == therapist.Id && t.TreatmentTime.Date == DateTime.Today.Date).ToList();
                foreach (var treatment in treatmentModels)
                {
                    viewModel.Add(new TreatmentViewModel
                    {
                        Name = pat.Name,
                        treatment = treatment
                    });
                }
            }
            
            return View(viewModel.OrderBy(t => t.treatment.TreatmentTime));
        }

        // The ID here should be the ID of the therapist.
        // GET: TherapistModelsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
