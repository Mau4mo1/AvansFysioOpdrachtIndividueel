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
        private readonly IRepo<StudentModel> _studentRepo;
        private readonly IRepo<TeacherModel> _teacherRepo;
        public TherapistModelsController(IRepo<PatientModel> repo, IRepo<StudentModel> studentRepo, IRepo<TeacherModel> teacherRepo)
        {
            _patientRepo = repo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
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
            // Find all patients that belong to this therapist and that have a treatment today.
            List<PatientModel> patients = _patientRepo.Get();
            List<PatientTreatmentViewModel> viewModel = new List<PatientTreatmentViewModel>();
            // TODO:: is there a better way to do this?
            foreach(PatientModel pat in patients)
            {
                List<TreatmentModel> treatmentModels = pat.PatientDossier.Treatments.
                    Where(t => t.TreatmentDoneBy.Id == id && t.TreatmentTime.Date == DateTime.Today.Date).ToList();
                if(treatmentModels.Count > 0)
                {
                    viewModel.Add(new PatientTreatmentViewModel
                    {
                        Name = pat.Name,
                        Id = pat.Id,
                        Treatments = treatmentModels
                    });
                }    
            }

            return View(viewModel);
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
