using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TreatmentPlanModel
    {
        public int Id {  get; set; }
        public int AmountOfTreaments { get; set; }
        public string TimeOfTreatment { get; set; }
    }
}
