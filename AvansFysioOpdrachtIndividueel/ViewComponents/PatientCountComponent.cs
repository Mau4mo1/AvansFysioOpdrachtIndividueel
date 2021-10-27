using AvansFysioOpdrachtIndividueel.Models;
using Core.DomainServices;
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
        IPatientRepo _repo;
        public PatientCountComponent(IPatientRepo _repo)
        {
            this._repo = _repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.PatientCount =  _repo.Get().ToList().Count;

            return View("PatientCount");
        }
    }
}
