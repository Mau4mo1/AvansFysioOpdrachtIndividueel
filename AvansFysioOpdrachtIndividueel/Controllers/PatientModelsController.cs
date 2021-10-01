using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientModelsController : Controller
    {
        private readonly FysioDBContext _context;
        private readonly IRepo<PatientModel> _patientRepo;

        public PatientModelsController(FysioDBContext context, IRepo<PatientModel> repo)
        {
            _context = context;
            _patientRepo = repo;
        }

        // GET: PatientModels
        public IActionResult Index()
        {
            return View(_patientRepo.Get().ToList());
        }
        // GET: PatientModels/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            PatientDossierViewModel patientModel = new();
            patientModel.PatientModel = _patientRepo.Get(id);
            patientModel.SelectItems = _patientRepo.Get();
            if (patientModel == null)
            {
                return NotFound();
            }
            try
            {
                patientModel.TreatmentModels = _patientRepo.Get(id).PatientDossier.Treatments.ToList();
            }
            catch (Exception)
            {

            }

            return View(patientModel);
        }

        // GET: PatientModels/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: PatientModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PatientNumber,DateOfBirth,Gender,Name,Email")] PatientModel patientModel)
        {
            if (ModelState.IsValid)
            {
                _patientRepo.Create(patientModel);
                return RedirectToAction(nameof(Index));
            }
            return View(patientModel);
        }

        // GET: PatientModels/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var patientModel = _patientRepo.Get(id);

            if (patientModel == null)
            {
                return NotFound();
            }
            return View(patientModel);
        }

        // POST: PatientModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientNumber,DateOfBirth,Gender,Id,Name,Email")] PatientModel patientModel)
        {
            if (id != patientModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientModelExists(patientModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patientModel);
        }

        // GET: PatientModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientModel = await _context.patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientModel == null)
            {
                return NotFound();
            }

            return View(patientModel);
        }
        // POST: PatientModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientModel = await _context.patients.FindAsync(id);
            _context.patients.Remove(patientModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientModelExists(int id)
        {
            return _context.patients.Any(e => e.Id == id);
        }

        public IActionResult AddDossier(PatientDossierViewModel model, int id)
        {

            PatientModel patientModel = _patientRepo.Get(id);
            //patientModel.PatientDossier 
            patientModel.PatientDossier.DueDate = model.PatientModel.PatientDossier.DueDate;
            patientModel.PatientDossier.ExtraComments = model.PatientModel.PatientDossier.ExtraComments;
            patientModel.PatientDossier.IssueDescription = model.PatientModel.PatientDossier.IssueDescription;
            patientModel.PatientDossier.DiagnosisCode = model.PatientModel.PatientDossier.DiagnosisCode;

            // TODO:: fix this hacky solution to model binding select boxes
            // this checks if the default option was selected. if that is the case we will fill it with what it used to be, but this is not very air tight
            if (model.IntakeDoneById == 0)
            {
                model.IntakeDoneById = patientModel.PatientDossier.IntakeDoneBy.Id;
            }
            if (model.TherapistId == 0)
            {
                model.TherapistId = patientModel.PatientDossier.Therapist.Id;
            }
            if (model.SupervisedById == 0)
            {
                model.SupervisedById = patientModel.PatientDossier.IntakeSupervisedBy.Id;
            }

            patientModel.PatientDossier.IntakeDoneBy = _patientRepo.Get(model.IntakeDoneById);
            patientModel.PatientDossier.IntakeSupervisedBy = _patientRepo.Get(model.SupervisedById);
            patientModel.PatientDossier.Therapist = _patientRepo.Get(model.TherapistId);

            _patientRepo.Update(patientModel, id);
            return RedirectToAction("Details", new { id });
        }
        public IActionResult AddTreatment(PatientDossierViewModel model, int id)
        {
            PatientModel patientModel = _patientRepo.Get(id);
            if (patientModel.PatientDossier.Treatments == null)
            {
                patientModel.PatientDossier.Treatments = new List<TreatmentModel>();
            }
            model.TreatmentModel.TreatmentDoneBy = _patientRepo.Get(model.TreatmentDoneById);
            patientModel.PatientDossier.Treatments.Add(model.TreatmentModel);

            _patientRepo.Update(patientModel, id);

            // TODO:: find out if there is a better way to do this
            return RedirectToAction("Details", new { id });
        }
    }
}
