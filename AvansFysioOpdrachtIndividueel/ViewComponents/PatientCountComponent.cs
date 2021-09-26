using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.ViewComponents
{
    [ViewComponent(Name = "PatientCount")]
    public class PatientCountComponent : ViewComponent
    {
        IRepo<PatientModel> _repo;
        public PatientCountComponent(IRepo<PatientModel> _repo)
        {
            this._repo = _repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.PatientCount = _repo.Get().ToList().Count;

            return View("PatientCount");
        }
    }
}
