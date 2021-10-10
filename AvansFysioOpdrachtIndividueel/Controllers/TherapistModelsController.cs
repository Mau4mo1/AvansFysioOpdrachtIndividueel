using AvansFysioOpdrachtIndividueel.Models;
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
        private readonly IRepo<PatientModel> _patientRepo;
        private readonly IRepo<StudentModel> _studentRepo;
        private readonly IRepo<TeacherModel> _teacherRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public TherapistModelsController(IRepo<PatientModel> repo, IRepo<StudentModel> studentRepo, IRepo<TeacherModel> teacherRepo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _patientRepo = repo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
        }
        // GET: TherapistModelsController
        [Authorize(Roles = "Therapist")]
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                return NotFound();
            }

            // Get the therapist that belongs to the user.
            // TODO:: Find a way to get this in one statement
            PersonModel therapist = _teacherRepo.Get().Where(t => t.Email == user.Email).FirstOrDefault();
            if (therapist == null)
            {
                therapist = _studentRepo.Get().Where(t => t.Email == user.Email).FirstOrDefault();
            }
            // Find all patients that belong to this therapist and that have a treatment today.
            List<PatientModel> patients = _patientRepo.Get();
            List<PatientTreatmentViewModel> viewModel = new List<PatientTreatmentViewModel>();
            // TODO:: is there a better way to do this?

            // now get the therapist.
            foreach (PatientModel pat in patients)
            {
                List<TreatmentModel> treatmentModels = pat.PatientDossier.Treatments.
                    Where(t => t.TreatmentDoneBy.Id == therapist.Id && t.TreatmentTime.Date == DateTime.Today.Date).ToList();
                if (treatmentModels.Count > 0)
                {
                    viewModel.Add(new PatientTreatmentViewModel
                    {
                        Name = pat.Name,
                        Id = pat.Id,
                        Treatments = treatmentModels
                    });
                }
            }

            return View();
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
