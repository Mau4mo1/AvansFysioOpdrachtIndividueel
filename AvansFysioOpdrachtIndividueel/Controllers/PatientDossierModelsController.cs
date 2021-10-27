using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientDossierModelsController : Controller
    {

        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly IAsyncRepo<DiagnosisModel> _diagnosisRepo;

        public PatientDossierModelsController(IPatientRepo patientRepo, ITherapistRepo therapistRepo, IAsyncRepo<DiagnosisModel> diagnosisRepo)
        {
            _patientRepo = patientRepo;
            _therapistRepo = therapistRepo;
            _diagnosisRepo = diagnosisRepo;
        }

        // GET: PatientDossierModelsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PatientDossierModelsController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: PatientDossierModelsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientDossierModelsController/Create
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

        // GET: PatientDossierModelsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await FillPatientDossierDetailsViewModel(id));
        }

        // POST: PatientDossierModelsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PatientDossierDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PatientModel patient = _patientRepo.Get(id);

                patient.PatientDossier.Therapist = _therapistRepo.Get(viewModel.TherapistId);
                patient.PatientDossier.IntakeDoneBy = _therapistRepo.Get(viewModel.IntakeDoneById);
                patient.PatientDossier.IntakeSupervisedBy = _therapistRepo.Get(viewModel.SupervisedById);

                patient.PatientDossier.PlannedDate = viewModel.PlannedDate;
                patient.PatientDossier.DueDate = viewModel.DueDate;
                patient.PatientDossier.IssueDescription = viewModel.IssueDescription;
                patient.PatientDossier.DiagnosisCode = new DiagnosisModel
                {
                    CodeAndDescription = viewModel.DiagnosisCode
                };
                _patientRepo.Update(patient, id);

                return RedirectToAction("Details", controllerName: "PatientModels", new { id });
            }
            else
            {                
                return View(await FillPatientDossierDetailsViewModel(id));
            }
        }

        // GET: PatientDossierModelsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientDossierModelsController/Delete/5
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
        public async Task<PatientDossierDetailsViewModel> FillPatientDossierDetailsViewModel(int id)
        {
            var patientModel = _patientRepo.Get(id);
            
            PatientDossierDetailsViewModel model = new PatientDossierDetailsViewModel
            {
                Name = patientModel.Name,
                Id = id,
                IssueDescription = patientModel.PatientDossier.IssueDescription,
                //DiagnosisCode = patientModel.PatientDossier.DiagnosisCode.CodeAndDescription,
                DueDate = patientModel.PatientDossier.DueDate,
                PlannedDate = patientModel.PatientDossier.PlannedDate,
                Therapist = patientModel.PatientDossier.Therapist,
                IntakeSupervisedBy = patientModel.PatientDossier.IntakeSupervisedBy,
                IntakeDoneBy = patientModel.PatientDossier.IntakeDoneBy,
                Therapists = _therapistRepo.GetTherapists()
            };
            var result = await _diagnosisRepo.Get();
            model.DiagnosisCodes = new List<SelectListItem>();
            // model.DiagnosisCodes.Add(new SelectListitem());
            foreach (var item in result)
            {
                model.DiagnosisCodes.Add(new SelectListItem(item.CodeAndDescription,item.CodeAndDescription));
            }
            
            if(patientModel.PatientDossier.DiagnosisCode != null)
            {
                model.DiagnosisCode = patientModel.PatientDossier.DiagnosisCode.CodeAndDescription;
            }

            return model;
        }
    }
}
