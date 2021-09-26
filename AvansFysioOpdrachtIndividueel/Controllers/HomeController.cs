using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo<PatientModel> _patientRepo;
        private readonly FysioDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        #region Home
        public HomeController(
            ILogger<HomeController> logger,
            IRepo<PatientModel> patientRepo,
            FysioDBContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _logger = logger;
            _patientRepo = patientRepo;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // Komt een verzoek binnen vanuit de index
        public IActionResult Index()
        {
            // Maak gebruik van de model
            PersonViewModelLists personViewModel = new PersonViewModelLists();
            // Doe een data verzoek naar een Repository en vul de model
            personViewModel.PatientModels = _patientRepo.Get();
            // Roep view aan en stuur data door naar view
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
        #endregion
        #region Authentication
        public IActionResult Authenticate()
        {
            var cookieClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Maurice"),
                new Claim(ClaimTypes.Email, "mauricederidder@outlook.com"),
                new Claim("Login.Says", "Allowed to pass"),
            };

            var userIdentity = new ClaimsIdentity(cookieClaims, "Login");

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
        public IActionResult Login(string userName, string password)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Register(string userName, string password)
        {

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        #endregion
    }
}
