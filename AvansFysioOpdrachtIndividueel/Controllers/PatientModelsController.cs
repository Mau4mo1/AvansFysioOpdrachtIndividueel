using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Authorization;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Identity;
using Core.Data;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class PatientModelsController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ITherapistRepo _therapistRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepo<PersonModel> _personRepo;
        public PatientModelsController( IPatientRepo repo, ITherapistRepo therapistRepo, UserManager<IdentityUser> userManager, IRepo<PersonModel> personRepo)
        {
            _patientRepo = repo;
            _therapistRepo = therapistRepo;
            _userManager = userManager;
            _personRepo = personRepo;
        }

        // GET: PatientModels
        [Authorize(Roles = "Patient,Therapist")]
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

                    if (_personRepo.Get().Exists(t => t?.Email == patientModel.Email))
                    {
                        ModelState.AddModelError(String.Empty, "De email bestaat al.");
                        return View();
                    }

                    _patientRepo.Create(patientModel);
                    return RedirectToAction(nameof(Index));

                // if it fails that means there are no persons in the database
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
                _patientRepo.Update(patientModel,id);
                return RedirectToAction(nameof(Index));
            }
            return View(patientModel);
        }

        // GET: PatientModels/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
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
        // POST: PatientModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _patientRepo.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddComment(int id,PatientDossierViewModel patientDossierViewModel)
        {
            if (ModelState.IsValid)
            {
                PatientModel patientModel = _patientRepo.Get(id);

                // Gets and sets the currently logged in user to the author.
                var user = await _userManager.GetUserAsync(HttpContext.User);
                patientDossierViewModel.Comment.CommentMadeBy = _therapistRepo.Get().Where(p => p?.Email == user?.UserName).FirstOrDefault();

                patientDossierViewModel.Comment.TimeOfCreation = DateTime.Now;
                patientModel.PatientDossier.ExtraComments.Add(patientDossierViewModel.Comment);

                _patientRepo.UpdatePatientDossier(patientModel, id);

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

            patientModel.Therapists = _therapistRepo.GetTherapists();

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
