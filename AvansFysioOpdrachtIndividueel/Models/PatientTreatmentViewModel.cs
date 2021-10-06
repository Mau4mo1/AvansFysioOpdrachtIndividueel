using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientTreatmentViewModel
    {
        // general patient info we want to show on the page
        public string Name { get; set; }
        public int Id { get; set; }
        // treatments
        public List<TreatmentModel> Treatments {  get; set; }
    }
}
