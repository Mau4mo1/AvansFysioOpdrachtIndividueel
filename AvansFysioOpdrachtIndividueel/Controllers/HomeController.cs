
using AvansFysioOpdrachtIndividueel.Models;
using Core.Data;
using Core.DomainServices;
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
        private readonly IPatientRepo _patientRepo;
        private readonly FysioDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        #region Home
        public HomeController(
            ILogger<HomeController> logger,
            IPatientRepo patientRepo,
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

            UserDBInitialiser.SeedUsers(_userManager);
        }
        // Komt een verzoek binnen vanuit de index
        public async Task<ViewResult> Index()
        {
            // Maak gebruik van de model
            PersonViewModelLists personViewModel = new PersonViewModelLists();
            // Doe een data verzoek naar een Repository en vul de model
            personViewModel.PatientModels = _patientRepo.Get();
            // Roep view aan en stuur data door naar view
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                //TODO:: Find a way to not have to do this?
                personViewModel.PersonViewModel = new PersonViewModel();
                personViewModel.PersonViewModel.PatientModel = _patientRepo.Get().Where(p => p?.Email == user?.UserName).FirstOrDefault();
            }
            
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            // TODO:: Find better way
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                // sign in
                var signInResult = await _signInManager.PasswordSignInAsync(userName, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, signInResult.ToString());
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {

            var user = new IdentityUser()
            {
                UserName = userName
            };

            if (password == null)
            {
                ModelState.AddModelError("PasswordNull", "Vul aub een wachtwoord in.");
                return View();
            }
            var patient = _patientRepo.Get().Where(p => p.Email == userName).FirstOrDefault();
            if (patient != null)
            {
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var currentUser = _userManager.FindByNameAsync(user.UserName);
                    var roleresult =  await _userManager.AddToRoleAsync(currentUser.Result, "Patient");
                    // sign in
                    var signInResult = await _signInManager.PasswordSignInAsync(userName, password, false, false);

                    return RedirectToAction("Index");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("UserError", item.Description);
                }

                return View();
            }
            else
            {
                ModelState.AddModelError("MatchingMail", "Er is geen matchende email gevonden");
                return View();
            }

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        //  [Authorize(Roles = "Patient")]
        public IActionResult Secret()
        {

            return View();

        }
        #endregion
    }
}
