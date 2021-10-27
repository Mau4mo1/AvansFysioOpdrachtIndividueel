using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
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
    public class PatientTreatmentModelsController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly ITreatmentManager _treatmentManager;
        private readonly IVektisRepo _vektisRepo;
        private readonly UserManager<IdentityUser> _userManager;
        public PatientTreatmentModelsController(IPatientRepo patientRepo, ITherapistRepo therapistRepo, ITreatmentManager treatmentManager, IVektisRepo vektisRepo, UserManager<IdentityUser> userManager)
        {
            _patientRepo = patientRepo;
            _therapistRepo = therapistRepo;
            _treatmentManager = treatmentManager;
            _vektisRepo = vektisRepo;
            _userManager = userManager;
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
        public async Task<ActionResult> Create(int id)
        {
            TreatmentFormViewModel treatmentFormViewModel = new TreatmentFormViewModel();

            treatmentFormViewModel.Therapists = _therapistRepo.GetTherapists();
            treatmentFormViewModel.VektisTypes = await _vektisRepo.GetVekti();
            treatmentFormViewModel.Id = id;

            return View(treatmentFormViewModel);
        }

        // POST: PatientTreatmentModelsController/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public async Task<ActionResult> Create(TreatmentFormViewModel collection, int id)
        {
            if (!ModelState.IsValid)
            {
                collection.Therapists = _therapistRepo.GetTherapists();
                collection.VektisTypes = await _vektisRepo.GetVekti();
                return View(collection);
            }

            if (!_treatmentManager.IsWithinAllowedTreatmentAmount(id, collection.Treatment))
            {
                return await SendBackToView("Je mag niet meer behandelingen aanmaken dan in je behandelplan staat.", collection);
            }

            PatientModel patientModel = _patientRepo.Get(id);
            // check if the patient has been registered.

            var userResult = await _userManager.FindByEmailAsync(patientModel.Email);

            if(userResult == null)
            {
                return await SendBackToView("De gebruik moet geregistreerd zijn voor een behandeling toegevoegd mag worden!", collection);
            }
            // Fill in the end of the treatment datetime by taking the treatmentplan TimeOfTreatment in minutes and adding it to the starting treatment time.
            collection.Treatment.TreatmentUntil = collection.Treatment.TreatmentTime.AddMinutes(patientModel.PatientDossier.TreatmentPlan.TimeOfTreatment);

            if (!_treatmentManager.IsTherapistAvailable(collection.TherapyDoneById, collection.Treatment))
            {
                return await SendBackToView("De therapeut is niet beschikbaar op deze tijd.", collection);
            }

            // get the vektis type 
            var result = await _vektisRepo.Get(collection.Treatment.VektisType.Id);
            // if it needs a description, check if the collection has a description
            if (result.NeedsDescription)
            {
                // if there is no description send back an error
                try
                {
                    if (collection.Treatment.Description.Length < 0)
                    {
                        return await SendBackToView("Dit vektysType heeft een beschrijving nodig", collection);
                    }
                }
                catch(Exception ex)
                {
                    return await SendBackToView("Dit vektysType heeft een beschrijving nodig", collection);
                }
            }
            collection.Treatment.VektisType = new VektisModel{
                Value = result.Value,
                NeedsDescription = result.NeedsDescription
            };
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
        public async Task<ActionResult> Edit(int id, int treatmentId, TreatmentFormViewModel collection)
        {
            PatientModel patient = _patientRepo.Get(id);
            var treatment = patient.PatientDossier.Treatments.Find(t => t.Id == treatmentId);

            if (!ModelState.IsValid)
            {
                collection.Therapists = _therapistRepo.GetTherapists();
                collection.Id = id;
                collection.Treatment = treatment;
                return View(collection);
            }
            
            if (DateTime.Now.Subtract(treatment.TreatmentTime).TotalHours > -24)
            { 
                return await SendBackToView("Je kan een behandeling niet 24 van te voren bewerken of verwijderen.", collection);
            }

            // get the vektis type 
            var result = await _vektisRepo.Get(collection.Treatment.VektisType.Id);
            // if it needs a description, check if the collection has a description
            if (result.NeedsDescription)
            {
                // if there is no description send back an error
                if (collection.Treatment.Description.Length < 0)
                {
                    return await SendBackToView("Dit vektysType heeft een beschrijving nodig", collection);
                }
            }
            collection.Treatment.VektisType = result;

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
            
            var result = patient.PatientDossier.Treatments.Find(t => t.Id == treatmentId);
            if (DateTime.Now.Subtract(result.TreatmentTime).TotalHours > -24)
            {
                ModelState.AddModelError("Delete", "Je kan een behandeling niet 24 van te voren bewerken of verwijderen.");
                return RedirectToAction("Details", "PatientModels", new { id });
            }

            patient.PatientDossier.Treatments.Remove(result);
            _patientRepo.Update(patient, id);
            return RedirectToAction("Details", "PatientModels", new { id });
            
        }

        public async Task<ViewResult> SendBackToView(string errorMessage, TreatmentFormViewModel collection)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            collection.Therapists = _therapistRepo.GetTherapists();
            collection.VektisType = new VektisModel {
                Id = collection.Treatment.VektisType.Id,
                NeedsDescription = collection.Treatment.VektisType.NeedsDescription,
                Value = collection.Treatment.VektisType.Value
            };
            collection.VektisTypes = await _vektisRepo.GetVekti();
            return View(collection);
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
