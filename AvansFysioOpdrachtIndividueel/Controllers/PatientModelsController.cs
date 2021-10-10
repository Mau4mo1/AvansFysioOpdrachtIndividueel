using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Authorization;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientModelsController : Controller
    {
        private readonly FysioDBContext _context;
        private readonly IRepo<PatientModel> _patientRepo;
        private readonly IRepo<StudentModel> _studentRepo;
        private readonly IRepo<TeacherModel> _teacherRepo;
        public PatientModelsController(FysioDBContext context, IRepo<PatientModel> repo, IRepo<TeacherModel> teacherRepo, IRepo<StudentModel> studentRepo)
        {
            _context = context;
            _patientRepo = repo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
        }

        // GET: PatientModels
        [Authorize(Roles = "Patient")]
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
            PatientDossierViewModel patientModel = FillPatientDossierViewModel(id);
            if(patientModel == null)
            {
                return NotFound();
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
                patientModel.PatientDossier = new PatientDossierModel();
                patientModel.PatientDossier.ExtraComments = new List<CommentModel>();
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
            patientModel.PatientDossier.PlannedDate = model.PatientModel.PatientDossier.PlannedDate;
            patientModel.PatientDossier.DueDate = model.PatientModel.PatientDossier.DueDate;
            patientModel.PatientDossier.ExtraComments = model.PatientModel.PatientDossier.ExtraComments;
            patientModel.PatientDossier.IssueDescription = model.PatientModel.PatientDossier.IssueDescription;
            patientModel.PatientDossier.DiagnosisCode = model.PatientModel.PatientDossier.DiagnosisCode;

            // TODO:: Find a better way? Right now it checks if the intake helpers are a student of a therapist. 
            if (CheckIfTeacher(model.IntakeDoneById) == true)
            {
                patientModel.PatientDossier.IntakeDoneBy = _teacherRepo.Get(model.IntakeDoneById);
            }
            else
            {
                patientModel.PatientDossier.IntakeDoneBy = _studentRepo.Get(model.IntakeDoneById);
            }
            if (CheckIfTeacher(model.SupervisedById) == true)
            {
                patientModel.PatientDossier.IntakeSupervisedBy = _teacherRepo.Get(model.SupervisedById);
            }
            else
            {
                patientModel.PatientDossier.IntakeSupervisedBy = _studentRepo.Get(model.SupervisedById);
            }
            if (CheckIfTeacher(model.TherapistId) == true)
            {
                patientModel.PatientDossier.Therapist = _teacherRepo.Get(model.TherapistId);
            }
            else
            {
                patientModel.PatientDossier.Therapist = _studentRepo.Get(model.TherapistId);
            }

            _patientRepo.Update(patientModel, id);
            return RedirectToAction("Details", new { id });
            
        }
        public IActionResult AddTreatment(PatientDossierViewModel model, int id)
        {
            PatientModel patientModel = _patientRepo.Get(id);
            if (!ModelState.IsValid)
            {
                return View("Details", FillPatientDossierViewModel(id));
            }
            if (patientModel.PatientDossier.Treatments == null)
            {
                patientModel.PatientDossier.Treatments = new List<TreatmentModel>();
            }
            if (CheckIfTeacher(model.TreatmentDoneById) == true)
            {
                model.TreatmentModel.TreatmentDoneBy = _teacherRepo.Get(model.TreatmentDoneById);
            }
            else
            {
                model.TreatmentModel.TreatmentDoneBy = _studentRepo.Get(model.TreatmentDoneById);
            }
            
            patientModel.PatientDossier.Treatments.Add(model.TreatmentModel);

            _patientRepo.Update(patientModel, id);

            // TODO:: find out if there is a better way to do this
            return RedirectToAction("Details", new { id });
        }

        // TODO:: Maybe refactor to different controller

        public IActionResult RemoveTreatment(int dossierId, int treatmentId, int id)
        {
            PatientModel patientModel = _patientRepo.Get(id);
            patientModel.PatientDossier.Treatments.Remove(patientModel.PatientDossier.Treatments.Find(x => x.Id == treatmentId));

            _patientRepo.Update(patientModel, dossierId);
            return RedirectToAction("Details", new { id });
        }
        // TODO:: Definetly find a way to not need this function anymore
        public bool CheckIfTeacher(int id)
        {
            var isTeacher = _teacherRepo.Get(id);

            if (isTeacher != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult AddComment(int id,PatientDossierViewModel patientDossierViewModel)
        {
            if (ModelState.IsValid)
            {
                PatientModel patientModel = _patientRepo.Get(id);
                // TODO:: change this to the current logged in user.
                patientDossierViewModel.Comment.CommentMadeBy = _patientRepo.Get(id);
                patientDossierViewModel.Comment.TimeOfCreation = DateTime.Now;
                patientModel.PatientDossier.ExtraComments.Add(patientDossierViewModel.Comment);


                _patientRepo.Update(patientModel, id);

                return RedirectToAction("Details", new { id });
            }
            else
            {
                // Need to return view, otherwise modelstate gets lost
                return View("Details", FillPatientDossierViewModel(id));
            }
           
        }

        public PatientDossierViewModel FillPatientDossierViewModel(int id)
        {
            PatientDossierViewModel patientModel = new();
            patientModel.PatientModel = _patientRepo.Get(id);
            // Make the selectitems only therapists. 
            // For this we need a therapist repo
            // TODO:: Find a better way to convert these 2 lists into personmodels..

            List<TeacherModel> teacherModels = _teacherRepo.Get();
            patientModel.Therapists = new List<SelectListItem>();
            foreach (var teacherModel in teacherModels)
            {
                patientModel.Therapists.Add(new SelectListItem { Text = teacherModel.Name, Value = teacherModel.Id.ToString() });
            }

            List<StudentModel> studentModels = _studentRepo.Get();

            foreach (var studentModel in studentModels)
            {
                patientModel.Therapists.Add(new SelectListItem { Text = studentModel.Name, Value = studentModel.Id.ToString() });
            }

            try
            {
                patientModel.TreatmentModels = _patientRepo.Get(id).PatientDossier.Treatments.ToList();
            }
            catch (Exception)
            {

            }
            return patientModel;
        }
    }
}
