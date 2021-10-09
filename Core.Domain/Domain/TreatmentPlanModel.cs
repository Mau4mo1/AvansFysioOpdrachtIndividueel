using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TreatmentPlanModel
    {
        public int Id {  get; set; }
        [Required]
        public int AmountOfTreaments { get; set; }
        [Required]
        public string TimeOfTreatment { get; set; }
    }
}
