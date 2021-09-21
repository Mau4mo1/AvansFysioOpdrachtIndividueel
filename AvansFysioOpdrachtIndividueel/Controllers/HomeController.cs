using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo<PatientModel> _patientRepo;
        public HomeController(ILogger<HomeController> logger, IRepo<PatientModel> patientRepo)
        {
            _logger = logger;
            _patientRepo = patientRepo;
        }

        public IActionResult Index()
        {
            PersonViewModelLists personViewModel = new PersonViewModelLists();
            personViewModel.PatientModels = _patientRepo.Get();
            return View(personViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Patient CRUD
        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPatient(PersonViewModel viewModel)
        {
            PatientModel newPatient = new PatientModel(
                viewModel.PatientModel.PatientNumber, 
                viewModel.PatientModel.DateOfBirth,
                viewModel.PatientModel.Gender,
                viewModel.Id,
                viewModel.Name,
                viewModel.Email
                );
            _patientRepo.Create(newPatient);
            return RedirectToAction("Index");
        }

        // PatientDossier CRUD
        [HttpGet]
        public IActionResult AddPatientDossier()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPatientDossier(PatientModel patient)
        {

            _patientRepo.Create(patient);
            return Redirect("Index");
        }
    }
}
