using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientTreatmentModelsController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly ITreatmentManager _treatmentManager;
        public PatientTreatmentModelsController(IPatientRepo patientRepo, ITherapistRepo therapistRepo, ITreatmentManager treatmentManager)
        {
            _patientRepo = patientRepo;
            _therapistRepo = therapistRepo;
            _treatmentManager = treatmentManager;
        }

        // GET: PatientTreatmentModelsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PatientTreatmentModelsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientTreatmentModelsController/Create/5
        public ActionResult Create(int id)
        {
            TreatmentFormViewModel treatmentFormViewModel = new TreatmentFormViewModel();

            treatmentFormViewModel.Therapists = _therapistRepo.GetTherapists();
            treatmentFormViewModel.Id = id;

            return View(treatmentFormViewModel);
        }

        // POST: PatientTreatmentModelsController/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public ActionResult Create(TreatmentFormViewModel collection, int id)
        {
            if (!ModelState.IsValid)
            {
                collection.Therapists = _therapistRepo.GetTherapists();
                return View(collection);
            }

            if (!_treatmentManager.IsWithinAllowedTreatmentAmount(id, collection.Treatment))
            {
                ModelState.AddModelError(string.Empty, "Je mag niet meer behandelingen aanmaken dan in je behandelplan staat");
                collection.Therapists = _therapistRepo.GetTherapists();
                return View(collection);
            }

            PatientModel patientModel = _patientRepo.Get(id);

            // Fill in the end of the treatment datetime by taking the treatmentplan TimeOfTreatment in minutes and adding it to the starting treatment time.
            collection.Treatment.TreatmentUntil = collection.Treatment.TreatmentTime.AddMinutes(patientModel.PatientDossier.TreatmentPlan.TimeOfTreatment);

            if (!_treatmentManager.IsTherapistAvailable(collection.TherapyDoneById, collection.Treatment))
            {
                ModelState.AddModelError(string.Empty, "De therapeut is niet beschikbaar op deze tijd.");
                collection.Therapists = _therapistRepo.GetTherapists();
                return View(collection);
            }

            collection.Treatment.TreatmentDoneBy = _therapistRepo.Get(collection.TherapyDoneById);

            patientModel.PatientDossier.Treatments.Add(collection.Treatment);

            _patientRepo.Update(patientModel, id);

            return RedirectToAction("Details", "PatientModels", new { id });
        }

        // GET: PatientTreatmentModelsController/Edit/5
        public ActionResult Edit(int id, int treatmentId)
        {
            TreatmentFormViewModel treatmentFormViewModel = new TreatmentFormViewModel();

            treatmentFormViewModel.Therapists = _therapistRepo.GetTherapists();
            treatmentFormViewModel.Id = id;
            treatmentFormViewModel.Treatment = _patientRepo.Get(id).PatientDossier.Treatments.Find(t => t.Id == treatmentId);

            return View(treatmentFormViewModel);
        }

        // POST: PatientTreatmentModelsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, int treatmentId, TreatmentFormViewModel collection)
        {
            if (!ModelState.IsValid)
            {
                collection.Therapists = _therapistRepo.GetTherapists();
                return View(collection);
            }

            PatientModel patient = _patientRepo.Get(id);

            // TODO:: Uitvinden hoe je dit in een keer doet..
            var treatment = patient.PatientDossier.Treatments.Find(t => t.Id == treatmentId);

            patient.PatientDossier.Treatments.Remove(treatment);

            treatment = collection.Treatment;
            treatment.TreatmentDoneBy = _therapistRepo.Get(collection.TherapyDoneById);

            patient.PatientDossier.Treatments.Add(treatment);

            _patientRepo.Update(patient, id);

            return RedirectToAction("Details", "PatientModels", new { id });
        }
        public ActionResult Delete(int id, int treatmentId)
        {
            PatientModel patient = _patientRepo.Get(id);

            patient.PatientDossier.Treatments.Remove(patient.PatientDossier.Treatments.Find(t => t.Id == treatmentId));
            _patientRepo.Update(patient, id);
            return RedirectToAction("Details", "PatientModels", new { id });
        }


        //public IActionResult AddTreatment(PatientDossierViewModel model, int id)
        //{
        //    PatientModel patientModel = _patientRepo.Get(id);
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Details", FillPatientDossierViewModel(id));
        //    }
        //    if (patientModel.PatientDossier.Treatments == null)
        //    {
        //        patientModel.PatientDossier.Treatments = new List<TreatmentModel>();
        //    }
        //    if (CheckIfTeacher(model.TreatmentDoneById) == true)
        //    {
        //        model.TreatmentModel.TreatmentDoneBy = _teacherRepo.Get(model.TreatmentDoneById);
        //    }
        //    else
        //    {
        //        model.TreatmentModel.TreatmentDoneBy = _studentRepo.Get(model.TreatmentDoneById);
        //    }

        //    patientModel.PatientDossier.Treatments.Add(model.TreatmentModel);

        //    _patientRepo.Update(patientModel, id);

        //    // TODO:: find out if there is a better way to do this
        //    return RedirectToAction("Details", new { id });
        //}
        //// TODO:: Maybe refactor to different controller
        //public IActionResult RemoveTreatment(int dossierId, int treatmentId, int id)
        //{
        //    PatientModel patientModel = _patientRepo.Get(id);
        //    patientModel.PatientDossier.Treatments.Remove(patientModel.PatientDossier.Treatments.Find(x => x.Id == treatmentId));

        //    _patientRepo.Update(patientModel, dossierId);
        //    return RedirectToAction("Details", new { id });
        //}
    }
}
