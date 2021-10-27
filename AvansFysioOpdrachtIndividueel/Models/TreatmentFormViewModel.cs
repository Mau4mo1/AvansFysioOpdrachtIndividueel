using Core.Domain.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TreatmentFormViewModel
    {
        [Required]
        public TreatmentModel Treatment { get; set; }
        public List<SelectListItem> Therapists { get; set; }

        public VektisModel VektisType { get; set; }
        public List<SelectListItem> VektisTypes { get; set; }
        [Required]
        public int TherapyDoneById { get; set; }
        public int Id { get;set; }

    }
}
